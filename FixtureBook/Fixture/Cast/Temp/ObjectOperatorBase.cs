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
using System.IO;
using System.Text;
using XPFriend.Fixture.Staff;
using XPFriend.Fixture.Toolkit;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal abstract class ObjectOperatorBase
    {
        protected const string NULL = "${NULL}";
        protected const string EMPTY = "${EMPTY}";

        protected Case testCase;
        private Section.SectionType sectiontype;
        private static readonly char[] FilePathMark = new char[] { '\\', '.' };

        protected ObjectOperatorBase(Section.SectionType sectiontype)
        {
            this.sectiontype = sectiontype;
        }

        protected Section Section { get { return testCase.GetSection(SectionType); } }

        internal Section.SectionType SectionType
        {
            get { return sectiontype; }
            set { sectiontype = value; }
        }

        public virtual void Initialize(Case testCase)
        {
            this.testCase = testCase;
        }

        protected virtual Table GetTable(Section section, Type cls, string typeName)
        {
            return GetTable(section, cls, typeName, null);
        }

        protected virtual Table GetTable(Section section, Type cls, string typeName, string defaultTypeName)
        {
            string tableName = GetTableName(section, cls, typeName, defaultTypeName);
            return section.GetTable(tableName);
        }

        private static string GetTableName(Section section, Type cls, string typeName, string defaultTypeName)
        {
            if (!Strings.IsEmpty(typeName))
            {
                return typeName;
            }

            List<string> tableNames = section.TableNames;
            if (tableNames.Count == 1)
            {
                return tableNames[0];
            }

            if (!Strings.IsEmpty(defaultTypeName))
            {
                return defaultTypeName;
            }

            if (cls != null)
            {
                return cls.Name;
            }

            throw new ConfigException("M_Fixture_Temp_ObjectOperator_GetTableName", section);
        }

        protected bool HasArraySeparator(string textValue)
        {
            return textValue != null && textValue.IndexOf('|') > -1;
        }

        protected virtual Array ToArray(Type componentType, string textValue)
        {
            if (componentType.Equals(typeof(byte)))
            {
                return ToByteArray(textValue);
            }

            return ToArrayInternal(componentType, textValue);
        }

        private static Array ToArrayInternal(Type componentType, string textValue)
        {
            string[] textValues = textValue.Split('|');
            if (componentType.Equals(typeof(string)))
            {
                return textValues;
            }

            Array values = Array.CreateInstance(componentType, textValues.Length);
            for (int i = 0; i < values.Length; i++)
            {
                values.SetValue(TypeConverter.ChangeType(textValues[i], componentType), i);
            }
            return values;
        }

        protected virtual Array ToByteArray(string textValue)
        {
            if(IsFilePath(textValue))
            {
                string filePath = PathUtil.GetFilePath(textValue);
                if (filePath == null)
                {
                    filePath = textValue;
                }
                return File.ReadAllBytes(filePath);
            }

            if (IsBarSeparatedArray(textValue))
            {
                return ToArrayInternal(typeof(byte), textValue);
            }

            return Convert.FromBase64String(textValue);
        }

        protected bool IsFilePath(string textValue)
        {
            return textValue.IndexOfAny(FilePathMark) > -1;
        }

        protected bool IsBarSeparatedArray(string textValue)
        {
            return textValue.Length == 1 || textValue.IndexOf('|') > -1;
        }
    }
}
