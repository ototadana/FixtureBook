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
using System.Reflection;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal abstract class ObjectFactoryBase : ObjectOperatorBase
    {
        private static readonly Type[] EmptyTypes = new Type[] { };
        private static readonly object[] EmptyObjects = new object[] { };

        private TempObjectFactory parent;

        protected ObjectFactoryBase(TempObjectFactory parent) : base(Section.SectionType.ObjectForExec)
        {
            this.parent = parent;
        }

        public virtual T GetObject<T>(params string[] typeName)
        {
            string name = null;
            if (typeName != null && typeName.Length > 0)
            {
                name = typeName[0];
            }
            return GetObject<T>(name);
        }

        protected virtual T GetObject<T>(string typeName)
        {
            Table table = GetTable(Section, typeof(T), typeName);
            List<Row> rows = table.Rows;
            if (rows.Count == 0)
            {
                return default(T);
            }
            return CreateObject<T>(table, rows[0]);
        }

        public virtual List<T> GetList<T>(string typeName)
        {
            Table table = GetTable(Section, typeof(T), typeName);
            List<T> list = new List<T>(table.Rows.Count);
            AddTo<T>(list, typeName);
            return list;
        }

        public virtual void AddTo<T>(IList list, string typeName)
        {
            Table table = GetTable(Section, typeof(T), typeName);
            List<Row> rows = table.Rows;
            foreach (Row row in rows)
            {
                T obj = CreateObject<T>(table, row);
                list.Add(obj);
            }
        }

        protected abstract T CreateObject<T>(Table table, Row row);

        public virtual T[] GetArray<T>(string typeName)
        {
            List<T> list = GetList<T>(typeName);
            return list.ToArray();
        }

        protected virtual object ToObject(string columnName, Type columnType, Type columnComponentType, 
            string textValue, Table table, Row row)
        {
            try
            {
                return ToObject(columnType, columnComponentType, textValue);
            }
            catch (Exception e)
            {
                throw new ConfigException(e, "M_Fixture_Temp_ObjectFactory_ConvertError", columnName, textValue, columnType, table, row);
            }
        }

        private object ToObject(Type type, Type componentType, string textValue)
        {
            if (textValue == null || NULL.Equals(textValue))
            {
                return null;
            }

            if (EMPTY.Equals(textValue))
            {
                return "";
            }

            if (type.IsArray)
            {
                if (HasArraySeparator(textValue) || !Section.HasTable(textValue))
                {
                    return ToArray(componentType, textValue);
                }
                else
                {
                    return InvokeMethod(componentType, textValue, "GetArray");
                }
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                if (HasArraySeparator(textValue) || !Section.HasTable(textValue))
                {
                    return ToList(type, componentType, textValue);
                }
                else
                {
                    IList list = (IList)type.GetConstructor(EmptyTypes).Invoke(EmptyObjects);
                    MethodInfo method = parent.GetType().GetMethod("AddTo");
                    method.MakeGenericMethod(componentType).Invoke(parent, new object[] { list, textValue });
                    return list;
                }
            }
            else if (TypeConverter.IsConvertible(type))
            {
                return TypeConverter.ChangeType(textValue, type);
            }
            else if (typeof(object).Equals(type))
            {
                return textValue;
            }
            else
            {
                return InvokeMethod(type, new string[]{textValue}, "GetObject");
            }
        }

        private object InvokeMethod(Type componentType, object textValue, string methodName)
        {
            MethodInfo method = parent.GetType().GetMethod(methodName);
            return method.MakeGenericMethod(componentType).Invoke(parent, new object[] { textValue });
        }

        protected virtual object ToList(Type type, Type componentType, string textValue)
        {
            Array values = ToArray(componentType, textValue);
            int length = values.Length;
            IList list = CreateList(type, componentType, length);
            for (int i = 0; i < length; i++)
            {
                list.Add(values.GetValue(i));
            }
            return list;
        }

        private IList CreateList(Type type, Type componentType, int length)
        {
            if(type.IsInterface || type.IsAbstract) 
            {
                type = typeof(List<object>);
            }
            ConstructorInfo constructor = type.GetConstructor(EmptyTypes);
            return (IList)constructor.Invoke(EmptyObjects);
        }

        protected string GetValue(Row row, string name)
        {
            string value = null;
            row.Values.TryGetValue(name, out value);
            return value;
        }
    }
}
