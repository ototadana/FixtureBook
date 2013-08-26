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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture.Staff;
using System.Collections.Generic;

namespace XPFriend.FixtureTest.Staff
{
    [TestClass]
    public class TableTest
    {
        private Case testCase;

        [TestInitialize]
        public void Setup()
        {
            Sheet sheet = Book.GetInstance(BookTest.BookFilePath).GetSheet("Sheet02");
            testCase = sheet.GetCase("テストケース001");
        }

        [TestMethod]
        public void ToStringはセクション情報とテーブル名を表す文字列を返す()
        {
            // setup
            Section section = testCase.GetSection("B");
            Table table = section.GetTable("T001");

            // when
            string text = table.ToString();

            // then
            Assert.IsTrue(text.IndexOf("BookTest.xlsx") > -1);
            Assert.IsTrue(text.IndexOf("Sheet02") > -1);
            Assert.IsTrue(text.IndexOf("テストケース001") > -1);
            Assert.IsTrue(text.IndexOf("B.") > -1);
            Assert.IsTrue(text.IndexOf("T001") > -1);
        }

        [TestMethod]
        public void GetSectionはこのテーブルが属すセクションを返す()
        {
            GetSectionはこのテーブルが属すセクションを返す("B", "T001");
            GetSectionはこのテーブルが属すセクションを返す("C", "T002");
            GetSectionはこのテーブルが属すセクションを返す("D", "P001");
            GetSectionはこのテーブルが属すセクションを返す("D", "P002");
            GetSectionはこのテーブルが属すセクションを返す("E", "R001");
            GetSectionはこのテーブルが属すセクションを返す("F", "T003");
            GetSectionはこのテーブルが属すセクションを返す("G", ".CSV");
        }

        private void GetSectionはこのテーブルが属すセクションを返す(string sectionName, string tableName)
        {
            // setup
            Section section = testCase.GetSection(sectionName);
            Table table = section.GetTable(tableName);

            // expect
            Assert.AreSame(section, table.Section);
        }

        [TestMethod]
        public void Nameはこのテーブルの名前を返す()
        {
            Nameはこのテーブルの名前を返す("B", "T001");
            Nameはこのテーブルの名前を返す("C", "T002");
        }

        private void Nameはこのテーブルの名前を返す(string sectionName, string tableName)
        {
            // setup
            Section section = testCase.GetSection(sectionName);
            Table table = section.GetTable(tableName);

            // expect
            Assert.AreEqual(tableName, table.Name);
        }

        [TestMethod]
        public void Rowsはテーブルに定義されている行情報を返す()
        {
            Rowsはテーブルに定義されている行情報を返す("B", "T001", 1);
            Rowsはテーブルに定義されている行情報を返す("C", "T002", 2);
            Rowsはテーブルに定義されている行情報を返す("D", "P001", 1);
            Rowsはテーブルに定義されている行情報を返す("D", "P002", 2);
            Rowsはテーブルに定義されている行情報を返す("E", "R001", 1);
            Rowsはテーブルに定義されている行情報を返す("F", "T003", 1);
            Rowsはテーブルに定義されている行情報を返す("G", ".CSV", 2);
        }

        private void Rowsはテーブルに定義されている行情報を返す(string sectionName, string tableName, int rowCount)
        {
            // setup
            Section section = testCase.GetSection(sectionName);
            Table table = section.GetTable(tableName);

            // expect
            Assert.AreEqual(rowCount, table.Rows.Count);
        }

        [TestMethod]
        public void 同一セクション内に同一名のテーブルが複数定義されている場合その情報はひとつにマージされる()
        {
            同一セクション内に同一名のテーブルが複数定義されている場合その情報はひとつにマージされる(0, TestUtil.ToList("1","s01","100.0","true","2013/12/12"));
            同一セクション内に同一名のテーブルが複数定義されている場合その情報はひとつにマージされる(1, TestUtil.ToList("2", "s02", "200.0", "false", "2013-12-13 00:00:00"));
        }

        private void 同一セクション内に同一名のテーブルが複数定義されている場合その情報はひとつにマージされる(int index, List<string> columnValues)
        {
            // setup
            Sheet sheet = Book.GetInstance(BookTest.BookFilePath).GetSheet("Sheet03");
            Section section = sheet.GetCase("テストケース001").GetSection("C");
            Table table = section.GetTable("T002");
            List<Row> rows = table.Rows;

		    // expect
            Assert.AreEqual(2, rows.Count);
            ContainsAll(columnValues, rows[index].Values);
        }

        private void ContainsAll(List<string> columnValues, Dictionary<string, string> dictionary)
        {
            Assert.AreEqual(columnValues.Count, dictionary.Count);
            for (int i = 0; i < columnValues.Count; i++)
            {
                Assert.IsTrue(dictionary.ContainsValue(columnValues[i]));
            }
        }
    }
}
