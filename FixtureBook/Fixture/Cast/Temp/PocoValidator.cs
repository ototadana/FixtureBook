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
using System.Reflection;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    /// <summary>
    /// オブジェクト及びオブジェクトのリスト用の <see cref="IObjectValidator"/>。
    /// </summary>
    internal class PocoValidator : ObjectValidatorBase, IObjectValidator
    {
        private PocoUtil pocoUtil = new PocoUtil();

        public PocoValidator(IObjectValidator parent) : base(parent) { }

        public bool HasRole(object obj, params string[] typeName)
        {
            return Section != null && Section.HasTable();
        }

        protected override object GetPropertyValue(object obj, string name, Table table, Row row)
        {
            if (IsSimpleType(obj, row))
            {
                return obj;
            }

            Type type = obj.GetType();
            PropertyInfo property = pocoUtil.GetPropertyInfo(name, type, table, row);
            return property.GetValue(obj, null);
        }

        private bool IsSimpleType(object value, Row row)
        {
            Dictionary<string, string> values = row.Values;
            return values.Count == 1 && values.ContainsKey(OWN) &&
                (value == null || TypeConverter.IsConvertible(value.GetType()));
        }

        public override void Validate<TException>(Action action, string typeName)
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
                Validate(e, typeName);
            }

            if (isNormalEnd)
            {
                Assertie.Fail("M_Fixture_Temp_ObjectValidator_Exception", typeof(TException).Name);
            }
        }

    }
}
