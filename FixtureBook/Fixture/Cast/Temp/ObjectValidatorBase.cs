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
using System.Text.RegularExpressions;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;
using XPFriend.Fixture.Toolkit;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal abstract class ObjectValidatorBase : ObjectOperatorBase
    {
        private const string TODAY = "${TODAY}";

        private IObjectValidator parent;

        protected ObjectValidatorBase(IObjectValidator parent)
            : base(Section.SectionType.ExpectedResult)
        {
            this.parent = parent;
        }

        public virtual void Validate(object obj, params string[] typeName)
        {
            string name = null;
            if (typeName != null && typeName.Length > 0)
            {
                name = typeName[0];
            }
            Validate(obj, name);
        }

        protected virtual void Validate(object obj, string typeName)
        {
            ValidateInternal(obj, typeName, null);
        }

        protected virtual void ValidateInternal(object obj, string typeName, string defaultTypeName)
        {
            if (obj == null)
            {
                AssertNull(typeName);
                return;
            }
            Table expected;
            ICollection actual;
            Type type;
            if (obj is Array)
            {
                actual = (Array)obj;
                type = obj.GetType().GetElementType();
            }
            else if (obj is IEnumerable && !(obj is IDictionary))
            {
                type = null;
                foreach (object o in (IEnumerable)obj)
                {
                    if (o != null)
                    {
                        type = o.GetType();
                        break;
                    }
                }
                actual = ToICollection((IEnumerable)obj);
            }
            else
            {
                ArrayList list = new ArrayList();
                list.Add(obj);
                actual = list;
                type = obj.GetType();
            }
            expected = GetTable(Section, type, typeName, defaultTypeName);
            AssertEquals(expected, actual);
        }

        private ICollection ToICollection(IEnumerable obj)
        {
            if (obj is ICollection)
            {
                return (ICollection)obj;
            }
            else
            {
                ArrayList list = new ArrayList();
                foreach (object o in obj)
                {
                    list.Add(o);
                }
                return list;
            }
        }

        protected virtual void AssertNull(string typeName)
        {
            if (Section.HasTable(typeName))
            {
                Assertie.Fail("M_Fixture_Temp_ObjectValidator_AssertNull", typeName, Section.GetTable(typeName));
            }
        }

        protected virtual void AssertEquals(Table table, ICollection actualList)
        {
            List<Row> expectedList = table.Rows;
            AssertLineNumber(table, expectedList.Count, actualList.Count);

            int i = 0;
            foreach (object actual in actualList)
            {
                AssertEquals(table, expectedList[i++], actual);
            }
        }

        protected virtual void AssertLineNumber(Table table, int expected, int actual)
        {
            if (expected != actual)
            {
                Assertie.AreEqual(expected, actual, "M_Fixture_Temp_ObjectValidator_AssertLineNumber", table, table.Name, expected, actual);
            }
        }

        protected virtual void AssertEquals(Table table, Row row, object actualObject)
        {
            if (row.Deleted)
            {
                return;
            }
            List<Column> columns = table.Columns;
            Dictionary<string, string> expectedObject = row.Values;
            foreach (Column column in columns)
            {
                if (column != null)
                {
                    string name = column.Name;
                    string expectedPropertyValue = null;
                    expectedObject.TryGetValue(name, out expectedPropertyValue);
                    object actualPropertyValue = GetPropertyValue(actualObject, name, table, row);
                    AssertEquals(table, row, name, expectedPropertyValue, actualPropertyValue);
                }
            }
        }

        protected virtual void AssertEquals(Table table, Row row, string columnName,
            string expected, object actual)
        {
            if (AssertEmpty(table, row, columnName, expected, actual))
            {
                return;
            }
            if (AssertNotEmpty(table, row, columnName, expected, actual))
            {
                return;
            }
            String actualAsText = ToString(expected, actual);
            if (AssertEqualsStrictly(table, row, columnName, expected, actualAsText))
            {
                return;
            }
            if (AssertNestedObject(table, row, columnName, expected, actual))
            {
                return;
            }
            if (AssertPartialEquality(table, row, columnName, expected, actualAsText))
            {
                return;
            }
            if (AssertEqualsAsDate(table, row, columnName, expected, actual))
            {
                return;
            }
            if (AssertEqualsAsBool(table, row, columnName, expected, actual))
            {
                return;
            }
            if (AssertEqualsAsByteArray(table, row, columnName, expected, actual))
            {
                return;
            }
            if (AssertEqualsByRegex(table, row, columnName, expected, actualAsText))
            {
                return;
            }
            Assertie.AreEqual(expected, actualAsText,
                    "M_Fixture_Temp_ObjectValidator_AssertEquals", table, row, columnName, expected, actualAsText, GetType(actual));
        }

        private static object GetType(object actual)
        {
            if (actual == null || actual is string)
            {
                return "";
            }
            return "(" + actual.GetType() + ")";
        }

        protected virtual bool AssertEqualsAsBool(Table table, Row row, string columnName, string expected, object actual)
        {
            if (actual is bool)
            {
                return actual.ToString().ToLower().Equals(expected.ToLower());
            }
            return false;
        }

        protected virtual bool AssertEqualsAsByteArray(Table table, Row row, string columnName, string expected, object actual)
        {
            if (!(actual is byte[]))
            {
                return false;
            }

            byte[] expectedBytes = (byte[])ToArray(typeof(byte), expected);
            byte[] actualBytes = (byte[])actual;
            if (expectedBytes.Length != actualBytes.Length)
            {
                AreEqual(table, row, columnName, expected, actualBytes);
                return false;
            }

            for (int i = 0; i < expectedBytes.Length; i++)
            {
                if (expectedBytes[i] != actualBytes[i])
                {
                    AreEqual(table, row, columnName, expected, actualBytes);
                    return false;
                }
            }

            return true;
        }

        private void AreEqual(Table table, Row row, string columnName, string expected, byte[] actualBytes)
        {
            string actualAsText = ToStringFromByteArray(actualBytes, expected);
            Assertie.AreEqual(expected, actualAsText,
                    "M_Fixture_Temp_ObjectValidator_AssertEquals", table, row, columnName, expected, actualAsText, GetType(actualBytes));
        }

        protected virtual bool AssertEqualsByRegex(Table table, Row row, string columnName,
            string expected, string actual)
        {
            if (expected.Length > 2 && expected.StartsWith("`") && expected.EndsWith("`"))
            {
                String regex = expected.Substring(1, expected.Length - 2);
                return Regex.IsMatch(actual, regex);
            }
            return false;
        }

        protected virtual bool AssertEqualsAsDate(Table table, Row row, string columnName,
            string expected, object actual)
        {
            if (expected.IndexOf(TODAY) > -1 && (actual is DateTime || actual is DateTimeOffset))
            {
                String today = Formi.Format(DateTime.Today, "yyyy-MM-dd");
                expected = expected.Replace(TODAY, today);
                if (AssertEqualsStrictly(table, row, columnName, expected, DateTimeToString((IFormattable)actual, today)))
                {
                    return true;
                }
                Assertie.AreEqual(expected, actual,
                        "M_Fixture_Temp_ObjectValidator_AssertEquals", table, row, columnName, expected, actual, GetType(actual));
            }
            if (actual is DateTime || actual is DateTimeOffset)
            {
                string actualAsText = DateTimeToString((IFormattable)actual, expected);
                if (AssertEqualsStrictly(table, row, columnName, expected, actualAsText))
                {
                    return true;
                }
                Assertie.AreEqual(expected, actualAsText,
                        "M_Fixture_Temp_ObjectValidator_AssertEquals", table, row, columnName, expected, actualAsText, GetType(actual));
            }
            return false;
        }

        protected virtual string DateTimeToString(IFormattable actual, string expected)
        {
            return actual.ToString(TypeConverter.GetDateTimeFormat(expected), null);
        }

        protected virtual bool AssertPartialEquality(Table table, Row row, string columnName,
            string expected, string actual)
        {
            if ("%".Equals(expected))
            {
                expected = "%%";
            }

            if (expected.StartsWith("%"))
            {
                if (expected.EndsWith("%"))
                {
                    return actual.IndexOf(expected.Substring(1, expected.Length - 2)) > -1;
                }
                return actual.EndsWith(expected.Substring(1));
            }
            else if (expected.EndsWith("%"))
            {
                return actual.StartsWith(expected.Substring(0, expected.Length - 1));
            }
            return false;
        }

        protected virtual bool AssertEqualsStrictly(Table table, Row row, string columnName, string expected, string actual)
        {
            return expected.Equals(actual);
        }

        protected virtual bool AssertNestedObject(Table table, Row row, string columnName,
            string expected, object actual)
        {
            if (!Section.HasTable(expected))
            {
                return false;
            }
            parent.Validate(actual, expected);
            return true;
        }

        protected virtual bool AssertNotEmpty(Table table, Row row, string columnName,
            string expected, object actual)
        {
            if ("*".Equals(expected))
            {
                if (actual != null)
                {
                    if (!(actual is string))
                    {
                        return true;
                    }
                    if (!Strings.IsEmpty((string)actual))
                    {
                        return true;
                    }
                }
                Assertie.Fail("M_Fixture_Temp_ObjectValidator_AssertEquals", table, row, columnName, "*", ToStringInternal(actual), GetType(actual));
            }
            return false;
        }

        protected virtual bool AssertEmpty(Table table, Row row, string columnName,
            string expected, object actual)
        {
            if (Strings.IsEmpty(expected))
            {
                if (actual == null)
                {
                    return true;
                }
                if (actual is string && Strings.IsEmpty((string)actual))
                {
                    return true;
                }
                Assertie.Fail("M_Fixture_Temp_ObjectValidator_AssertEquals", table, row, columnName, "", actual, GetType(actual));
            }

            if (NULL.Equals(expected))
            {
                if (actual != null)
                {
                    Assertie.Fail("M_Fixture_Temp_ObjectValidator_AssertEquals", table, row, columnName, NULL, actual, GetType(actual));
                }
                return true;
            }

            if (EMPTY.Equals(expected))
            {
                if (actual == null || !(actual is string) || ((string)actual).Length > 0)
                {
                    Assertie.Fail("M_Fixture_Temp_ObjectValidator_AssertEquals", table, row, columnName, EMPTY, ToStringInternal(actual), GetType(actual));
                }
                return true;
            }
            return false;
        }

        private string ToStringFromByteArray(byte[] actualBytes, string expected)
        {
            if (IsFilePath(expected))
            {
                string filePath = PathUtil.GetFilePath(expected);
                if (filePath != null)
                {
                    string actualFilePath = Path.GetFullPath(filePath + ".tmp");
                    File.WriteAllBytes(actualFilePath, actualBytes);
                    return actualFilePath;
                }
            }

            if (IsBarSeparatedArray(expected))
            {
                return FromListToString(actualBytes);
            }

            return Convert.ToBase64String(actualBytes);
        }

        private string ToString(string expected, object actual)
        {
            if (actual is IList && !Section.HasTable(expected))
            {
                return FromListToString((IList)actual);
            }
            return ToString(actual);
        }

        private string FromListToString(IList list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object o in list)
            {
                if (sb.Length > 0)
                {
                    sb.Append('|');
                }
                sb.Append(ToString(o));
            }
            return sb.ToString();
        }

        private string ToString(object o)
        {
            if (o == null)
            {
                return null;
            }
            return o.ToString();
        }

        private object ToStringInternal(object actual)
        {
            if (actual == null)
            {
                return "null";
            }
            return actual.ToString();
        }

        protected abstract object GetPropertyValue(object obj, string name, Table table, Row row);

        public virtual void Validate<TException>(Action action, string typeName) where TException : Exception
        {
            throw new NotImplementedException();
        }
    }
}
