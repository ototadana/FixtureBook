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
using System.Reflection;
using XPFriend.Fixture.Role;

namespace XPFriend.Fixture.Cast.Temp
{
    /// <summary>
    /// オブジェクト及びオブジェクトのリスト用の <see cref="IObjectValidator"/>。
    /// </summary>
    internal class PocoValidator : ObjectValidatorBase, IObjectValidator
    {
        private PocoUtil propertiesUtil = new PocoUtil();

        public PocoValidator(IObjectValidator parent) : base(parent) { }

        public bool HasRole(object obj, params string[] typeName)
        {
            return Section != null && Section.HasTable();
        }

        protected override object GetPropertyValue(object obj, string name)
        {
            Type type = obj.GetType();
            PropertyInfo property = propertiesUtil[type][name];
            return property.GetValue(obj, null);
        }

        public override void Validate<TException>(Action action, string typeName)
        {
            try
            {
                action();
                Assertie.Fail("M_Fixture_Temp_ObjectValidator_Exception", typeof(TException).Name);
            }
            catch (TException e)
            {
                Validate(e, typeName);
            }
        }

    }
}
