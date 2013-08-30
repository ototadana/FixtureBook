/*
 * Copyright 2013 XPFriend Community.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class TempConductor : IConductor
    {
        private Case testCase;

        public TempConductor() { }

        public void Initialize(Case testCase)
        {
            this.testCase = testCase;
        }

        public void ExpectThrown<TException>(Delegate action, params Type[] types) where TException : Exception
        {
            testCase.Setup();
            testCase.Validate<TException>(() => DynamicInvoke(action, types), null);
            testCase.ValidateStorageInternal();
        }

        private object DynamicInvoke(Delegate action, Type[] types)
        {
            try
            {
                return action.DynamicInvoke(GetParameters(types));
            }
            catch (TargetInvocationException e)
            {
                Loggi.Debug(e);
                throw e.InnerException;
            }
        }

        public void ExpectReturn(Delegate func, params Type[] types)
        {
            testCase.Setup();
            object result = DynamicInvoke(func, types);
            Validate(ToResult(result));
            testCase.ValidateStorageInternal();
        }

        public void Expect(Delegate action, params Type[] types)
        {
            testCase.Setup();
            DynamicInvoke(action, types);
            testCase.ValidateStorageInternal();
        }

        private object[] GetParameters(Type[] types)
        {
            object[] parameters = new object[types.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = GetParameter(types[i], i);
            }
            return parameters;
        }

        private object GetParameter(Type type, int index)
        {
            string tableName = GetTableName(index, Section.SectionType.ObjectForExec);
            return GetParameter(type, tableName);
        }

        private Result ToResult(object result)
        {
            string tableName = GetTableName(0, Section.SectionType.ExpectedResult);
            return new Result { Name = tableName, Value = result };
        }

        private void Validate(Result parameter)
        {
            Section section = testCase.GetSection(Section.SectionType.ExpectedResult);
            if (section != null && section.HasTable(parameter.Name))
            {
                if (parameter.Value is DataSet)
                {
                    testCase.Validate(parameter.Value);
                }
                else
                {
                    testCase.Validate(parameter.Value, parameter.Name);
                }
            }
        }

        private string GetTableName(int index, Section.SectionType sectionType)
        {
            Section section = testCase.GetSection(sectionType);
            if (section == null)
            {
                throw new ConfigException("M_Fixture_FixtureBook_GetSection_" + sectionType, testCase);
            }

            if (!section.HasTable(index))
            {
                throw new ConfigException("M_Fixture_FixtureBook_GetTable_" + sectionType, index + 1, section);
            }
            return section.GetTable(index).Name;
        }

        private object GetParameter(Type type, string tableName)
        {
            if (type.IsArray)
            {
                Type objectType = type.GetElementType();
                return InvokeMethod(objectType, "GetArray", tableName);
            }

            if (type.IsGenericType &&
                typeof(List<>).Equals(type.GetGenericTypeDefinition()))
            {
                Type objectType = type.GetGenericArguments()[0];
                return InvokeMethod(objectType, "GetList", tableName);
            }

            if (typeof(DataSet).IsAssignableFrom(type))
            {
                return InvokeMethod(type, "GetObject", new string[]{});
            }
            return InvokeMethod(type, "GetObject", new string[]{tableName});
        }

        private object InvokeMethod(Type objectType, string methodName, object parameter)
        {
            MethodInfo method = testCase.GetType().GetMethod(methodName);
            object[] parameters = new object[] { parameter };
            return method.MakeGenericMethod(objectType).Invoke(testCase, parameters);
        }

        internal class Result
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }
    }
}
