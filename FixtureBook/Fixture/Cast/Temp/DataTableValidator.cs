using System;
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
using System.Data;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;

namespace XPFriend.Fixture.Cast.Temp
{
    /// <summary>
    /// データベーステーブルの状態が予想結果と等しいかどうかを調べる。
    /// </summary>
    internal class DataTableValidator : ObjectValidatorBase, IObjectValidator
    {
        public DataTableValidator(IObjectValidator parent) : base(parent) { }

        public virtual bool HasRole(object obj, params string[] typeName)
        {
            return Section != null && Section.HasTable() && obj is DataTable;
        }

        protected override void Validate(object obj, string typeName)
        {
            DataTable dataTable = (DataTable)obj;
            base.ValidateInternal(dataTable.Rows, typeName, dataTable.TableName);
        }

        protected override object GetPropertyValue(object obj, string name, Table table, Row row)
        {
            DataRow dataRow = (DataRow)obj;
            if (dataRow.IsNull(name))
            {
                return null;
            }
            return dataRow[name];
        }
    }
}
