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
    public class RowTest
    {
        private Case testCase;

        [TestInitialize]
        public void Setup()
        {
            Sheet sheet = Book.GetInstance(BookTest.BookFilePath).GetSheet("Sheet02");
            testCase = sheet.GetCase("テストケース001");
        }

        [TestMethod]
        public void Columnsは行中の列情報を返す()
        {
            Columnsは行中の列情報を返す("B", "T001", 0, TestUtil.ToList("p01"), TestUtil.ToList("*"));
            Columnsは行中の列情報を返す("C", "T002", 0, TestUtil.ToList("p02", "p03", "p04", "p05", "p06", "p07"), TestUtil.ToList("1", "s01", "100.0", "true", "2013/12/12", "1234.0"));
            Columnsは行中の列情報を返す("C", "T002", 1, TestUtil.ToList("p02", "p03", "p04", "p05", "p06", "p07"), TestUtil.ToList("2", "s02", "200.0", "false", "2013-12-13 00:00:00", "1234"));
            Columnsは行中の列情報を返す("G", ".CSV", 0, TestUtil.ToList("delete", "path"), TestUtil.ToList("true", "target/test-classes/test/fc-testA1.txt"));
            Columnsは行中の列情報を返す("G", ".CSV", 1, TestUtil.ToList("delete", "expectedFilePath"), TestUtil.ToList("false", "target/test-classes/test/fc-testA2.txt"));
        }

        private void Columnsは行中の列情報を返す(string sectionName, string tableName, int index, List<string> columnNames, List<string> columnValues)
        {
            // setup
            Section section = testCase.GetSection(sectionName);
            Table table = section.GetTable(tableName);
            Row row = table.Rows[index];
            Console.WriteLine(sectionName + " - " + tableName + "[" + index + "]");

            // expect
            Assert.AreEqual(columnNames.Count, row.Values.Count);
            for (int i = 0; i < columnNames.Count; i++)
            {
                string key = columnNames[i];
                Assert.IsTrue(row.Values.ContainsKey(key));
                string value = row.Values[key];
                Console.WriteLine(" " + key + "=" + value + ", expected=" + columnValues[i]);
                Assert.AreEqual(columnValues[i], value);
            }
            Console.WriteLine();
        }

        [TestMethod]
        public void IndexはExcelファイル上の行番号を返す()
        {
            IndexはExcelファイル上の行番号を返す("B", "T001", 0, 10);
            IndexはExcelファイル上の行番号を返す("C", "T002", 0, 16);
            IndexはExcelファイル上の行番号を返す("C", "T002", 1, 17);
            IndexはExcelファイル上の行番号を返す("G", ".CSV", 0, 49);
            IndexはExcelファイル上の行番号を返す("G", ".CSV", 1, 50);
        }

        private void IndexはExcelファイル上の行番号を返す(string sectionName, string tableName, int index, int rowIndex)
        {
            // setup
            Section section = testCase.GetSection(sectionName);
            Table table = section.GetTable(tableName);
            Row row = table.Rows[index];

            // expect
            Assert.AreEqual(rowIndex, row.Index);
        }

        [TestMethod]
        public void Deletedは削除フラグが設定されているかどうかを返す()
        {
            Deletedは削除フラグが設定されているかどうかを返す("C", "T002", 0, false);
            Deletedは削除フラグが設定されているかどうかを返す("C", "T002", 1, true);
            Deletedは削除フラグが設定されているかどうかを返す("D", "P001", 0, true);
            Deletedは削除フラグが設定されているかどうかを返す("D", "P002", 0, true);
            Deletedは削除フラグが設定されているかどうかを返す("D", "P002", 1, false);
        }

        private void Deletedは削除フラグが設定されているかどうかを返す(string sectionName, string tableName, int index, bool deleted)
        {
            // setup
            Section section = testCase.GetSection(sectionName);
            Table table = section.GetTable(tableName);
            Row row = table.Rows[index];

            // expect
            Assert.AreEqual(deleted, row.Deleted);
        }

        [TestMethod]
        public void ToStringは行番号と削除指定行かどうかを表す文字列を返す()
        {
            ToStringは行番号と削除指定行かどうかを表す文字列を返す("C", "T002", 0, false, 16);
            ToStringは行番号と削除指定行かどうかを表す文字列を返す("C", "T002", 1, true, 17);
        }

        private void ToStringは行番号と削除指定行かどうかを表す文字列を返す(string sectionName, string tableName, int index, bool deleted, int rowNumber)
        {
            // setup
            Section section = testCase.GetSection(sectionName);
            Table table = section.GetTable(tableName);
            Row row = table.Rows[index];

            // when
            string s = row.ToString();

            // then
            Console.WriteLine(s);
            Assert.IsTrue(s.EndsWith(rowNumber.ToString()));
        }
    }
}
