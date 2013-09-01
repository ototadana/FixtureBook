using System;
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
using System.Text;
using XPFriend.Junk;

namespace XPFriend.Fixture.Staff
{
    /// <summary>
    /// 列データ。
    /// </summary>
    internal class Column
    {
        private string name;
        private string type;
        private string componentType;

        /// <summary>
        /// 列データを作成する。
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="type">列タイプ</param>
        /// <param name="componentType">列要素タイプ</param>
        public Column(string name, string type, string componentType)
        {
            if (name != null && name.StartsWith("*"))
            {
                this.name = name.Substring(1);
                this.IsSearchKey = true;
            }
            else
            {
                this.name = name;
                this.IsSearchKey = false;
            }
            this.type = type;
            this.componentType = componentType;
        }

        /// <summary>
        /// 列名。
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// 検索キーかどうか。
        /// </summary>
        public bool IsSearchKey { get; set; }

        /// <summary>
        /// 列タイプ。
        /// </summary>
        public string Type { get { return type; } }

        /// <summary>
        /// 列要素タイプ。
        /// </summary>
        public string ComponentType { get { return componentType; } }

        /// <summary>
        /// この列が配列かどうかを調べる。
        /// </summary>
        /// <returns>配列の場合は true</returns>
        public bool IsArray()
        {
            return type == null && componentType != null;
        }

        public void SetType(Type type)
        {
            if (type.IsArray)
            {
                this.componentType = type.GetElementType().ToString();
            }
            else
            {
                if (type.IsGenericType)
                {
                    this.componentType = Join(type.GetGenericArguments());
                }
                this.type = type.ToString();
            }
        }

        private string Join(Type[] types)
        {
            StringBuilder value = new StringBuilder();
            foreach (Type type in types)
            {
                if (value.Length > 0)
                {
                    value.Append(",");
                }
                value.Append(type);
            }
            return value.ToString();
        }

        public override string ToString()
        {
            if (Strings.IsEmpty(type) && Strings.IsEmpty(componentType))
            {
                return name;
            }
            
            StringBuilder sb = new StringBuilder();
            sb.Append(name).Append(":");
            if (Strings.IsEmpty(type))
            {
                sb.Append(componentType).Append("[]");
                return sb.ToString();
            }

            sb.Append(type);
            if (Strings.IsEmpty(componentType))
            {
                return sb.ToString();
            }

            sb.Append("<").Append(componentType).Append(">");
            return sb.ToString();
        }
    }
}
