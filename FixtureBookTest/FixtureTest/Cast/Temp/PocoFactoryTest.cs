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
    public class PocoFactoryTest
    {
        private FixtureBook fixtureBook = new FixtureBook();

        [TestMethod]
        public void HasRoleはInitializeで指定されたテストケースにパラメタセクションがなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            PocoFactory factory = GetPocoFactory(sheet.GetCase("ロールなし"));

            // expect
            Assert.IsFalse(factory.HasRole<object>(null));
        }

        [TestMethod]
        public void HasRoleはInitializeで指定されたテストケースにパラメタセクションがあればtrueを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            PocoFactory factory = GetPocoFactory(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsTrue(factory.HasRole<object>(null));
        }

        [TestMethod]
        public void HasRoleは引数のクラスがDictionaryでもtrueを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            PocoFactory factory = GetPocoFactory(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsTrue(factory.HasRole<object>(null));
        }

        [TestMethod]
        [Fixture("PocoFactoryTest", "データあり")]
        public void GetObjectは指定された名前の定義を読み込みオブジェクト作成する()
        {
            GetObjectは指定された名前の定義を読み込みオブジェクト作成する(null, "a", 1);
            GetObjectは指定された名前の定義を読み込みオブジェクト作成する("Data1", "c", 3);
        }

        private void GetObjectは指定された名前の定義を読み込みオブジェクト作成する(string name, string text1, int number1)
        {
            // when
            Data result = fixtureBook.GetObject<Data>(name);

            // then
            Assert.AreEqual(text1, result.Text1);
            Assert.AreEqual(number1, result.Number1);
        }

        [TestMethod]
        [Fixture("PocoFactoryTest", "データなし")]
        public void GetObjectはデータ行がない場合nullを返す()
        {
            // when
            Data result = fixtureBook.GetObject<Data>();

            // then
            Assert.IsNull(result);
        }

        [TestMethod]
        [Fixture("PocoFactoryTest", "データあり")]
        public void GetListは指定された名前の定義を読み込みリスト作成する()
        {
            GetListは指定された名前の定義を読み込みリスト作成する(null, "a,1", "b,2", ",0");
            GetListは指定された名前の定義を読み込みリスト作成する("Data1", "c,3");
        }
        private void GetListは指定された名前の定義を読み込みリスト作成する(string name, params string[] value)
        {
            // when
            List<Data> list = fixtureBook.GetList<Data>(name);

            // then
            Assert.AreEqual(value.Length, list.Count);
            for (int i = 0; i < value.Length; i++)
            {
                Assert.AreEqual(value[i], list[i].ToString());
            }
        }

        [TestMethod]
        [Fixture("PocoFactoryTest", "データなし")]
        public void GetListはデータ行がない場合に要素数ゼロのリストを返す()
        {
            // when
            List<Data> list = fixtureBook.GetList<Data>();

            // then
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        [Fixture("PocoFactoryTest", "データあり")]
        public void GetArrayは指定された名前の定義を読み込み配列作成する()
        {
            GetArrayは指定された名前の定義を読み込み配列作成する(null, "a,1", "b,2", ",0" );
            GetArrayは指定された名前の定義を読み込み配列作成する("Data1", "c,3" );
        }
        private void GetArrayは指定された名前の定義を読み込み配列作成する(string name, params string[] value)
        {
            // when
            Data[] array = fixtureBook.GetArray<Data>(name);

            // then
            Assert.AreEqual(value.Length, array.Length);
            for (int i = 0; i < value.Length; i++)
            {
                Assert.AreEqual(value[i], array[i].ToString());
            }
        }

        [TestMethod]
        [Fixture("PocoFactoryTest", "データなし")]
        public void GetArrayはデータ行がない場合に要素数ゼロの配列を返す()
        {
            // when
            Data[] array = fixtureBook.GetArray<Data>();

            // then
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        [Fixture("PocoFactoryTest", "親子データ")]
        public void 親子構造になったデータを取得できる()
        {
            // when
            Order order = fixtureBook.GetObject<Order>();

            /*
            Order order = new Order
            {
                OrderNo = "H001",
                CustomerInfo = new Customer { Code = "C001", Name = "XX商事", Telno = "045-999-9999" },
                Detail = new List<OrderDetail> 
                { 
                    new OrderDetail { DetailNo = "001", ItemCd = "X01", Qty = 10 },
                    new OrderDetail { DetailNo = "002", ItemCd = "X02", Qty = 20 },
                }
            };
            */

            // then
            Assert.AreEqual("H001", order.OrderNo);
            Assert.AreEqual("C001", order.CustomerInfo.Code);
            Assert.AreEqual("XX商事", order.CustomerInfo.Name);
            Assert.AreEqual("045-999-9999", order.CustomerInfo.Telno);
            Assert.AreEqual(2, order.Detail.Count);
            Assert.AreEqual("001", order.Detail[0].DetailNo);
            Assert.AreEqual("X01", order.Detail[0].ItemCd);
            Assert.AreEqual(10, order.Detail[0].Qty);
            Assert.AreEqual("002", order.Detail[1].DetailNo);
            Assert.AreEqual("X02", order.Detail[1].ItemCd);
            Assert.AreEqual(20, order.Detail[1].Qty);

        }

        [TestMethod]
        [Fixture("PocoFactoryTest", "配列データ")]
        public void バーティカルバー区切りの値で配列およびリストを表現できる()
        {
            // when
            ArrayData arrayData = fixtureBook.GetObject<ArrayData>();

            // then
            Assert.AreEqual(3, arrayData.StringArray.Length);
            Assert.AreEqual("a", arrayData.StringArray[0]);
            Assert.AreEqual("b", arrayData.StringArray[1]);
            Assert.AreEqual("c", arrayData.StringArray[2]);

            Assert.AreEqual(3, arrayData.IntArray.Length);
            Assert.AreEqual(1, arrayData.IntArray[0]);
            Assert.AreEqual(2, arrayData.IntArray[1]);
            Assert.AreEqual(3, arrayData.IntArray[2]);

            Assert.AreEqual(3, arrayData.StringList.Count);
            Assert.AreEqual("e", arrayData.StringList[0]);
            Assert.AreEqual("f", arrayData.StringList[1]);
            Assert.AreEqual("g", arrayData.StringList[2]);
        }

        private PocoFactory GetPocoFactory(Case testCase)
        {
            TempObjectFactory parent = new TempObjectFactory();
            parent.Initialize(testCase);
            return parent.pocoFactory;
        }

        [TestMethod]
        public void 指定されたプロパティが存在しない場合は例外が発生する()
        {
            try
            {
                // when
                fixtureBook.GetObject<Data>();
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e.Message);
                Assert.AreEqual("M_Fixture_Temp_ObjectFactory_NoSuchProperty", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("zzz") > -1);
            }
        }

        [TestMethod]
        public void stringを作成できる()
        {
            // when
            string obj = fixtureBook.GetObject<String>();
            // then
            Assert.AreEqual("a", obj);

            {
                // when
                List<string> list = fixtureBook.GetList<string>();
                // then
                Assert.AreEqual(2, list.Count);
                Assert.AreEqual("a", list[0]);
                Assert.AreEqual("b", list[1]);
            }

            {
                // when
                string[] array = fixtureBook.GetArray<string>();
                // then
                Assert.AreEqual(2, array.Length);
                Assert.AreEqual("a", array[0]);
                Assert.AreEqual("b", array[1]);
            }

            // expect
            FixtureBook.Expect((string s) => Assert.AreEqual("a", s));
            FixtureBook.Expect((List<string> list) =>
            {
                Assert.AreEqual(2, list.Count);
                Assert.AreEqual("a", list[0]);
                Assert.AreEqual("b", list[1]);
            });
            FixtureBook.Expect((string[] array) =>
            {
                Assert.AreEqual(2, array.Length);
                Assert.AreEqual("a", array[0]);
                Assert.AreEqual("b", array[1]);
            });
        }

        [TestMethod]
        public void intを作成できる()
        {
            {
                // when
                int obj = fixtureBook.GetObject<int>();
                // then
                Assert.AreEqual(1, obj);
            }

            {
                // when
                List<int> list = fixtureBook.GetList<int>();
                // then
                Assert.AreEqual(2, list.Count);
                Assert.AreEqual(1, list[0]);
                Assert.AreEqual(2, list[1]);
            }

            {
                // when
                int[] array = fixtureBook.GetArray<int>();
                // then
                Assert.AreEqual(2, array.Length);
                Assert.AreEqual(1, array[0]);
                Assert.AreEqual(2, array[1]);
            }

            // expect
            FixtureBook.Expect((int obj) => Assert.AreEqual(1, obj));
            FixtureBook.Expect((List<int> list) =>
            {
                Assert.AreEqual(2, list.Count);
                Assert.AreEqual(1, list[0]);
                Assert.AreEqual(2, list[1]);
            });
            FixtureBook.Expect((int[] array) =>
            {
                Assert.AreEqual(2, array.Length);
                Assert.AreEqual(1, array[0]);
                Assert.AreEqual(2, array[1]);
            });
        }
    }
}
