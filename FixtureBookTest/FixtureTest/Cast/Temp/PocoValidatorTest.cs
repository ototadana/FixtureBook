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
using XPFriend.FixtureTest.Cast.Temp.Datas;
using XPFriend.Junk;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class PocoValidatorTest
    {
        private FixtureBook fixtureBook = new FixtureBook();

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
        [Fixture("PocoValidatorTest", "データあり")]
        public void Validateは指定されたリストが予想結果と等しいかどうかを調べる()
        {
            // setup
            List<Data> actual = fixtureBook.GetList<Data>(); 

            // expect
            fixtureBook.Validate(actual);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "データあり")]
        public void Validateは指定されたオブジェクトが予想結果と等しいかどうかを調べる()
        {
            // setup
            Data actual = fixtureBook.GetObject<Data>("Data1");

            // expect
            fixtureBook.Validate(actual, "Data1");
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "データあり")]
        public void Validateは指定された配列が予想結果と等しいかどうかを調べる()
        {
            // setup
            Data[] actual = fixtureBook.GetArray<Data>();

            // expect
            fixtureBook.Validate(actual);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "データあり")]
        public void Validateにnullを渡すと指定された定義が存在しなければエラーにならない()
        {
            // expect
            fixtureBook.Validate(null, "xxx");
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "データなし")]
        public void Validateは要素数ゼロのリストもチェックできる()
        {
            // setup
            List<Data> actual = fixtureBook.GetList<Data>(); 

            // expect
            fixtureBook.Validate(actual, "Data");
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "データなし")]
        public void Validateは要素数ゼロのリストも一つの定義しかなければ定義名指定なしでチェックできる()
        {
            // setup
            List<Data> actual = fixtureBook.GetList<Data>();

            // expect
            fixtureBook.Validate(actual);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "データあり")]
        public void Validateは複数のテーブル定義がある場合には要素数ゼロのリストはチェックできない()
        {
            // setup
            List<Data> actual = new List<Data>();
            try
            {
                // when
                fixtureBook.Validate(actual);
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                Assert.AreEqual("M_Fixture_Temp_ObjectOperator_GetTableName", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("PocoValidatorTest.xlsx#PocoValidatorTest") > -1);
            }

        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "データあり")]
        public void Validateでは取得データの行数が実際と異なる場合はエラーになる()
        {
            // setup
            List<Data> actual = fixtureBook.GetList<Data>(null);

            try
            {
                // when
                fixtureBook.Validate(actual, "行数違い");
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e);
                Assert.IsTrue(e.ToString().IndexOf("<1>") > -1);
                Assert.IsTrue(e.ToString().IndexOf("<3>") > -1);
            }
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "データあり")]
        public void Validateでは予想結果のセル値が実際と異なる場合はエラーになる()
        {
            Validateでは予想結果のセル値が実際と異なる場合はエラーになる("文字列違い", "<ax>", "<a>");
            Validateでは予想結果のセル値が実際と異なる場合はエラーになる("数値違い", "<2>", "<1>");
            Validateでは予想結果のセル値が実際と異なる場合はエラーになる("文字列違い（2行目）", "<bx>", "<b>");
        }

        private void Validateでは予想結果のセル値が実際と異なる場合はエラーになる(string typeName, string expectedValue, string actualValue)
        {
            // setup
            List<Data> actual = fixtureBook.GetList<Data>(null);

            try
            {
                // when
                fixtureBook.Validate(actual, typeName);
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e);
                Assert.IsTrue(e.ToString().IndexOf(expectedValue) > -1);
                Assert.IsTrue(e.ToString().IndexOf(actualValue) > -1);
            }
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が空の場合はnullでも空文字列でもOK")]
        public void セル内容が空の場合はnullでも空文字列でもOK()
        {
            セル内容が空の場合はnullでも空文字列でもOK("");
            セル内容が空の場合はnullでも空文字列でもOK(null);
        }

        private void セル内容が空の場合はnullでも空文字列でもOK(string actual)
        {
            // setup
            Data data = new Data();
            data.Text1 = actual;

            // expect
            fixtureBook.Validate(data, null);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が空の場合はnullでも空文字列でもOK")]
        public void セル内容が空の場合はnullまたは空文字列以外はNG()
        {
            // setup
            Data data = new Data();
            data.Text1 = "xxx";

            try
            {
                // when
                fixtureBook.Validate(data, null);
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e);
                Assert.IsTrue(e.ToString().IndexOf("text1") > -1);
            }
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が${EMPTY}の場合は空文字列のみOK")]
        public void セル内容が_EMPTY_の場合は空文字列のみOK()
        {
            // setup
            Data data = new Data();
            data.Text1 = "";

            // expect
            fixtureBook.Validate(data);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が${EMPTY}の場合は空文字列のみOK")]
        public void セル内容が_EMPTY_の場合は空文字列以外はNG()
        {
            セル内容が_EMPTY_の場合は空文字列以外はNG("xxx");
            セル内容が_EMPTY_の場合は空文字列以外はNG(null);
        }

        private void セル内容が_EMPTY_の場合は空文字列以外はNG(string actual)
        {
            // setup
            Data data = new Data();
            data.Text1 = actual;

            try
            {
                // when
                fixtureBook.Validate(data);
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e);
                Assert.IsTrue(e.ToString().IndexOf("text1") > -1);
            }
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が${NULL}の場合はnullのみOK")]
        public void セル内容が_NULL_の場合はnullのみOK()
        {
            // setup
            Data data = new Data();
            data.Text1 = null;

            // expect
            fixtureBook.Validate(data);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が${NULL}の場合はnullのみOK")]
        public void セル内容が_NULL_の場合はnull以外はNG()
        {
            セル内容が_NULL_の場合はnull以外はNG("xxx");
            セル内容が_NULL_の場合はnull以外はNG("");
        }

        private void セル内容が_NULL_の場合はnull以外はNG(string actual)
        {
            // setup
            Data data = new Data();
            data.Text1 = actual;

            try
            {
                // when
                fixtureBook.Validate(data);
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e);
                Assert.IsTrue(e.ToString().IndexOf("text1") > -1);
            }
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が空白スペースの場合はそのまま比較できる")]
        public void セル内容が空白スペースの場合はそのまま比較できる()
        {
            // setup
            Data data = new Data();
            data.Text1 = " ";

            // expect
            fixtureBook.Validate(data);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が*の場合はnullまたは空文字列以外ならば何でもOK")]
        public void セル内容がアスタリスクの場合はnullまたは空文字列以外ならば何でもOK()
        {
            // setup
            Data data = new Data();
            data.Text1 = "xxx";

            // expect
            fixtureBook.Validate(data);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が*の場合はnullまたは空文字列以外ならば何でもOK")]
        public void セル内容がアスタリスクの場合はnullまたは空文字列だとNG()
        {
            セル内容がアスタリスクの場合はnullまたは空文字列だとNG(null);
            セル内容がアスタリスクの場合はnullまたは空文字列だとNG("");
        }

        private void セル内容がアスタリスクの場合はnullまたは空文字列だとNG(string actual)
        {
            // setup
            Data data = new Data();
            data.Text1 = actual;

            try
            {
                // when
                fixtureBook.Validate(data);
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e);
                Assert.IsTrue(e.ToString().IndexOf("text1") > -1);
            }
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が%を含む場合は前方一致、後方一致、部分一致で判定できる")]
        public void セル内容がパーセントを含む場合は前方一致_後方一致_部分一致で判定できる()
        {
            // setup
            List<Data> list = new List<Data>();
            for(int i = 0; i < 3; i++) 
            {
			    Data data = new Data();
			    data.Text1 = "abcd";
                list.Add(data);
		    }

            // expect
            fixtureBook.Validate(list);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が%を含む場合は前方一致、後方一致、部分一致で判定できる")]
        public void セル内容がパーセントを含む場合は前方一致_後方一致_部分一致で判定できる_エラーのケース()
        {
            セル内容がパーセントを含む場合は前方一致_後方一致_部分一致で判定できる_エラーのケース("abcx", "%cd");
            セル内容がパーセントを含む場合は前方一致_後方一致_部分一致で判定できる_エラーのケース("axcd", "ab%");
            セル内容がパーセントを含む場合は前方一致_後方一致_部分一致で判定できる_エラーのケース("abxcd", "%bc%");
        }

        private void セル内容がパーセントを含む場合は前方一致_後方一致_部分一致で判定できる_エラーのケース(string actual, string error)
        {
            // setup
            List<Data> list = new List<Data>();
            for (int i = 0; i < 3; i++)
            {
                Data data = new Data();
                data.Text1 = actual;
                list.Add(data);
            }
            
            try
            {
                // when
                fixtureBook.Validate(list);
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e);
                Assert.IsTrue(e.ToString().IndexOf(error) > -1);
            }
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が``で囲まれている場合は正規表現としてのマッチングを行う")]
        public void セル内容がバッククォートで囲まれている場合は正規表現としてのマッチングを行う()
        {
            セル内容がバッククォートで囲まれている場合は正規表現としてのマッチングを行う("abcd");
            セル内容がバッククォートで囲まれている場合は正規表現としてのマッチングを行う("abxd");
        }

        private void セル内容がバッククォートで囲まれている場合は正規表現としてのマッチングを行う(string actual)
        {
            // setup
            Data data = new Data();
            data.Text1 = actual;

            // expect
            fixtureBook.Validate(data);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が``で囲まれている場合は正規表現としてのマッチングを行う")]
        public void セル内容がバッククォートで囲まれている場合は正規表現としてのマッチングを行う_エラーのケース()
        {
            // setup
            Data data = new Data();
            data.Text1 = "abccd";

            try
            {
                // when
                fixtureBook.Validate(data);
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e);
                Assert.IsTrue(e.ToString().IndexOf("<`ab.?d`>") > -1);
                Assert.IsTrue(e.ToString().IndexOf("<abccd>") > -1);
            }
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "配列データ")]
        public void バーティカルバー区切りの値で表現された配列およびリスト項目をチェックできる()
        {
            // setup
            ArrayData arrayData = fixtureBook.GetObject<ArrayData>();

            // expect
            fixtureBook.Validate(arrayData);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "親子データ")]
        public void 親子構造になったデータをチェックできる()
        {
            // setup
            Order order = fixtureBook.GetObject<Order>();

            // expect
            fixtureBook.Validate(order);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が${TODAY}の場合は本日の日付のみOK")]
        public void セル内容がTODAYの場合は本日の日付のみOK()
        {
            // setup
            Data data = new Data();
            data.Date1 = DateTime.Now;

            // expect
            fixtureBook.Validate(data);
        }

        [TestMethod]
        [Fixture("PocoValidatorTest", "セル内容が${TODAY}の場合は本日の日付のみOK")]
        public void セル内容がTODAYの場合は本日の日付のみOK_エラーのケース()
        {
            // setup
            Data data = new Data();
            data.Date1 = new DateTime(1, 1, 1);

            try
            {
                // when
                fixtureBook.Validate(data);
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e);
            }
        }

        private PocoValidator GetPocoValidator(Case testCase)
        {
            TempObjectValidator parent = new TempObjectValidator();
            parent.Initialize(testCase);
            return parent.pocoValidator;
        }
    }
}
