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

namespace XPFriend.Fixture.Staff
{
    /// <summary>
    /// テーブル。
    /// </summary>
    internal class Table
    {
        private Section section;
        private string tableName;
        private List<Column> columns = new List<Column>();
        private List<Row> rows = new List<Row>();

        /// <summary>
        /// テーブルを作成する。
        /// </summary>
        /// <param name="section">このテーブルが属するセクション</param>
        /// <param name="tableName">テーブル名</param>
        internal Table(Section section, string tableName)
        {
            this.section = section;
            this.tableName = tableName;
        }

        /// <summary>
        /// このテーブルが属するセクション。
        /// </summary>
        public Section Section
        {
            get { return section; }
        }

        /// <summary>
        /// テーブル名。
        /// </summary>
        public string Name
        {
            get { return tableName; }
        }

        /// <summary>
        /// 列データ。
        /// </summary>
        public List<Column> Columns
        {
            get { return columns; }
        }

        /// <summary>
        /// 行データ。
        /// </summary>
        public List<Row> Rows
        {
            get { return rows; }
        }

        /// <summary>
        /// 行データを追加する。
        /// </summary>
        /// <param name="index">行番号</param>
        /// <param name="deleted">削除指定行かどうか</param>
        /// <returns>追加した行データ</returns>
        public Row AddRow(int index, bool deleted)
        {
            Row row = new Row(index, deleted);
            rows.Add(row);
            return row;
        }

        public override string ToString()
        {
            return section.ToString() + " - " + Name;
        }
    }
}
