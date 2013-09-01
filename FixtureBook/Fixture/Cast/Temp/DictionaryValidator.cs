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
using System.Collections;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;

namespace XPFriend.Fixture.Cast.Temp
{
    /// <summary>
    /// Dictionary および Dictionary のリスト用の <see cref="IObjectValidator"/>。
    /// </summary>
    internal class DictionaryValidator : ObjectValidatorBase, IObjectValidator
    {
        public DictionaryValidator(IObjectValidator parent) : base(parent) { }

        public bool HasRole(object obj, params string[] typeName)
        {
            return Section != null && Section.HasTable() && IsDictionary(obj);
        }

        private bool IsDictionary(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is IList)
            {
                IList list = (IList)obj;
                if (list.Count == 0)
                {
                    return false;
                }
                return IsDictionaryInternal(list[0]);
            }

            return IsDictionaryInternal(obj);
        }

        private bool IsDictionaryInternal(object obj)
        {
            return obj is IDictionary;
        }

        protected override object GetPropertyValue(object obj, string name, Table table, Row row)
        {
            IDictionary dictionary = (IDictionary)obj;
            if (!dictionary.Contains(name))
            {
                return null;
            }
            return dictionary[name];
        }
    }
}
