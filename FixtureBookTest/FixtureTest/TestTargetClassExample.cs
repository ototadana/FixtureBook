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
using System.Linq;
using System.Text;
using XPFriend.FixtureTest.Tutorial;

namespace XPFriend.FixtureTest
{
    public class TestTargetClassExample
    {
        public void Save(List<Employee> employees)
        {
            new EmployeeStore().Save(ToDataSet(employees));
        }

        public void Delete(List<Employee> employees)
        {
            new EmployeeStore().Delete(ToDataTable(employees));
        }

        public List<Employee> GetEmployees(List<Employee> employees)
        {
            DataSet dataSet = new EmployeeStore().GetEmployees(ToDataTable(employees));
            return ToList(dataSet);
        }

        public static List<Employee> ToList(DataSet dataSet)
        {
            return ToList(dataSet.Tables[0]);
        }

        public static List<Employee> ToList(DataTable table)
        {
            List<Employee> list = new List<Employee>(table.Rows.Count);
            foreach (DataRow row in table.Rows)
            {
                list.Add(new Employee()
                {
                    Id = (int)row["ID"],
                    Age = (int)((Int16)row["AGE"]),
                    Name = (string)row["NAME"],
                    Retire = (Int16)row["RETIRE"] != ((Int16)0),
                    LastUpdated = (DateTime)row["LAST_UPDATED"]
                });
            }
            return list;
        }

        public static DataSet ToDataSet(List<Employee> employees)
        {
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(ToDataTable(employees));
            return dataSet;
        }

        public static DataTable ToDataTable(List<Employee> employees)
        {
            DataTable table = new DataTable("EMPLOYEE");
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("NAME", typeof(string));
            table.Columns.Add("AGE", typeof(int));
            table.Columns.Add("RETIRE", typeof(int));
            table.Columns.Add("LAST_UPDATED", typeof(DateTime));

            foreach (Employee employee in employees)
            {
                DataRow row = table.NewRow();
                if (employee.Id == 0)
                {
                    row["ID"] = DBNull.Value;
                }
                else
                {
                    row["ID"] = employee.Id;
                }
                row["NAME"] = employee.Name;
                row["AGE"] = employee.Age;
                row["RETIRE"] = employee.Retire;
                row["LAST_UPDATED"] = employee.LastUpdated;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
