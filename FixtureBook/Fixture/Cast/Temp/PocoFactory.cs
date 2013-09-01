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
using System.Reflection;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    /// <summary>
    /// オブジェクト及びオブジェクトのリスト用の <see cref="IObjectFactory"/>。
    /// </summary>
    internal class PocoFactory : ObjectFactoryBase, IObjectFactory
    {
        private PocoUtil pocoUtil = new PocoUtil();

        public PocoFactory(TempObjectFactory parent) : base(parent) { }

        public bool HasRole<T>(params string[] typeName)
        {
            return Section != null;
        }

        protected override T CreateObject<T>(Table table, Row row)
        {
            T obj = Activator.CreateInstance<T>();
            foreach (Column column in table.Columns)
            {
                if (column != null)
                {
                    SetProperty(table, row, column, obj);
                }
            }
            return obj;
        }

        protected void SetProperty<T>(Table table, Row row, Column column, T obj)
        {
            string name = column.Name;
            string textValue = GetValue(row, name);
            Type type = obj.GetType();
            PropertyInfo property = pocoUtil.GetPropertyInfo(name, type, table, row);
            object value = ToObject(name, property.PropertyType, GetComponentType(property), textValue, table, row);
            SetProperty<T>(obj, property, value, name, type, table, row);
        }

        private static void SetProperty<T>(T obj, PropertyInfo property, object value, string columnName, Type type, Table table, Row row)
        {
            try
            {
                property.SetValue(obj, value, null);
            }
            catch (Exception e)
            {
                throw new ConfigException(e, "M_Fixture_Temp_ObjectFactory_SetProperty", columnName, type, table, row, value);
            }
        }

        private Type GetComponentType(PropertyInfo property)
        {
            Type type = property.PropertyType;
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                Type[] componentTypes = type.GetGenericArguments();
                if (componentTypes.Length > 0)
                {
                    return componentTypes[0];
                }
            }

            return typeof(string);
        }
    }
}
