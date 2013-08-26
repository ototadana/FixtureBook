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
    public class CaseTest
    {
        Sheet sheet = Book.GetInstance(BookTest.BookFilePath).GetSheet("Sheet01");

        [TestMethod]
        public void Nameはテストケースの名前を返す()
        {
            Nameはテストケースの名前を返す("テストケース001");
            Nameはテストケースの名前を返す("テストケース002");
        }

        private void Nameはテストケースの名前を返す(string testCaseName)
        {
            // setup
            Case testCase = sheet.GetCase(testCaseName);

            // expect
            Assert.AreEqual(testCaseName, testCase.Name);
        }

        [TestMethod]
        public void Sheetはこのテストケースが属すシートを返す()
        {
            // setup
            Case testCase = sheet.GetCase("テストケース001");

            // expect
            Assert.AreSame(sheet, testCase.Sheet);
        }

        [TestMethod]
        public void GetSection_intとGetSection_Stringは同一インスタンスのセクションを返す()
        {
            GetSection_intとGetSection_Stringは同一インスタンスのセクションを返す(1, "A. test case");
            GetSection_intとGetSection_Stringは同一インスタンスのセクションを返す(2, "C. test data");
            GetSection_intとGetSection_Stringは同一インスタンスのセクションを返す(3, "B. clear condition");
            GetSection_intとGetSection_Stringは同一インスタンスのセクションを返す(4, "F. updated");
            GetSection_intとGetSection_Stringは同一インスタンスのセクションを返す(5, "D. parameters");
            GetSection_intとGetSection_Stringは同一インスタンスのセクションを返す(6, "E. expected result");
            GetSection_intとGetSection_Stringは同一インスタンスのセクションを返す(7, "G. file check");
        }

        private void GetSection_intとGetSection_Stringは同一インスタンスのセクションを返す(int number, string name)
        {
            // setup
            Case testCase = sheet.GetCase("テストケース001");

            // when
            var section1 = testCase.GetSection(number);
            var section2 = testCase.GetSection(name);

            // then
            Assert.AreSame(section1, section2);
        }


        [TestMethod]
        public void GetSection_intで不正なセクション番号を指定すると例外が発生する()
        {
            GetSection_intで不正なセクション番号を指定すると例外が発生する(-1);
            GetSection_intで不正なセクション番号を指定すると例外が発生する(8);
        }

        private void GetSection_intで不正なセクション番号を指定すると例外が発生する(int number)
        {
            // setup
            Case testCase = sheet.GetCase("テストケース001");

            try
            {
                // when
                testCase.GetSection(number);
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Case_GetSection", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("テストケース001") > -1);
            }
        }

        [TestMethod]
        public void ToStringはシートの情報とテストケース名を表す文字列を返す()
        {
            // setup
            Case testCase = sheet.GetCase("テストケース001");

            // expect
            Console.WriteLine(testCase.ToString());
            Assert.AreEqual("BookTest.xlsx#Sheet01[テストケース001]", testCase.ToString());
        }

        [TestMethod]
        public void GetObjectをDパラメタに何も定義されていない場合に呼び出すと例外が発生する()
        {
            // setup
            Sheet sheet = Book.GetInstance(BookTest.BookFilePath).GetSheet("Sheet03");
            Case testCase = sheet.GetCase("テストケース001");

            try
            {
                // when
                testCase.GetObject<object>("");
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Case_GetObject", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("テストケース001") > -1);
            }
        }

        [TestMethod]
        public void GetListをDパラメタに何も定義されていない場合に呼び出すと例外が発生する()
        {
            // setup
            Sheet sheet = Book.GetInstance(BookTest.BookFilePath).GetSheet("Sheet03");
            Case testCase = sheet.GetCase("テストケース001");

            try
            {
                // when
                testCase.GetList<object>("");
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Case_GetList", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("テストケース001") > -1);
            }
        }

        [TestMethod]
        public void GetArrayをDパラメタに何も定義されていない場合に呼び出すと例外が発生する()
        {
            // setup
            Sheet sheet = Book.GetInstance(BookTest.BookFilePath).GetSheet("Sheet03");
            Case testCase = sheet.GetCase("テストケース001");

            try
            {
                // when
                testCase.GetArray<object>("");
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Case_GetArray", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("テストケース001") > -1);
            }
        }

        [TestMethod]
        public void Validate_StorageをF更新後データに何も定義されていない場合に呼び出すと例外が発生する()
        {
            // setup
            Case testCase = sheet.GetCase("テストケース001");

            try
            {
                // when
                testCase.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Case_Validate_Storage", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("テストケース001") > -1);
            }
        }

        [TestMethod]
        public void Validate_ObjectをE取得データが定義されていない場合に呼び出すと例外が発生する()
        {
            // setup
            Case testCase = sheet.GetCase("テストケース001");

            try
            {
                // when
                testCase.Validate(null, "");
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Case_Validate_Object", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("テストケース001") > -1);
            }
        }
    }
}
