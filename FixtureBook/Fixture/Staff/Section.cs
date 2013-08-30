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
using System.Collections.Generic;
using System.Text;
using XPFriend.Junk;

namespace XPFriend.Fixture.Staff
{
    /// <summary>
    /// セクション。
    /// </summary>
    internal class Section
    {
        public enum SectionType
        {
            TestCaseDesctiption = 1,
            DataToCreate = 2,
            DataToDelete =3,
            DataAsExpected = 4,
            ObjectForExec = 5,
            ExpectedResult = 6,
            FileAsExpected = 7
        }

        private static readonly SectionType[] Sections;
        static Section()
        {
            Sections = new SectionType[128];
            Sections['1'] = SectionType.TestCaseDesctiption;
            Sections['2'] = SectionType.DataToCreate;
            Sections['3'] = SectionType.DataToDelete;
            Sections['4'] = SectionType.DataAsExpected;
            Sections['5'] = SectionType.ObjectForExec;
            Sections['6'] = SectionType.ExpectedResult;
            Sections['7'] = SectionType.FileAsExpected;

            Sections['A'] = SectionType.TestCaseDesctiption;
            Sections['a'] = SectionType.TestCaseDesctiption;
            Sections['B'] = SectionType.DataToDelete;
            Sections['b'] = SectionType.DataToDelete;
            Sections['C'] = SectionType.DataToCreate;
            Sections['c'] = SectionType.DataToCreate;
            Sections['D'] = SectionType.ObjectForExec;
            Sections['d'] = SectionType.ObjectForExec;
            Sections['E'] = SectionType.ExpectedResult;
            Sections['e'] = SectionType.ExpectedResult;
            Sections['F'] = SectionType.DataAsExpected;
            Sections['f'] = SectionType.DataAsExpected;
            Sections['G'] = SectionType.FileAsExpected;
            Sections['g'] = SectionType.FileAsExpected;
        }

        private Case testCase;
        private string sectionName;
        private int sectionNumber;
        private Dictionary<string, Table> tables = new Dictionary<string, Table>();
        private Dictionary<string, Table> tablesWithAliases = new Dictionary<string, Table>();
        private List<string> tableNames = new List<string>();

        internal Section(Case testCase, string sectionName)
        {
            this.testCase = testCase;
            this.sectionName = sectionName;
            this.sectionNumber = ToSectionNumber(sectionName);
        }

        /// <summary>
        /// 最大のセクション番号。
        /// </summary>
        public static int MaxNumber { get { return 7; } }

        /// <summary>
        /// このセクションの名前。
        /// </summary>
        public string Name { get { return sectionName; } }

        /// <summary>
        /// このセクションのセクション番号。
        /// </summary>
        public int Number { get { return sectionNumber; } }

        /// <summary>
        /// このセクションが属すテストケース定義。
        /// </summary>
        public Case Case { get { return testCase; } }

        /// <summary>
        /// このセクション内に定義されているテーブルの名前。
        /// </summary>
        public List<string> TableNames
        {
            get
            {
                return tableNames;
            }
        }

        private int ToSectionNumber(string sectionName)
        {
            try
            {
                return (int)Sections[sectionName[0]];
            }
            catch
            {
                return 0;
            }
        }

        internal Table CreateTable(string tableName) 
        {
            Table table = null;
            tables.TryGetValue(tableName, out table);
            if (table == null)
            {
                table = new Table(this, tableName);
                tables[tableName] = table;
                tablesWithAliases[tableName] = table;
                int atIndex = tableName.IndexOf('@');
                if (atIndex > -1)
                {
                    tablesWithAliases[tableName.Substring(0, atIndex)] = table;
                }
                tableNames.Add(tableName);
            }
            return table;
        }

        /// <summary>
        /// 指定された名前のテーブルを取得する。
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <returns>テーブル</returns>
        public Table GetTable(string tableName)
        {
            Table table = null;
            tables.TryGetValue(tableName, out table);
            if (table != null)
            {
                return table;
            }

            tablesWithAliases.TryGetValue(tableName, out table);
            if (table != null)
            {
                return table;
            }
            throw new ConfigException("M_Fixture_Section_GetTable", tableName, this);
        }

        /// <summary>
        /// 指定された名前のテーブルがあるかどうかを調べる。
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <returns>指定された名前のテーブルがある場合はtrue</returns>
        public bool HasTable(string tableName)
        {
            return tablesWithAliases.ContainsKey(tableName);
        }

        /// <summary>
        /// このセクションにテーブルが定義されているかどうかを調べる。
        /// </summary>
        /// <returns>テーブルが定義されていればtrue</returns>
        public bool HasTable()
        {
            return tables.Count > 0;
        }

        public override string ToString()
        {
            return testCase.ToString() + ", " + Name;
        }

        internal bool HasTable(int index)
        {
            return 0 <= index && index < tableNames.Count;
        }

        internal Table GetTable(int index)
        {
            return GetTable(tableNames[index]);
        }
    }
}
