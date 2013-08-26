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
using XPFriend.Junk;

namespace XPFriend.FixtureTest.Staff
{
    [TestClass]
    public class SectionTest
    {
        private Case testCase;

        [TestInitialize]
        public void Setup()
        {
            Sheet sheet = Book.GetInstance(BookTest.BookFilePath).GetSheet("Sheet01");
            testCase = sheet.GetCase("テストケース001");
        }

        [TestMethod]
        public void MaxNumberは最大のセクション番号を返す()
        {
            // expect
            Assert.AreEqual(7, Section.MaxNumber);
        }

        [TestMethod]
        public void Nameはセクション名を返す()
        {
            Nameはセクション名を返す(1, "A. テストケース");
            Nameはセクション名を返す(2, "C. テストデータ");
            Nameはセクション名を返す(3, "B. テストデータクリア条件");
            Nameはセクション名を返す(4, "F. 更新後データ");
            Nameはセクション名を返す(5, "D. パラメタ");
            Nameはセクション名を返す(6, "E. 取得データ");
            Nameはセクション名を返す(7, "G. ファイルチェック");
        }

        private void Nameはセクション名を返す(int number, string name)
        {
            // setup
            Section section = testCase.GetSection(number);

            // expect
            Assert.AreEqual(name, section.Name);
        }

        [TestMethod]
        public void Numberはセクション番号を返す()
        {
            Numberはセクション番号を返す(1, "A. テストケース");
            Numberはセクション番号を返す(2, "C. テストデータ");
            Numberはセクション番号を返す(3, "B. テストデータクリア条件");
            Numberはセクション番号を返す(4, "F. 更新後データ");
            Numberはセクション番号を返す(5, "D. パラメタ");
            Numberはセクション番号を返す(6, "E. 取得データ");
            Numberはセクション番号を返す(7, "G. ファイルチェック");
        }

        private void Numberはセクション番号を返す(int number, string name)
        {
            // setup
            Section section = testCase.GetSection(number);

            // expect
            Assert.AreEqual(number, section.Number);
        }

        [TestMethod]
        public void ToStringはテストケースの情報とをセクション名を表す文字列を返す()
        {
            // setup
            Section section = testCase.GetSection(1);

            // expect
            Assert.AreEqual("BookTest.xlsx#Sheet01[テストケース001], A. テストケース", section.ToString());
        }

        [TestMethod]
        public void Caseはこのセクションが属すテストケースを返す()
        {
            // setup
            Section section = testCase.GetSection(1);

            // expect
            Assert.AreSame(testCase, section.Case);
        }

        [TestMethod]
        public void GetTableで存在しない名前を指定した場合は例外を発生する()
        {
            // setup
            Section section = testCase.GetSection(2);

            try
            {
                // when
                section.GetTable("xxx");
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Section_GetTable", e.ResourceKey);
            }
        }

        [TestMethod]
        public void GetTableでは指定した名前のテーブル定義を取得する()
        {
            GetTableでは指定した名前のテーブル定義を取得する("B", "T001");
            GetTableでは指定した名前のテーブル定義を取得する("C", "T002");
            GetTableでは指定した名前のテーブル定義を取得する("D", "P001");
            GetTableでは指定した名前のテーブル定義を取得する("D", "P002");
            GetTableでは指定した名前のテーブル定義を取得する("E", "R001");
            GetTableでは指定した名前のテーブル定義を取得する("F", "T003");
            GetTableでは指定した名前のテーブル定義を取得する("G", ".CSV");
        }

        private void GetTableでは指定した名前のテーブル定義を取得する(string sectionName, string tableName)
        {
            // setup
            Sheet sheet = Book.GetInstance(BookTest.BookFilePath).GetSheet("Sheet02");
            Section section = sheet.GetCase("テストケース001").GetSection(sectionName);

            // expect
            Assert.AreEqual(tableName, section.GetTable(tableName).Name);
        }

        [TestMethod]
        public void 不正なセクション名が定義されている場合セクション番号は0として扱われる()
        {
            // setup
            Sheet sheet = Book.GetInstance(BookTest.BookFilePath).GetSheet("Sheet03");

            // when
            Section section = sheet.GetCase("テストケース001").GetSection(0);

            // then
            Assert.AreEqual("あいうえお", section.Name);
        }
    }
}
