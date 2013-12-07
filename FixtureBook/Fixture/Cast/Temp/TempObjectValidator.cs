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
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class TempObjectValidator : IObjectValidator
    {
        internal DictionaryValidator dictionaryValidator;
        internal PocoValidator pocoValidator;
        internal DataSetValidator dataSetValidator;
        internal DataTableValidator dataTableValidator;
        private Case testCase;

        public TempObjectValidator()
        {
            dictionaryValidator = new DictionaryValidator(this);
            pocoValidator = new PocoValidator(this);
            dataSetValidator = new DataSetValidator(this);
            dataTableValidator = new DataTableValidator(this);
        }

        public void Initialize(Case testCase)
        {
            dictionaryValidator.Initialize(testCase);
            pocoValidator.Initialize(testCase);
            dataSetValidator.Initialize(testCase);
            dataTableValidator.Initialize(testCase);
            this.testCase = testCase;
        }

        public bool HasRole(object obj, params string[] typeName)
        {
            return pocoValidator.HasRole(obj, typeName) ||
                dataSetValidator.HasRole(obj, typeName) ||
                dataTableValidator.HasRole(obj, typeName) ||
                dictionaryValidator.HasRole(obj, typeName);
        }

        public void Validate(object obj, params string[] typeName)
        {
            if (dataSetValidator.HasRole(obj, typeName))
            {
                dataSetValidator.Validate(obj, typeName);
            }
            else if (dataTableValidator.HasRole(obj, typeName))
            {
                dataTableValidator.Validate(obj, typeName);
            }
            else if (dictionaryValidator.HasRole(obj, typeName))
            {
                dictionaryValidator.Validate(obj, typeName);
            }
            else
            {
                pocoValidator.Validate(obj, typeName);
            }
        }

        public void Validate<TException>(Action action, string typeName) where TException : Exception
        {
            bool isNormalEnd = false;
            try
            {
                action();
                isNormalEnd = true;
            }
            catch (TException e)
            {
                Loggi.Debug(e);
                object obj = e;
                Delegate editor = GetExceptionEditor(e.GetType());
                if (editor != null)
                {
                    obj = editor.DynamicInvoke(e);
                }
                Validate(obj, typeName);
            }

            if (isNormalEnd)
            {
                Assertie.Fail("M_Fixture_Temp_ObjectValidator_Exception", typeof(TException).Name);
            }
        }

        private Delegate GetExceptionEditor(Type type)
        {
            Delegate editor = testCase.GetExceptionEditor(type);
            if (editor != null)
            {
                return editor;
            }
            if (typeof(Exception).IsAssignableFrom(type.BaseType))
            {
                return GetExceptionEditor(type.BaseType);
            }
            return null;
        }


        public Section.SectionType SectionType 
        {
            set
            {
                dictionaryValidator.SectionType = value;
                pocoValidator.SectionType = value;
                dataSetValidator.SectionType = value;
                dataTableValidator.SectionType = value;
            }
        }
    }
}
