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
    public class SheetTest
    {
        Book book = Book.GetInstance(BookTest.BookFilePath);

        [TestMethod]
        public void Bookはこのシートが属するブックを返す()
        {
            // setup
            var sheet = book.GetSheet("Sheet01");

            // expect
            Assert.AreSame(book, sheet.Book);
        }

        [TestMethod]
        public void ToStringはBookの情報とシート名を表す文字列を返す()
        {
            ToStringはBookの情報とシート名を表す文字列を返す("Sheet01");
            ToStringはBookの情報とシート名を表す文字列を返す("Sheet02");
        }

        private void ToStringはBookの情報とシート名を表す文字列を返す(string sheetName)
        {
            // setup
            var sheet = book.GetSheet(sheetName);

            // expect
            Console.WriteLine(sheet.ToString());
            Assert.AreEqual(book.ToString() + "#" + sheetName, sheet.ToString()); 

        }

        [TestMethod]
        public void GetCaseは指定された名前のテストケースを返す()
        {
            GetCaseは指定された名前のテストケースを返す("テストケース001");
            GetCaseは指定された名前のテストケースを返す("テストケース002");
        }

        private void GetCaseは指定された名前のテストケースを返す(String testCaseName)
        {
            // setup
            var sheet = book.GetSheet("Sheet01");

            // when
            Case testCase = sheet.GetCase(testCaseName);

            // then
            Assert.AreEqual(testCaseName, testCase.Name);
        }



        [TestMethod]
        public void GetCaseで存在しないケース名を指定すると例外が発生する()
        {
            // setup
            var sheet = book.GetSheet("Sheet01");
            try
            {
                // when
                sheet.GetCase("zzz");
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e.ToString());
                Assert.AreEqual("M_Fixture_Sheet_GetCase", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("zzz") > -1);
                Assert.IsTrue(e.Message.IndexOf("Sheet01") > -1);
            }
        }
    }
}
