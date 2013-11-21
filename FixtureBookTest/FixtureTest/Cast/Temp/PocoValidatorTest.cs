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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using XPFriend.Fixture;
using XPFriend.Fixture.Cast.Temp;
using XPFriend.Fixture.Staff;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class PocoValidatorTest
    {
        [TestMethod]
        public void HasRoleはInitializeで指定されたテストケースに取得データセクションがなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            PocoValidator validator = GetPocoValidator(sheet.GetCase("ロールなし"));

            // expect
            Assert.IsFalse(validator.HasRole(null, null));
        }

        [TestMethod]
        public void HasRoleはInitializeで指定されたテストケースに取得データセクションがあればtrueを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            PocoValidator validator = GetPocoValidator(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsTrue(validator.HasRole(null, null));
        }

        [TestMethod]
        public void HasRole引数のクラスがDictionaryでもtrueを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            PocoValidator validator = GetPocoValidator(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsTrue(validator.HasRole(new Dictionary<string, string>(), null));
        }

        [TestMethod]
        public void 例外のValidate実行時に例外が発生しない場合はAssertionFaildExceptionとなる()
        {
            try
            {
                new FixtureBook().Validate<Exception>(() => { });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private PocoValidator GetPocoValidator(Case testCase)
        {
            TempObjectValidator parent = new TempObjectValidator();
            parent.Initialize(testCase);
            return parent.pocoValidator;
        }

        [TestMethod]
        [Fixture("stringを検証できる")]
        public void stringを検証できる_正常()
        {
            FixtureBook.ExpectReturn(() => { return "aa"; });
        }

        [TestMethod]
        [Fixture("stringを検証できる")]
        public void stringを検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { return "bb"; });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("intを検証できる")]
        public void intを検証できる_正常()
        {
            FixtureBook.ExpectReturn(() => { return 1; });
        }

        [TestMethod]
        [Fixture("intを検証できる")]
        public void intを検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { return 2; });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("nullを検証できる")]
        public void nullを検証できる_正常()
        {
            FixtureBook.ExpectReturn(() => { string a = null; return a; });
        }

        [TestMethod]
        [Fixture("nullを検証できる")]
        public void nullを検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { return "a"; });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("stringの配列を検証できる")]
        public void stringの配列を検証できる_正常()
        {
            FixtureBook.ExpectReturn(() => { return new string[] { "aa", "b" }; });
        }

        [TestMethod]
        [Fixture("stringの配列を検証できる")]
        public void stringの配列を検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { return "aa"; });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("intの配列を検証できる")]
        public void intの配列を検証できる_正常()
        {
            FixtureBook.ExpectReturn(() => { return new int[] { 1, 2 }; });
        }

        [TestMethod]
        [Fixture("intの配列を検証できる")]
        public void intの配列を検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { return new int[] { 1, 3 }; });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("nullと空文字列の配列を検証できる")]
        public void nullと空文字列の配列を検証できる_正常()
        {
            FixtureBook.ExpectReturn(() => { return new string[] { null, "" }; });
        }

        [TestMethod]
        [Fixture("nullと空文字列の配列を検証できる")]
        public void nullと空文字列の配列を検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { return new string[] { "", "" }; });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("stringの配列を検証できる")]
        public void stringのリストを検証できる_正常()
        {
            FixtureBook.ExpectReturn(() => { return new List<string>() { "aa", "b" }; });
        }

        [TestMethod]
        [Fixture("stringの配列を検証できる")]
        public void stringのリストを検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { return new List<string>() { "aa", "bb" }; });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("intの配列を検証できる")]
        public void intのリストを検証できる_正常()
        {
            FixtureBook.ExpectReturn(() => { return new List<int>() { 1, 2 }; });
        }

        [TestMethod]
        [Fixture("intの配列を検証できる")]
        public void intのリストを検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { return new List<int>() { 1, 3 }; });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("nullと空文字列の配列を検証できる")]
        public void nullと空文字列のリストを検証できる_正常()
        {
            FixtureBook.ExpectReturn(() => { return new List<string>() { null, "" }; });
        }

        [TestMethod]
        [Fixture("nullと空文字列の配列を検証できる")]
        public void nullと空文字列のリストを検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { return new List<string>() { "", "" }; });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
