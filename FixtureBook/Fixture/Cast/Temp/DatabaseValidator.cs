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
using System.Data;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;

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
            using (Database database = new Database())
            {
                Section section = Section;
                DataSet dataSet = GetDataSet(Section.SectionType.DataAsExpected);
                foreach (DataTable keyTable in dataSet.Tables)
                {
                    List<string> keyColumns = GetKeyColumns(section, keyTable);
                    Table expectedTable = section.GetTable(keyTable.TableName);
                    DataTable actualDataTable = database.Select(keyColumns, keyTable, expectedTable);
                    Validate(actualDataTable);
                }
            }
        }

        private void Validate(DataTable actualDataTable)
        {
            tempObjectValidator.Validate(actualDataTable);
        }

        private static List<string> GetKeyColumns(Section section, DataTable table)
        {
            return section.GetTable(table.TableName).GetKeyColumnNames();
        }
    }
}
