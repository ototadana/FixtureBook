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
using System.Data;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    /// <summary>
    /// データベース用の <see cref="IStorageUpdater"/>。
    /// </summary>
    internal class DatabaseUpdater : DatabaseOperator, IStorageUpdater
    {
        protected Section CreateSection { get { return testCase.GetSection(Section.SectionType.DataToCreate); } }
        protected Section DeleteSection { get { return testCase.GetSection(Section.SectionType.DataToDelete); } }

        public void Initialize(Sheet sheet)
        {
            Initialize(sheet.GetCase(Case.Anonymous));
        }

        public bool HasRole()
        {
            return CreateSection != null || DeleteSection != null;
        }

        public void Setup()
        {
            if (DeleteSection == null && CreateSection == null)
            {
                return;
            }

            using (Database database = new Database())
            {
                if (DeleteSection != null && DeleteSection.HasTable())
                {
                    DataSet dataSet = GetDataSet(Section.SectionType.DataToDelete);
                    database.Delete(dataSet, DeleteSection);
                }

                if (CreateSection != null && CreateSection.HasTable())
                {
                    UpdateColumnTypes(database, Section.SectionType.DataToCreate);
                    DataSet dataSet = GetDataSet(Section.SectionType.DataToCreate);
                    database.Insert(dataSet, CreateSection);
                }
                database.Commit();
            }
        }

        private void UpdateColumnTypes(Database database, Section.SectionType sectionType)
        {
            Section section = testCase.GetSection(sectionType);
            foreach (string tableName in section.TableNames)
            {
                Table table = section.GetTable(tableName);
                UpdateColumnTypes(database, table);
            }
        }

        private static void UpdateColumnTypes(Database database, Table table)
        {
            DataTable metaData = database.GetMetaData(table);
            foreach (Column column in table.Columns)
            {
                if (column != null)
                {
                    DataColumn dataColumn = metaData.Columns[column.Name];
                    if (dataColumn == null)
                    {
                        throw new ConfigException("M_Fixture_Temp_DatabaseOperator_Column_NotFound",
                            metaData.TableName, column.Name, table);
                    }
                    if (column.Type == null && column.ComponentType == null)
                    {
                        column.SetType(dataColumn.DataType);
                    }
                }
            }
        }
    }
}
