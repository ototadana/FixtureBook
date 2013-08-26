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
using System.Data;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    /// <summary>
    /// データベース用の StorageValidator。
    /// </summary>
    internal class DatabaseValidator : DatabaseOperator, IStorageValidator
    {
        private TempObjectValidator tempObjectValidator;

        protected Section Section { get { return testCase.GetSection(Section.SectionType.DataAsExpected); } }

        public override void Initialize(Case testCase)
        {
            base.Initialize(testCase);
            tempObjectFactory.dataSetFactory.ForSearchKey = true;
            tempObjectValidator = new TempObjectValidator();
            tempObjectValidator.Initialize(testCase);
            tempObjectValidator.SectionType = Section.SectionType.DataAsExpected;
        }

        public bool HasRole()
        {
            return Section != null && Section.HasTable();
        }

        public void Validate()
        {
            InitializeSearchKey();
            using (Database database = new Database())
            {
                Section section = Section;
                DataSet dataSet = GetDataSet(Section.SectionType.DataAsExpected);
                foreach (DataTable expectedDataTable in dataSet.Tables)
                {
                    List<string> keyColumns = GetKeyColumns(section, expectedDataTable);
                    Table expectedTable = section.GetTable(expectedDataTable.TableName);
                    DataTable actualDataTable = database.Select(keyColumns, expectedDataTable, expectedTable);
                    Validate(actualDataTable);
                }
            }
        }

        private void InitializeSearchKey()
        {
            Section section = Section;
            if (section == null)
            {
                return;
            }
            foreach (string tableName in section.TableNames)
            {
                Table table = section.GetTable(tableName);
                InitializeSearchKey(table);
            }
        }

        private static void InitializeSearchKey(Table table)
        {
            if (HasSearchKeyColumn(table.Columns))
            {
                return;
            }
            foreach (Column column in table.Columns)
            {
                if (column != null)
                {
                    column.IsSearchKey = true;
                }
            }
        }

        private static bool HasSearchKeyColumn(List<Column> columns)
        {
            foreach (Column column in columns)
            {
                if (column != null && column.IsSearchKey)
                {
                    return true;
                }
            }
            return false;
        }

        private void Validate(DataTable actualDataTable)
        {
            tempObjectValidator.Validate(actualDataTable);
        }

        private static List<string> GetKeyColumns(Section section, DataTable table)
        {
            List<Column> columns = section.GetTable(table.TableName).Columns;
            return GetSearchKey(columns);
        }

        private static List<string> GetSearchKey(List<Column> columns)
        {
            List<string> keyColumns = new List<string>();
            foreach (Column column in columns)
            {
                if (column != null && column.IsSearchKey)
                {
                    keyColumns.Add(column.Name);
                }
            }
            return keyColumns;
        }
    }
}
