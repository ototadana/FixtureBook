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

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class DictionaryFactoryTest
    {
        private FixtureBook fixtureBook = new FixtureBook();


        [TestMethod]
        public void HasRoleはInitializeで指定されたテストケースにパラメタセクションがなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DictionaryFactory factory = GetDictionaryFactory(sheet.GetCase("ロールなし"));

            // expect
            Assert.IsFalse(factory.HasRole<object>(null));
        }

        [TestMethod]
        public void HasRoleは引数のクラスがDictionaryでなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DictionaryFactory factory = GetDictionaryFactory(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsFalse(factory.HasRole<object>(null));
        }

        [TestMethod]
        public void HasRoleは引数のクラスがDictionaryでありかつInitializeで指定されたテストケースにパラメタセクションがあればtrueを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DictionaryFactory factory = GetDictionaryFactory(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsTrue(factory.HasRole<Dictionary<string,object>>(null));
        }

        [TestMethod]
        [Fixture("DictionaryFactoryTest", "GetObjectは指定された名前の定義を読み込んでオブジェクト作成する")]
        public void GetListは指定された名前の定義を読み込みオブジェクトのリストを作成する()
        {
            // when
            List<Dictionary<string, object>> list = fixtureBook.GetList<Dictionary<string, object>>();

            // then
            Assert.AreEqual(2, list.Count);
            ValidateDictionary(list[0]);
        }

        [TestMethod]
        [Fixture("DictionaryFactoryTest", "GetObjectは指定された名前の定義を読み込んでオブジェクト作成する")]
        public void GetArrayは指定された名前の定義を読み込みオブジェクトの配列を作成する()
        {
            // when
            Dictionary<string, object>[] array = fixtureBook.GetArray<Dictionary<string, object>>();

            // then
            Assert.AreEqual(2, array.Length);
            ValidateDictionary(array[0]);
        }

        [TestMethod]
        public void GetObjectは指定された名前の定義を読み込んでオブジェクト作成する()
        {
            // when
            Dictionary<string, object> dictionary = fixtureBook.GetObject<Dictionary<string, object>>("Data");

            // then
            ValidateDictionary(dictionary);
        }
        private void ValidateDictionary(Dictionary<string, object> dictionary)
        {
		    Validate(dictionary["s1"], "a", typeof(string));
		    Validate(dictionary["s2"], "b", typeof(string));
		    Validate(dictionary["decimal1"], 1.111m, typeof(decimal));
		    Validate(dictionary["decimal2"], 2.222m, typeof(decimal));
		    Validate(dictionary["int1"], 3, typeof(int));
		    Validate(dictionary["int2"], 4, typeof(int));
		    Validate(dictionary["long1"], 5L, typeof(long));
		    Validate(dictionary["short1"], (short)6, typeof(short));
		    Validate(dictionary["float1"], 7.77f, typeof(float));
		    Validate(dictionary["double1"], 8.88d, typeof(double));
		    Validate(dictionary["byte1"], (byte)9, typeof(byte));

            Validate(dictionary["sbyte1"], (sbyte)1, typeof(sbyte));
            Validate(dictionary["ushort1"], (ushort)2, typeof(ushort));
            Validate(dictionary["int16a"], (short)3, typeof(Int16));
            Validate(dictionary["uint16a"], (ushort)4, typeof(UInt16));
            Validate(dictionary["uinta"], 5u, typeof(uint));
            Validate(dictionary["int32a"], 6, typeof(Int32));
            Validate(dictionary["uint32a"], 7u, typeof(UInt32));
            Validate(dictionary["ulonga"], 8ul, typeof(ulong));
            Validate(dictionary["int64a"], 9L, typeof(Int64));
            Validate(dictionary["uint64a"], 10ul, typeof(UInt64));
            Validate(dictionary["singlea"], 11f, typeof(Single));

            Validate(dictionary["char1"], 'c', typeof(char));
		    Validate(dictionary["char2"], 'd', typeof(char));
            Validate(dictionary["boolean1"], true, typeof(bool));
            Validate(dictionary["boolean2"], false, typeof(bool));
            Validate(dictionary["timestamp1"], ToDateTime("2013-01-02 12:13:14"), typeof(DateTime));
            Validate(dictionary["timestamp2"], ToDateTime("2013-01-03 12:13:15"), typeof(DateTime));
            Validate(dictionary["date1"], ToDateTime("2012-12-31 00:00:00"), typeof(DateTime));

            // .NET の場合、時間に関しては本日の時間となるが、変換仕様としては「日付部分は不定」とする。
		    Validate(dictionary["time1"], ToDateTime(DateTime.Today.ToString("yyyy-MM-dd") + " 10:10:11"), typeof(DateTime));
        }

        private object ToDateTime(string value)
        {
            return Convert.ToDateTime(value);
        }

        private void Validate(object actual, object expected , Type type)
        {
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(type, actual.GetType());
        }

        [TestMethod]
        public void 親子構造になったデータを取得できる()
        {
            // when
            Dictionary<string, object> order = fixtureBook.GetObject<Dictionary<string, object>>("Order");

            // then
            Assert.AreEqual("H001", order["orderNo"]); 

            Dictionary<string, object> customerInfo = (Dictionary<string, object>)order["customerInfo"];
            Assert.AreEqual("C001", customerInfo["code"]);
            Assert.AreEqual("XX商事", customerInfo["name"]);
            Assert.AreEqual("045-999-9999", customerInfo["telno"]);

            List<Dictionary<string, object>> detail = (List<Dictionary<string, object>>)order["detail"];
            Assert.AreEqual(2, detail.Count);
            Assert.AreEqual("001", detail[0]["detailNo"]);
            Assert.AreEqual("X01", detail[0]["itemCd"]);
            Assert.AreEqual(10, detail[0]["qty"]);
            Assert.AreEqual("002", detail[1]["detailNo"]);
            Assert.AreEqual("X02", detail[1]["itemCd"]);
            Assert.AreEqual(20, detail[1]["qty"]);
        }

        [TestMethod]
        public void 親子構造になったデータを取得できる2()
        {
            // when
            Dictionary<string, object> order = fixtureBook.GetObject<Dictionary<string, object>>("Order");

            // then
            Assert.AreEqual("H001", order["orderNo"]);

            Customer customerInfo = (Customer)order["customerInfo"];
            Assert.AreEqual("C001", customerInfo.Code);

            List<OrderDetail> detail = (List<OrderDetail>)order["detail"];
            Assert.AreEqual(2, detail.Count);
            Assert.AreEqual("001", detail[0].DetailNo);
            Assert.AreEqual("002", detail[1].DetailNo);
        }

        [TestMethod]
        public void バーティカルバー区切りの値で配列およびリストを表現できる()
        {
            // when
            Dictionary<string, object> arrayData = fixtureBook.GetObject<Dictionary<string, object>>("ArrayData");

            // then
            string[] stringArray = (string[])arrayData["stringArray"];
            Assert.AreEqual(3, stringArray.Length);
            Assert.AreEqual("a", stringArray[0]);
            Assert.AreEqual("b", stringArray[1]);
            Assert.AreEqual("c", stringArray[2]);

            int[] intArray = (int[])arrayData["intArray"];
            Assert.AreEqual(3, intArray.Length);
            Assert.AreEqual(1, intArray[0]);
            Assert.AreEqual(2, intArray[1]);
            Assert.AreEqual(3, intArray[2]);

            List<string> stringList1 = (List<string>)arrayData["stringList1"];
            Assert.AreEqual(3, stringList1.Count);
            Assert.AreEqual("e", stringList1[0]);
            Assert.AreEqual("f", stringList1[1]);
            Assert.AreEqual("g", stringList1[2]);

            List<object> stringList2 = (List<object>)arrayData["stringList2"];
            Assert.AreEqual(3, stringList2.Count);
            Assert.AreEqual("h", stringList2[0]);
            Assert.AreEqual("i", stringList2[1]);
            Assert.AreEqual("j", stringList2[2]);
        }

        private DictionaryFactory GetDictionaryFactory(Case testCase)
        {
            TempObjectFactory parent = new TempObjectFactory();
            parent.Initialize(testCase);
            return parent.dictionaryFactory;
        }
    }
}
