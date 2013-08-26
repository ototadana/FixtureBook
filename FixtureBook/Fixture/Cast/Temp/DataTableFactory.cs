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
using System.Data;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class DataTableFactory : ObjectFactoryBase, IObjectFactory
    {
        private TypeConverter typeConverter = new TypeConverter();
        private Dictionary<string, bool> searchKeyMap = new Dictionary<string, bool>();

        public DataTableFactory(TempObjectFactory parent) : base(parent) { }

        public bool ForSearchKey { get; set; }

        public virtual bool HasRole<T>(params string[] typeName)
        {
            return Section != null && typeof(DataTable).IsAssignableFrom(typeof(T));
        }

        protected override T CreateObject<T>(Table table, Row row)
        {
            throw new InvalidOperationException(); // ここにはこない。
        }

        public override List<T> GetList<T>(string typeName)
        {
            List<T> list = new List<T>(1);
            list.Add(GetObject<T>(typeName));
            return list;
        }

        protected override T GetObject<T>(string typeName)
        {
            return (T)(object)GetDataTable(typeName);
        }

        protected virtual DataTable GetDataTable(string typeName)
        {
            Table table = GetTable(Section, typeof(DataTable), typeName);
            DataTable dataTable = CreateDataTable(table);
            DynaType dynaType = typeConverter.GetDynaType(table);

            List<Row> rows = table.Rows;
            foreach (Row row in rows)
            {
                DataRow dataRow = CreateDataRow(dataTable, table, row, dynaType);
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        private DataRow CreateDataRow(DataTable dataTable, Table table, Row row, DynaType dynaType)
        {
            DataRow dataRow = dataTable.NewRow();
            foreach (DataColumn column in dataTable.Columns)
            {
                string columnName = column.ColumnName;
                if (!ForSearchKey || searchKeyMap[columnName])
                {
                    string textValue = GetValue(row, columnName);
                    dataRow[column] = ToObject(dynaType[columnName], textValue, table, row);
                }
            }
            return dataRow;
        }

        private object ToObject(DynaColumn column, string textValue, Table table, Row row)
        {
            object value = ToObject(column.Name, column.Type, column.ComponentType, textValue, table, row);
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }

        private DataTable CreateDataTable(Table table)
        {
            DataTable dataTable = new DataTable(table.Name);
            foreach (Column column in table.Columns)
            {
                if (column != null)
                {
                    searchKeyMap[column.Name] = column.IsSearchKey;
                    Type type = new DynaColumn(column).Type;
                    DataColumn dataColumn = new DataColumn(column.Name, type);
                    dataTable.Columns.Add(dataColumn);
                }
            }
            return dataTable;
        }
    }
}
