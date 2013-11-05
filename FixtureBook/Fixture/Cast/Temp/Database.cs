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
using System.Data.Common;
using System.Text;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class Database : IDisposable
    {
        DatabaseConnection connection = new DatabaseConnection();
        private Dictionary<string, MetaData> metaDatas = new Dictionary<string, MetaData>();

        internal class MetaData
        {
            public DataTable MetaDataTable { get; set; }
            public bool? HasIdentityColumn { get; set; }
        }

        public void Use(string databaseName)
        {
            connection.Use(databaseName);
        }
        
        public void Commit() 
        {
            connection.Commit();
            Dispose();
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        public void Insert(DataSet dataSet, Section tableInfos)
        {
            foreach (DataTable table in dataSet.Tables)
            {
                Table tableInfo = tableInfos.GetTable(table.TableName);
                Use(table);
                Insert(table, tableInfo);
            }
        }

        private void Use(DataTable table)
        {
            string[] tableNameAndDatabaseName = SplitTableNameAndDatabaseName(table.TableName);
            table.TableName = tableNameAndDatabaseName[0];
            Use(tableNameAndDatabaseName[1]);
        }

        private string[] SplitTableNameAndDatabaseName(string tableName)
        {
            if (tableName.IndexOf('@') > -1)
            {
                return tableName.Split('@');
            }
            return new string[] { tableName, null };
        }

        internal void Insert(DataTable table, Table tableInfo)
        {
            try
            {
                SetEnabledIdentityInsert(true, table.TableName);
                DbCommand command = CreateInsertCommand(table, tableInfo);
                List<Row> rowInfo = tableInfo.Rows;
                int rowIndex = 0;
                foreach (DataRow row in table.Rows)
                {
                    InsertRow(table, command, row, tableInfo, rowInfo[rowIndex++]);
                }
            }
            finally
            {
                SetEnabledIdentityInsert(false, table.TableName);
            }
        }

        private void SetEnabledIdentityInsert(bool enabled, string tableName)
        {
            if (connection.IsSQLServer && HasIdentityColumn(tableName))
            {
                try
                {
                    string onOff = enabled ? " ON" : " OFF";
                    ExecuteNonQuery(CreateCommand("SET IDENTITY_INSERT " + tableName + onOff));
                }
                catch (Exception e)
                {
                    Loggi.Warn(e);
                }
            }
        }

        private bool HasIdentityColumn(string tableName)
        {
            MetaData metaData = GetMetaData(tableName);

            if (metaData.HasIdentityColumn == null)
            {
                metaData.HasIdentityColumn = HasIdentityColumnInternal(tableName);
            }

            return (bool)metaData.HasIdentityColumn;
        }

        private bool HasIdentityColumnInternal(string tableName)
        {
            DbDataAdapter dataAdapter = connection.ProviderFactory.CreateDataAdapter();
            dataAdapter.SelectCommand = CreateCommand("select * from " + tableName);
            DataTable table = new DataTable(tableName);
            table = dataAdapter.FillSchema(table, SchemaType.Source);
            foreach (DataColumn column in table.Columns)
            {
                if (column.AutoIncrement)
                {
                    return true;
                }
            }
            return false;
        }

        private void InsertRow(DataTable table, DbCommand command, DataRow row, Table tableInfo, Row rowInfo)
        {
            try
            {
                command.Parameters.Clear();
                foreach (DataColumn column in table.Columns)
                {
                    AddParameter(command, column.ColumnName, column.DataType, row[column]);
                }
                ExecuteNonQuery(command);
            }
            catch (Exception e)
            {
                throw new ConfigException(e, "M_Fixture_Temp_Database_InsertRow", table.TableName, tableInfo, rowInfo);
            }
        }

        private DbCommand CreateInsertCommand(DataTable table, Table tableInfo)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into ").Append(table.TableName);
                sql.Append(GetColumnList(table.Columns));
                sql.Append(GetValueList(table.Columns));
                return CreateCommand(sql.ToString());
            }
            catch (Exception e)
            {
                throw new ConfigException(e, "M_Fixture_Temp_Database_CreateInsertCommand", table.TableName, tableInfo);
            }
        }

        private StringBuilder GetValueList(DataColumnCollection columns)
        {
            StringBuilder sql = new StringBuilder();
            foreach (DataColumn column in columns)
            {
                if (sql.Length == 0)
                {
                    sql.Append("values(");
                }
                else
                {
                    sql.Append(",");
                }
                sql.Append(connection.ParameterPrefix).Append(column.ColumnName);
            }
            sql.Append(")");
            return sql;
        }

        private StringBuilder GetColumnList(DataColumnCollection columns)
        {
            StringBuilder sql = new StringBuilder();
            foreach (DataColumn column in columns)
            {
                if (sql.Length == 0)
                {
                    sql.Append("(");
                }
                else
                {
                    sql.Append(",");
                }
                sql.Append(column.ColumnName);
            }
            sql.Append(")");
            return sql;
        }

        public void Delete(DataSet dataSet, Section tableInfos)
        {
            foreach (DataTable table in dataSet.Tables)
            {
                Table tableInfo = tableInfos.GetTable(table.TableName);
                Use(table);
                Delete(table, tableInfo);
            }
        }

        internal void Delete(DataTable table, Table tableInfo)
        {
            String queryPrefix = "delete from " + table.TableName + " where ";
            List<string> columnNames = new List<string>();
            foreach (DataColumn column in table.Columns)
            {
                columnNames.Add(column.ColumnName);
            }

            List<Row> rowInfo = tableInfo.Rows;
            int rowIndex = 0;
            foreach (DataRow row in table.Rows)
            {
                Delete(table, row, queryPrefix, columnNames, tableInfo, rowInfo[rowIndex++]);
            }
        }

        private void Delete(DataTable table, DataRow row, String queryPrefix, List<string> columnNames, Table tableInfo, Row rowInfo)
        {
            try
            {
                DbCommand command = connection.CreateCommand();
                SetQueryString(table, row, command, queryPrefix, columnNames);
                ExecuteNonQuery(command);
            }
            catch (Exception e)
            {
                throw new ConfigException(e, "M_Fixture_Temp_Database_DeleteRow", table.TableName, tableInfo, rowInfo);
            }
        }

        internal void ExecuteNonQuery(DbCommand command)
        {
            PrintSQL(command);
            command.ExecuteNonQuery();
        }

        private static void PrintSQL(DbCommand command)
        {
            if (!Loggi.DebugEnabled)
            {
                return;
            }

            StringBuilder sqlText = new StringBuilder();
            foreach (DbParameter parameter in command.Parameters)
            {
                if (sqlText.Length == 0)
                {
                    sqlText.Append(command.CommandText).Append(" - ");
                }
                else
                {
                    sqlText.Append(", ");
                }
                sqlText.Append(parameter.Value);
            }

            if (sqlText.Length == 0)
            {
                sqlText.Append(command.CommandText);
            }
            Loggi.Debug("SQL: " + sqlText.ToString());
        }

        private void SetQueryString(DataTable table, DataRow row, DbCommand command, String queryPrefix, List<string> columnNames)
        {
            DataTable metaData = GetMetaDataTable(table.TableName);
            StringBuilder query = new StringBuilder();
            foreach (string columnName in columnNames)
            {
                Type columnType = GetColumnType(columnName, metaData, table);
                object columnValue = ConvertType(columnType, row[columnName]);
                AddParameter(command, queryPrefix, query, columnName, columnType, columnValue);
            }
            command.CommandText = query.ToString();
        }

        private object ConvertType(Type columnType, object columnValue)
        {
            if (columnValue == null || 
                DBNull.Value.Equals(columnValue) ||
                !(columnValue is string) ||
                columnType.IsAssignableFrom(columnValue.GetType()))
            {
                return columnValue;
            }

            string valueAsText = (string)columnValue;
            if ("*".Equals(valueAsText) || valueAsText.IndexOf("%") > -1)
            {
                return valueAsText;
            }

            try
            {
                return TypeConverter.ChangeType(valueAsText, columnType);
            }
            catch (Exception)
            {
                return columnValue;
            }
        }

        private Type GetColumnType(string columnName, DataTable metaData, DataTable table)
        {
            DataColumn column = metaData.Columns[columnName];
            if (column != null)
            {
                return column.DataType;
            }
            return table.Columns[columnName].DataType;
        }

        private void AddParameter(DbCommand command, String queryPrefix, StringBuilder query, string columnName, Type columnType, object columnValue)
        {
            AddParameterPrefix(queryPrefix, query);
            query.Append(columnName);
            if (DBNull.Value.Equals(columnValue) || columnValue == null)
            {
                query.Append(" is null");
            }
            else if ("*".Equals(columnValue))
            {
                query.Append(" is not null");
            }
            else
            {
                if (columnValue is string &&
                    (((string)columnValue).StartsWith("%") || ((string)columnValue).EndsWith("%")))
                {
                    query.Append(" like ");
                }
                else
                {
                    query.Append(" = ");
                }
                query.Append(connection.ParameterPrefix).Append(columnName);
                AddParameter(command, columnName, columnType, columnValue);
            }
        }

        private void AddParameterPrefix(string queryPrefix, StringBuilder query)
        {
            if (query.Length == 0)
            {
                query.Append(queryPrefix);
            }
            else
            {
                query.Append(" and ");
            }
        }

        internal void AddParameter(DbCommand command, string columnName, Type columnType, object parameterValue)
        {
            DbParameter parameter = connection.ProviderFactory.CreateParameter();
            parameter.ParameterName = connection.ParameterPrefix + columnName;
            SetParameterValue(columnType, parameterValue, parameter);
            command.Parameters.Add(parameter);
        }

        private void SetParameterValue(Type columnType, object parameterValue, DbParameter parameter)
        {
            if (columnType == null)
            {
                parameter.Value = parameterValue;
                return;
            }

            if (columnType == typeof(byte[]))
            {
                parameter.DbType = DbType.Binary;
            }
            parameter.Value = parameterValue;
        }

        // FIXME : DataTableValidator の機能が一部こっちに入っていてきれいに役割分担できていないのでイマイチ。
        public DataTable Select(List<string> keyColumns, DataTable keyTable, Table tableInfo)
        {
            Use(keyTable);
            string queryPrefix = GetQueryPrefixForSelect(keyTable);
            DataTable resultTable = new DataTable(keyTable.TableName);
            List<Row> rowInfo = tableInfo.Rows;
            int rowIndex = 0;
            foreach (DataRow keyRow in keyTable.Rows)
            {
                DbCommand command = connection.CreateCommand();
                Select(command, keyColumns, keyTable, keyRow, resultTable, tableInfo, rowInfo[rowIndex++], queryPrefix);
            }
            return resultTable;
        }

        private string GetQueryPrefixForSelect(DataTable table)
        {
            StringBuilder queryString = new StringBuilder();
            foreach (DataColumn column in table.Columns)
            {
                if (queryString.Length == 0)
                {
                    queryString.Append("select ");
                }
                else
                {
                    queryString.Append(",");
                }
                queryString.Append(column.ColumnName);
            }
            queryString.Append(" from ").Append(table.TableName).Append(" where ");
            return queryString.ToString();
        }

        private void Select(DbCommand command, List<string> keyColumns, DataTable keyTable, DataRow keyRow, DataTable resultTable, Table tableInfo, Row rowInfo, string queryPrefix)
        {
            SetQueryString(keyTable, keyRow, command, queryPrefix, keyColumns);
            DataTable result = ExecuteQuery(command);
            result.TableName = keyTable.TableName;
            DataRow source = SelectResultRow(result, keyRow, tableInfo, rowInfo, keyColumns);
            if (resultTable.Columns.Count == 0 && source != null)
            {
                foreach(DataColumn column in source.Table.Columns) 
                {
                    resultTable.Columns.Add(new DataColumn(column.ColumnName, column.DataType)); 
                }
            }
            DataRow destination = resultTable.NewRow();
            Copy(source, destination);
            resultTable.Rows.Add(destination);
        }

        // FIXME : この処理は本当は DataTableValidator でやるべき
        private DataRow SelectResultRow(DataTable result, DataRow keyRow, Table tableInfo, Row rowInfo, List<string> keyColumns)
        {
            int rowCount = result.Rows.Count;
            if (rowInfo.Deleted)
            {
                if (rowCount == 0)
                {
                    return null;
                }
                else
                {
                    Assertie.Fail("M_Fixture_Temp_DatabaseValidator_UnexpectedData", result.TableName, tableInfo, rowInfo, ToString(keyColumns, keyRow));
                }
            }

            if (rowCount == 0)
            {
                string message = tableInfo.HasKeyColumn ?
                    "M_Fixture_Temp_DatabaseValidator_NotFound" :
                    "M_Fixture_Temp_DatabaseValidator_NotFound_With_Comment";
                Assertie.Fail(message, result.TableName, tableInfo, rowInfo, ToString(keyColumns, keyRow));
            }

            if (rowCount > 1)
            {
                Assertie.Fail("M_Fixture_Temp_DatabaseValidator_OneMoreData", result.TableName, tableInfo, rowInfo, ToString(keyColumns, keyRow));
            }

            return result.Rows[0];
        }

        private string ToString(List<string> keyColumns, DataRow keyRow)
        {
            StringBuilder text = new StringBuilder();
            foreach (string columnName in keyColumns)
            {
                if (text.Length == 0)
                { 
                    text.Append("{");
                }
                else
                {
                    text.Append(", ");
                }

                text.Append(columnName).Append("=").Append(keyRow[columnName]);
            }
            text.Append("}");
            return text.ToString();
        }

        private void Copy(DataRow source, DataRow destination)
        {
            if (source == null)
            {
                return;
            }
            foreach (DataColumn column in source.Table.Columns)
            {
                string columnName = column.ColumnName;
                destination[columnName] = source[columnName];
            }
        }

        private DataTable ExecuteQuery(DbCommand command)
        {
            PrintSQL(command);
            DbDataAdapter adapter = connection.ProviderFactory.CreateDataAdapter();
            adapter.SelectCommand = command;
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public DataTable GetMetaData(Table table)
        {
            string[] tableNameAndDatabaseName = SplitTableNameAndDatabaseName(table.Name);
            Use(tableNameAndDatabaseName[1]);
            try
            {
                DataTable metaData = GetMetaDataTable(tableNameAndDatabaseName[0]);
                metaData.TableName = tableNameAndDatabaseName[0];
                return metaData;
            }
            catch (Exception e)
            {
                throw new ConfigException(e, "M_Fixture_Temp_Database_GetMetaData", tableNameAndDatabaseName[0], table);
            }
        }

        private DataTable GetMetaDataTable(string tableName)
        {
            MetaData metaData = GetMetaData(tableName);

            if (metaData.MetaDataTable == null)
            {
                metaData.MetaDataTable = ExecuteQuery("select * from " + tableName + " where 1=2");
            }
            return metaData.MetaDataTable;
        }

        private MetaData GetMetaData(string tableName)
        {
            MetaData metaData = null;
            if (!metaDatas.TryGetValue(tableName, out metaData))
            {
                metaData = new MetaData();
                metaDatas[tableName] = metaData;

            }
            return metaData;
        }

        public static DataTable ExecuteQuery(string databaseName, string queryString)
        {
            using (Database database = new Database())
            {
                if (databaseName != null)
                {
                    database.Use(databaseName);
                }
                return database.ExecuteQuery(queryString);
            }
        }

        private DataTable ExecuteQuery(string queryString)
        {
            DbCommand command = CreateCommand(queryString);
            return ExecuteQuery(command);
        }

        public static void ExecuteNonQuery(string databaseName, string sql)
        {
            using (Database database = new Database())
            {
                if (databaseName != null)
                {
                    database.Use(databaseName);
                }
                DbCommand command = database.CreateCommand(sql);
                database.ExecuteNonQuery(command);
                database.Commit();
            }
        }

        internal DbCommand CreateCommand(string sql)
        {
            return connection.CreateCommand(sql);
        }
    }
}
