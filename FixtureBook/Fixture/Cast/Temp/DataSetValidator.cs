﻿/*
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
using System.Text;
using XPFriend.Fixture.Role;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class DataSetValidator : DataTableValidator, IObjectValidator
    {
        public DataSetValidator(IObjectValidator parent) : base(parent) { }

        public override bool HasRole(object obj, params string[] typeName)
        {
            return Section != null && obj is DataSet;
        }

        public override void Validate(object obj, params string[] typeName)
        {
            DataSet dataSet = (DataSet)obj;

            if (typeName == null || typeName.Length == 0) 
            {
                typeName = GetTableNamesFrom(dataSet);
                if (dataSet.Tables.Count != Section.TableNames.Count)
                {
                    Assertie.Fail(
                        "M_Fixture_Temp_ObjectValidator_AssertTableNumberEquals",
                        Section.TableNames.Count, dataSet.Tables.Count, Section);
                }

            }

            if (dataSet.Tables.Count < typeName.Length)
            {
                Assertie.Fail("M_Fixture_Temp_ObjectValidator_AssertTableNumberExp", 
                    ToStringInternal(typeName), ToStringInternal(dataSet.Tables), Section);
            }

            List<DataTable> dataTables = CreateDataTableList(dataSet.Tables);
            for (int i = 0; i < typeName.Length; i++)
            {
                DataTable dataTable = GetDataTable(typeName[i], dataTables);
                Validate(dataTable, typeName[i]);
            }
        }

        private string[] GetTableNamesFrom(DataSet dataSet)
        {
            List<string> tableNames = new List<string>();
            if (ValidateTableNames(dataSet))
            {
                foreach (DataTable table in dataSet.Tables)
                {
                    tableNames.Add(table.TableName);
                }
            }
            else if (dataSet.Tables.Count > Section.TableNames.Count)
            {
                Assertie.Fail(
                    "M_Fixture_Temp_ObjectValidator_AssertTableNumberImp",
                    Section.TableNames.Count, dataSet.Tables.Count, Section);
            }
            else
            {
                string messageFormat = Resi.Get("M_Fixture_Temp_ObjectValidator_UseTableOrder");
                Loggi.Warn(String.Format(messageFormat, Section));
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    Loggi.Warn("DataSet.Table[" + i + "] = " + Section.TableNames[i]);
                    tableNames.Add(Section.TableNames[i]);
                }
            }
            return tableNames.ToArray();
        }

        private bool ValidateTableNames(DataSet dataSet)
        {
            foreach (DataTable table in dataSet.Tables)
            {
                if (!Section.HasTable(table.TableName))
                {
                    return false;
                }
            }
            return true;
        }

        private DataTable GetDataTable(string name, List<DataTable> dataTables)
        {
            foreach (DataTable dataTable in dataTables)
            {
                if (Strings.Equals(name, dataTable.TableName))
                {
                    dataTables.Remove(dataTable);
                    return dataTable;
                }
            }
            {
                DataTable dataTable = dataTables[0];
                dataTables.RemoveAt(0);
                return dataTable;
            }
        }

        private List<DataTable> CreateDataTableList(DataTableCollection dataTableCollection)
        {
            List<DataTable> list = new List<DataTable>();
            foreach (DataTable dataTable in dataTableCollection)
            {
                list.Add(dataTable);
            }
            return list;
        }

        private string ToStringInternal(DataTableCollection tables)
        {
            StringBuilder text = new StringBuilder();
            foreach (DataTable table in tables)
            {
                if (text.Length > 0)
                {
                    text.Append(", ");
                }
                text.Append(table.TableName);
            }
            return text.ToString();
        }

        private string ToStringInternal(string[] typeName)
        {
            return string.Join(", ", typeName);
        }
    }
}
