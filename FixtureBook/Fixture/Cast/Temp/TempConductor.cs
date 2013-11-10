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
using System.Text;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;
using XPFriend.Fixture.Toolkit;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class TempConductor : IConductor
    {
        private Case testCase;
        private object[] parameters;
        private string[] parameterNames;

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
            this.parameters = parameters;
            this.parameterNames = new string[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = GetParameter(types[i], i);
            }
            return parameters;
        }

        private object GetParameter(Type type, int index)
        {
            string tableName = GetTableName(index, Section.SectionType.ObjectForExec);
            parameterNames[index] = tableName;
            return GetParameter(type, tableName);
        }

        private Result ToResult(object result)
        {
            string tableName = GetTableName(0, Section.SectionType.ExpectedResult);
            return new Result { Name = tableName, Value = result };
        }

        private void Validate(Result result)
        {
            Section section = testCase.GetSection(Section.SectionType.ExpectedResult);
            if (section != null && section.HasTable(result.Name))
            {
                if (result.Value is DataSet)
                {
                    testCase.Validate(result.Value);
                }
                else
                {
                    testCase.Validate(result.Value, result.Name);
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


        public void ValidateParameterAt(int index, string name)
        {
            AssertParameterIndex(index, "ValidateParameterAt");
            testCase.Validate(parameters[index], name);
        }

        public void ValidateParameterAt(int index)
        {
            AssertParameterIndex(index, "ValidateParameterAt");
            testCase.Validate(parameters[index], parameterNames[index]);
        }

        public T GetParameterAt<T>(int index)
        {
            AssertParameterIndex(index, "GetParameterAt");
            return (T)parameters[index];
        }

        private void AssertParameterIndex(int index, string methodName)
        {
            if (parameters == null)
            {
                throw new ConfigException("M_Fixture_Temp_Conductor_InvalidStatus", methodName);
            }

            if (index >= parameters.Length)
            {
                throw new ConfigException("M_Fixture_Temp_Conductor_InvalidParameterIndex", index);
            }
        }

        public void Expect(Type targetClass, string targetMethod, Type[] targetMethodParameter)
        {
            testCase.Setup();
            DynamicInvokeInternal(targetClass, targetMethod, targetMethodParameter);
            testCase.ValidateStorageInternal();
        }

        private object DynamicInvoke(Type targetClass, string targetMethod, Type[] targetMethodParameter)
        {
            try
            {
                return DynamicInvokeInternal(targetClass, targetMethod, targetMethodParameter);
            }
            catch (TargetInvocationException e)
            {
                Loggi.Debug(e);
                throw e.InnerException;
            }
        }

        private object DynamicInvokeInternal(Type targetClass, string targetMethod, Type[] targetMethodParameter)
        {
            MethodInfo method = GetMethod(targetClass, targetMethod, targetMethodParameter);
            object instance = null;
            if (!method.IsStatic)
            {
                instance = NewInstance(targetClass);
            }
            return method.Invoke(instance, GetParameters(method.GetParameters()));
        }

        private object[] GetParameters(ParameterInfo[] parameterInfo)
        {
            Type[] types = new Type[parameterInfo.Length];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = parameterInfo[i].ParameterType;
            }
            return GetParameters(types);
        }

        private object NewInstance(Type targetClass)
        {
            try
            {
                return Activator.CreateInstance(targetClass);
            }
            catch (Exception e)
            {
                throw new ConfigException(e, "M_Fixture_Temp_Conductor_CannotCreateInstance", targetClass.FullName);
            }
        }

        private MethodInfo GetMethod(Type targetClass, string targetMethod, Type[] targetMethodParameter)
        {
            if (targetMethodParameter != null && targetMethodParameter.Length > 0)
            {
                MethodInfo method = targetClass.GetMethod(targetMethod, targetMethodParameter);
                if (method == null)
                {
                    throw new ConfigException("M_Fixture_Temp_Conductor_CannotFindMethod",
                        targetClass.FullName, GetMethodName(targetMethod, targetMethodParameter));
                }
                return method;
            }

            {
                MethodInfo method = MethodFinder.FindMethod(targetClass, targetMethod);
                if (method == null)
                {
                    throw new ConfigException("M_Fixture_Temp_Conductor_CannotFindMethod",
                        targetClass.FullName, targetMethod);
                }
                return method;
            }
        }

        private object GetMethodName(string targetMethod, Type[] targetMethodParameter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(targetMethod).Append("(");
            for (int i = 0; i < targetMethodParameter.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(targetMethodParameter[i].FullName);
            }
            sb.Append(")");
            return sb;
        }

        public void ExpectReturn(Type targetClass, string targetMethod, Type[] targetMethodParameter)
        {
            testCase.Setup();
            object result = DynamicInvokeInternal(targetClass, targetMethod, targetMethodParameter);
            Validate(ToResult(result));
            testCase.ValidateStorageInternal();
        }

        public void ExpectThrown<TException>(Type targetClass, string targetMethod, Type[] targetMethodParameter) where TException : Exception
        {
            testCase.Setup();
            testCase.Validate<TException>(() => 
                DynamicInvoke(targetClass, targetMethod, targetMethodParameter), null);
            testCase.ValidateStorageInternal();
        }
    }
}
