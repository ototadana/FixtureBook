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
using System.Collections.Generic;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;

namespace XPFriend.Fixture.Cast.Temp
{
    /// <summary>
    /// Dictionary および Dictionary のリスト用の <see cref="IObjectFactory"/>。
    /// </summary>
    internal class DictionaryFactory : ObjectFactoryBase, IObjectFactory
    {
        private TypeConverter typeConverter = new TypeConverter();

        public DictionaryFactory(TempObjectFactory parent) : base(parent) { }

        public bool HasRole<T>(params string[] typeName)
        {
            return Section != null && typeof(IDictionary).IsAssignableFrom(typeof(T));
        }

        protected override T CreateObject<T>(Table table, Row row)
        {
            DynaType dynaType = typeConverter.GetDynaType(table);
            DynaRow<T> bean = dynaType.NewInstance<T>();
            SetProperties(table, row, table.Columns, bean);
            return bean.Instance;
        }

        private void SetProperties<T>(Table table, Row row, List<Column> columns, DynaRow<T> bean)
        {
            foreach (Column column in columns)
            {
                if (column != null)
                {
                    SetProperty(table, row, column, bean);
                }
            }
        }

        private void SetProperty<T>(Table table, Row row, Column column, DynaRow<T> bean)
        {
            string name = column.Name;
            string textValue = GetValue(row, name);
            DynaColumn property = bean.DynaType[column.Name];
            object value = ToObject(name, property.Type, property.ComponentType, textValue, table, row);
            ((IDictionary)bean.Instance)[name] = value;
        }
    }
}
