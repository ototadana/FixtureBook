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
using System.Data;
using XPFriend.Fixture;
using XPFriend.FixtureTest.Cast.Temp.Datas;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class EmptyValidateTest
    {
        private FixtureBook fixtureBook = new FixtureBook();

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void ExpectReturnで要素数ゼロのListを検証できる_正常()
        {
            FixtureBook.ExpectReturn((List<Data> list) => { 
                Assert.AreEqual(0, list.Count);
                return list;
            });
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void ExpectReturnで要素数ゼロのListを検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn((List<Data> list) => {
                    list.Add(new Data());
                    return list;
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void 要素数ゼロのListを検証できる_正常()
        {
            // setup
            List<Data> list = fixtureBook.GetList<Data>();

            // expect
            Assert.AreEqual(0, list.Count);
            fixtureBook.Validate(list);
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void 要素数ゼロのListを検証できる_エラー()
        {
            // setup
            List<Data> list = fixtureBook.GetList<Data>();
            list.Add(new Data());

            // expect
            ValidateException(list);
        }

        private void ValidateException(object obj)
        {
            try
            {
                fixtureBook.Validate(obj);
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void ExpectReturnで要素数ゼロの配列を検証できる_正常()
        {
            FixtureBook.ExpectReturn((Data[] array) => {
                Assert.AreEqual(0, array.Length);
                return array;
            });
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void ExpectReturnで要素数ゼロの配列を検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { return new Data[]{new Data()}; });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void 要素数ゼロの配列を検証できる_正常()
        {
            // setup
            Data[] array = fixtureBook.GetArray<Data>();

            // expect
            Assert.AreEqual(0, array.Length);
            fixtureBook.Validate(array);
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void 要素数ゼロの配列を検証できる_エラー()
        {
            // setup
            Data[] array = new Data[]{new Data()};

            // expect
            ValidateException(array);
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void ExpectReturnで要素数ゼロのDataTableを検証できる_正常()
        {
            FixtureBook.ExpectReturn((DataTable dt) => {
                Assert.AreEqual(0, dt.Rows.Count);
                return dt;
            });
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void ExpectReturnで要素数ゼロのDataTableを検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn((DataTable dt) => {
                    dt.Rows.Add(dt.NewRow());
                    return dt;
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void 要素数ゼロのDataTableを検証できる_正常()
        {
            // setup
            DataTable dt = fixtureBook.GetObject<DataTable>();

            // expect
            Assert.AreEqual(0, dt.Rows.Count);
            fixtureBook.Validate(dt);
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void 要素数ゼロのDataTableを検証できる_エラー()
        {
            // setup
            DataTable dt = fixtureBook.GetObject<DataTable>();
            dt.Rows.Add(dt.NewRow());

            // expect
            ValidateException(dt);
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void ExpectReturnで要素数ゼロのDictionaryListを検証できる_正常()
        {
            FixtureBook.ExpectReturn((List<Dictionary<string, object>> list) =>
            {
                Assert.AreEqual(0, list.Count);
                return list;
            });
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void ExpectReturnで要素数ゼロのDictionaryListを検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn((List<Dictionary<string, object>> list) =>
                {
                    list.Add(new Dictionary<string, object>());
                    return list;
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void 要素数ゼロのDictionaryListを検証できる_正常()
        {
            // setup
            List<Dictionary<string, object>> list = fixtureBook.GetList<Dictionary<string, object>>();

            // expect
            Assert.AreEqual(0, list.Count);
            fixtureBook.Validate(list);
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void 要素数ゼロのDictionaryListを検証できる_エラー()
        {
            // setup
            List<Dictionary<string, object>> list = fixtureBook.GetList<Dictionary<string, object>>();
            list.Add(new Dictionary<string, object>());

            // expect
            ValidateException(list);
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void ExpectReturnで要素数ゼロのDictionary配列を検証できる_正常()
        {
            FixtureBook.ExpectReturn((Dictionary<string, object>[] array) =>
            {
                Assert.AreEqual(0, array.Length);
                return array;
            });
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void ExpectReturnで要素数ゼロのDictionary配列を検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn(() => { 
                    return new Dictionary<string, object>[] { new Dictionary<string, object>() };
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void 要素数ゼロのDictionary配列を検証できる_正常()
        {
            // setup
            Dictionary<string, object>[] array = fixtureBook.GetArray<Dictionary<string, object>>();

            // expect
            Assert.AreEqual(0, array.Length);
            fixtureBook.Validate(array);
        }

        [TestMethod]
        [Fixture("要素数ゼロのコレクションを検証できる")]
        public void 要素数ゼロのDictionary配列を検証できる_エラー()
        {
            // setup
            Dictionary<string, object>[] array = new Dictionary<string, object>[]{new Dictionary<string, object>()};

            // expect
            ValidateException(array);
        }

        [TestMethod]
        [Fixture("TableがゼロのDataSetを検証できる")]
        public void TableがゼロのDataSetを検証できる_正常()
        {
            // setup
            DataSet ds = fixtureBook.GetObject<DataSet>();

            // expect
            Assert.AreEqual(0, ds.Tables.Count);
            fixtureBook.Validate(ds);
        }

        [TestMethod]
        [Fixture("TableがゼロのDataSetを検証できる")]
        public void TableがゼロのDataSetを検証できる_エラー()
        {
            // setup
            DataSet ds = fixtureBook.GetObject<DataSet>();
            ds.Tables.Add(new DataTable());

            // expect
            ValidateException(ds);
        }

        [TestMethod]
        [Fixture("TableがゼロのDataSetを検証できる")]
        public void ExpectReturnでTableがゼロのDataSetを検証できる_正常()
        {
            FixtureBook.ExpectReturn((DataSet ds) => {
                Assert.AreEqual(0, ds.Tables.Count);
                return ds;
            });
        }

        [TestMethod]
        [Fixture("TableがゼロのDataSetを検証できる")]
        public void ExpectReturnでTableがゼロのDataSetを検証できる_エラー()
        {
            try
            {
                FixtureBook.ExpectReturn((DataSet ds) => {
                    ds.Tables.Add(new DataTable());
                    return ds;
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
