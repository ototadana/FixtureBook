using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture;
using System.Data;
using XPFriend.Fixture.Cast.Temp;
using System.Collections.Generic;
using XPFriend.Junk;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class TempConductorTest
    {
        [TestMethod]
        public void Expect__DataSetを引数にした場合Dパラメタにあるすべてのテーブル定義を取得する()
        {
            FixtureBook.Expect<DataTable, DataSet>((dataTable, dataSet) => 
            {
                Assert.AreEqual(2, dataSet.Tables.Count);
                Assert.AreEqual("abc", dataSet.Tables[0].Rows[0]["text"]);
                Assert.AreEqual("abc", dataSet.Tables["Data1"].Rows[0]["text"]);
                Assert.AreEqual("def", dataSet.Tables[1].Rows[0]["text"]);
                Assert.AreEqual("def", dataSet.Tables["Data2"].Rows[0]["text"]);

                Assert.AreEqual("abc", dataTable.Rows[0]["text"]);
            });
        }

        [TestMethod]
        [Fixture("Expect", "引数なしの場合Setupとテスト対象メソッド呼び出しとValidateStorageができる")]
        public void Expectは引数なしの場合Setupとテスト対象メソッド呼び出しとValidateStorageができる_エラー()
        {
            try
            {
                // when
                FixtureBook.Expect(() => 
                { 
                    ValidateDatabase(111);
                    UpdateDatabase(112);
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                //then
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("<111>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<112>") > -1);
            }
        }

        [TestMethod]
        [Fixture("Expect", "引数なしの場合Setupとテスト対象メソッド呼び出しとValidateStorageができる")]
        public void Expectは引数なしの場合Setupとテスト対象メソッド呼び出しとValidateStorageができる_正常終了()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.Expect(() =>
            {
                ValidateDatabase(111);
                called = true;
            });

            // then
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void Expect__引数なしの場合は全くテーブル定義がなくてもなんとなく動作する()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.Expect(() => called = true);

            // then
            Assert.IsTrue(called);
        }

        [TestMethod]
        [Fixture("Expect", "配列やリストデータも取得できる")]
        public void Expect__リストデータも取得できる()
        {
            // setup
            List<FixtureBookAttributeTestData> data = null;

            // when
            FixtureBook.Expect((List<FixtureBookAttributeTestData> p1) => data = p1);

            // then
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual("abc", data[0].Text);
            Assert.AreEqual("efg", data[1].Text);
        }

        [TestMethod]
        [Fixture("Expect", "配列やリストデータも取得できる")]
        public void Expect__配列も取得できる()
        {
            // setup
            FixtureBookAttributeTestData[] data = null;

            // when
            FixtureBook.Expect((FixtureBookAttributeTestData[] p1) => data = p1);

            // then
            Assert.AreEqual(2, data.Length);
            Assert.AreEqual("abc", data[0].Text);
            Assert.AreEqual("efg", data[1].Text);
        }

        private void ValidateDatabase(int expect) 
        {
            using (Database database = new Database())
            {
                DataTable table = database.ExecuteQuery("select int1 from TypesTable");
                Assert.AreEqual(1, table.Rows.Count);
                Assert.AreEqual(expect, table.Rows[0]["int1"]);
            }
        }

        private void UpdateDatabase(int expect)
        {
            using (Database database = new Database())
            {
                database.ExecuteNonQuery("update TypesTable set int1 = " + expect);
                database.Commit();
            }
        }

        [TestMethod]
        [Fixture("Expect", "引数1つの場合にGetObjectとテスト対象メソッド呼び出しとValidateStorageができる")]
        public void Expect__引数1つの場合にGetObjectとテスト対象メソッド呼び出しとValidateStorageができる_正常終了()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.Expect((FixtureBookTestData data) => 
            {
                Assert.AreEqual("abc", data.Text);
                ValidateDatabase(112);
                called = true;
            });

            //then
            Assert.IsTrue(called);
        }

        [TestMethod]
        [Fixture("Expect", "引数1つの場合にGetObjectとテスト対象メソッド呼び出しとValidateStorageができる")]
        public void Expect__引数1つの場合にGetObjectとテスト対象メソッド呼び出しとValidateStorageができる_ValidateStorageエラー()
        {
            try
            {
                // when
                FixtureBook.Expect((FixtureBookTestData data) => UpdateDatabase(113));
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                //then
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("<112>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<113>") > -1);
            }
        }

        [TestMethod]
        public void Expect__引数がある場合はDパラメタの定義だけで正常動作する()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.Expect((FixtureBookTestData data) => 
            {
                Assert.AreEqual("abc", data.Text);
                called = true;
            });

            // then
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void Expect__引数があるのにDパラメタが定義されていないとエラーになる()
        {
            try
            {
                FixtureBook.Expect((FixtureBookTestData data) => { });
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ResourceKey);
                Assert.AreEqual("M_Fixture_FixtureBook_GetSection_ObjectForExec", e.ResourceKey);
            }
        }

        [TestMethod]
        public void Expect__引数があるのにDパラメタにテーブル定義がないとエラーになる()
        {
            try
            {
                FixtureBook.Expect((FixtureBookTestData data) => { });
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ResourceKey);
                Assert.AreEqual("M_Fixture_FixtureBook_GetTable_ObjectForExec", e.ResourceKey);
            }
        }


        [TestMethod]
        [Fixture("ExpectReturn", "パラメタ引数なしの場合Setupとテスト対象メソッド呼び出しとValidateとValidateStorageができる")]
        public void ExpectReturn__パラメタ引数なしの場合Setupとテスト対象メソッド呼び出しとValidateとValidateStorageができる_正常終了()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.ExpectReturn(() => 
            {
                ValidateDatabase(121);
                called = true;
                return new FixtureBookTestData { Text = "abc" };
            });

            //then
            Assert.IsTrue(called);
        }

        [TestMethod]
        [Fixture("ExpectReturn", "パラメタ引数なしの場合Setupとテスト対象メソッド呼び出しとValidateとValidateStorageができる")]
        public void ExpectReturn__パラメタ引数なしの場合Setupとテスト対象メソッド呼び出しとValidateとValidateStorageができる_Validateエラー()
        {
            try
            {
                // when
                FixtureBook.ExpectReturn(() => new FixtureBookTestData { Text = "ABC" });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                //then
                Console.WriteLine(e.Message);
                    Assert.IsTrue(e.Message.IndexOf("<abc>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<ABC>") > -1);
            }
        }


        [TestMethod]
        [Fixture("ExpectReturn", "パラメタ引数なしの場合Setupとテスト対象メソッド呼び出しとValidateとValidateStorageができる")]
        public void ExpectReturn__パラメタ引数なしの場合Setupとテスト対象メソッド呼び出しとValidateとValidateStorageができる_ValidateStorageエラー()
        {
            try
            {
                // when
                FixtureBook.ExpectReturn(() => 
                { 
                    UpdateDatabase(122);
                    return new FixtureBookTestData { Text = "abc" };
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                //then
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("<121>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<122>") > -1);
            }
        }

        [TestMethod]
        public void ExpectReturn__パラメタ引数なしの場合E取得データだけで正常動作する()
        {
            FixtureBook.ExpectReturn(() => new FixtureBookAttributeTestData { Text = "abc" });
        }


        [TestMethod]
        [Fixture("ExpectReturn", "パラメタ引数1つの場合はパラメタ取得と戻り値の検証ができる")]
        public void ExpectReturn__パラメタ引数1つの場合はパラメタ取得と戻り値の検証ができる_正常終了()
        {
            FixtureBook.ExpectReturn((FixtureBookAttributeTestData param) =>
            {
                Assert.AreEqual("abc", param.Text);
                return new FixtureBookAttributeTestData { Text = "zzz" };
            });
        }

        [TestMethod]
        [Fixture("ExpectReturn", "パラメタ引数1つの場合はパラメタ取得と戻り値の検証ができる")]
        public void ExpectReturn__パラメタ引数1つの場合はパラメタ取得と戻り値の検証ができる_エラー()
        {
            try
            {
                // when
                FixtureBook.ExpectReturn((FixtureBookAttributeTestData param) =>
                {
                    return new FixtureBookAttributeTestData { Text = "yyy" };
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                //then
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("<yyy>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
            }
        }

        [TestMethod]
        [Fixture("ExpectThrown", "Setupとテスト対象メソッド呼び出しと例外のValidateとValidateStorageができる")]
        public void ExpectThrown__Setupとテスト対象メソッド呼び出しと例外のValidateとValidateStorageができる_正常終了()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.ExpectThrown<Exception>(() => 
            {
                ValidateDatabase(131);
                called = true;
                throw new Exception("ABC"); 
            });

            //then
            Assert.IsTrue(called);
        }

        [TestMethod]
        [Fixture("ExpectThrown", "Setupとテスト対象メソッド呼び出しと例外のValidateとValidateStorageができる")]
        public void ExpectThrown__Setupとテスト対象メソッド呼び出しと例外のValidateとValidateStorageができる_Validateエラー()
        {
            // when
            try
            {
                FixtureBook.ExpectThrown<Exception>(() => { throw new Exception("aBC"); });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                //then
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("<ABC>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<aBC>") > -1);
            }
        }

        [TestMethod]
        [Fixture("ExpectThrown", "Setupとテスト対象メソッド呼び出しと例外のValidateとValidateStorageができる")]
        public void ExpectThrown__Setupとテスト対象メソッド呼び出しと例外のValidateとValidateStorageができる_ValidateStorageエラー()
        {
            // when
            try
            {
                FixtureBook.ExpectThrown<Exception>(() => 
                {
                    UpdateDatabase(132);
                    throw new Exception("ABC"); 
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                //then
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("<131>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<132>") > -1);
            }
        }

        [TestMethod]
        [Fixture("ExpectReturn", "DataSetを戻り値にした場合E取得データの全テーブル定義でチェックする")]
        public void ExpectReturn__DataSetを戻り値にした場合E取得データの全テーブル定義でチェックする_正常終了()
        {
            FixtureBook.ExpectReturn(() => 
            {
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(new DataTable("Data1"));
                dataSet.Tables.Add(new DataTable("Data2"));

                dataSet.Tables[0].Columns.Add("text");
                dataSet.Tables[1].Columns.Add("text");

                dataSet.Tables[0].Rows.Add("abc");
                dataSet.Tables[1].Rows.Add("def");
                return dataSet;
            });
        }

        [TestMethod]
        [Fixture("ExpectReturn", "DataSetを戻り値にした場合E取得データの全テーブル定義でチェックする")]
        public void ExpectReturn__DataSetを戻り値にした場合E取得データの全テーブル定義でチェックする_エラー1()
        {
            try
            {
                FixtureBook.ExpectReturn(() =>
                {
                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(new DataTable("Data1"));
                    dataSet.Tables.Add(new DataTable("Data2"));

                    dataSet.Tables[0].Columns.Add("text");
                    dataSet.Tables[1].Columns.Add("text");

                    dataSet.Tables[0].Rows.Add("ABC");
                    dataSet.Tables[1].Rows.Add("def");
                    return dataSet;
                });
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<ABC>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<abc>") > -1);
            }
        }

        [TestMethod]
        [Fixture("ExpectReturn", "DataSetを戻り値にした場合E取得データの全テーブル定義でチェックする")]
        public void ExpectReturn__DataSetを戻り値にした場合E取得データの全テーブル定義でチェックする_エラー2()
        {
            try
            {
                FixtureBook.ExpectReturn(() =>
                {
                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(new DataTable("Data1"));
                    dataSet.Tables.Add(new DataTable("Data2"));

                    dataSet.Tables[0].Columns.Add("text");
                    dataSet.Tables[1].Columns.Add("text");

                    dataSet.Tables[0].Rows.Add("abc");
                    dataSet.Tables[1].Rows.Add("def");
                    return dataSet;
                });
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<DEF>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<def>") > -1);
            }
        }

        [TestMethod]
        public void ValidateParameter__Expect系メソッドを呼ぶ前にGetParamterAtメソッドを呼ぶと例外が発生する()
        {
            // expect
            new FixtureBook().Validate<ConfigException>(() => new FixtureBook().GetParameterAt<string>(0));
        }

        [TestMethod]
        public void ValidateParameter__Expect系メソッドを呼ぶ前にValidateParamterAtメソッドを呼ぶと例外が発生する()
        {
            // expect
            new FixtureBook().Validate<ConfigException>(() => new FixtureBook().ValidateParameterAt(0));
            new FixtureBook().Validate<ConfigException>(() => new FixtureBook().ValidateParameterAt(0, "xxx"));
        }

        [TestMethod]
        public void ValidateParameter__GetParamterAtメソッドのインデックスがExpectの引数の数よりも多い場合は例外が発生する()
        {
            // expect
            new FixtureBook().Validate<ConfigException>(() => FixtureBook.Expect(() => { }).GetParameterAt<string>(0));
        }

        [TestMethod]
        public void ValidateParameter__ValidateParamterAtメソッドのインデックスがExpectの引数の数よりも多い場合は例外が発生する()
        {
            // expect
            new FixtureBook().Validate<ConfigException>(() => 
                FixtureBook.Expect((FixtureBookAttributeTestData a, FixtureBookAttributeTestData b) => { }
                ).ValidateParameterAt(2));
            new FixtureBook().Validate<ConfigException>(() => 
                FixtureBook.Expect((FixtureBookAttributeTestData a) => { }
                ).ValidateParameterAt(2, "xxx"));
        }
    }
}
