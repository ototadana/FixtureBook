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
using Oracle.DataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace XPFriend.FixtureTest.Tutorial
{
    public class EmployeeStore
    {
        public void Save(DataSet dataSet)
        {
            DataTable table = dataSet.Tables["EMPLOYEE"];
            using (OracleConnection connection = GetConnection())
            {
                OracleCommand insertCommand = new OracleCommand(
                    "INSERT INTO EMPLOYEE(ID, NAME, AGE, RETIRE, LAST_UPDATED)" + 
                    "VALUES(:ID, :NAME, :AGE, :RETIRE, :LAST_UPDATED)");
                insertCommand.Connection = connection;
                foreach(DataRow row in table.Rows) 
                {
                    insertCommand.Parameters.Clear();
                    insertCommand.Parameters.Add(new OracleParameter(":ID", row["ID"]));
                    insertCommand.Parameters.Add(new OracleParameter(":NAME", row["NAME"]));
                    insertCommand.Parameters.Add(new OracleParameter(":AGE", row["AGE"]));
                    insertCommand.Parameters.Add(new OracleParameter(":RETIRE", row["RETIRE"]));
                    insertCommand.Parameters.Add(new OracleParameter(":LAST_UPDATED", DateTime.Now));
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public void Delete(DataTable parameter)
        {
            if (parameter.Rows[0]["ID"] == DBNull.Value)
            {
                throw new ApplicationException("Invalid ID");
            }

            using (OracleConnection connection = GetConnection())
            {
                OracleCommand deleteCommand = new OracleCommand("DELETE FROM EMPLOYEE WHERE ID = :ID");
                deleteCommand.Connection = connection;
                deleteCommand.Parameters.Add(new OracleParameter(":ID", parameter.Rows[0]["ID"]));
                deleteCommand.ExecuteNonQuery();
            }
        }

        public DataTable GetAllEmployees()
        {
            using (OracleConnection connection = GetConnection())
            {
                OracleCommand selectCommand = new OracleCommand("SELECT * FROM EMPLOYEE");
                selectCommand.Connection = connection;
                OracleDataAdapter adapter = new OracleDataAdapter(selectCommand);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataSet GetEmployees(DataTable parameter)
        {
            using (OracleConnection connection = GetConnection())
            {
                OracleCommand selectCommand = new OracleCommand("SELECT * FROM EMPLOYEE where RETIRE = :RETIRE");
                selectCommand.Connection = connection;
                selectCommand.Parameters.Add(new OracleParameter(":RETIRE", parameter.Rows[0]["RETIRE"]));
                OracleDataAdapter adapter = new OracleDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet;
            }
        }

        private OracleConnection GetConnection()
        {
            OracleConnection connection = new OracleConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
            connection.Open();
            return connection;
        }
    }
}
