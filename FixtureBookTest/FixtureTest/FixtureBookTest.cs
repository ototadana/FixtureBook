using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture;
using XPFriend.Junk;
using XPFriend.Fixture.Cast.Temp;
using System.Collections.Generic;
using System.Data;

namespace XPFriend.FixtureTest
{
    public class FixtureBookTestData
    {
        public string Text { get; set; }
    }

    [TestClass]
    public class FixtureBookTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            Loggi.DebugEnabled = false;
        }

        [TestMethod]
        public void FixtureBook属性指定をしなくても参照可能なこと()
        {
            // when
            FixtureBookTestData obj = new FixtureBook().GetObject<FixtureBookTestData>();

            // then
            Assert.AreEqual("abc", obj.Text);
        }

        [TestMethod]
        public void Sheet1__アンダーバー区切りで参照できること()
        {
            // when
            FixtureBookTestData obj = new FixtureBook().GetObject<FixtureBookTestData>();

            // then
            Assert.AreEqual("efg", obj.Text);
        }

        [TestMethod]
        [Fixture("Sheet1", "Fixture属性で参照できること")]
        public void Test_Fixture属性で参照できること()
        {
            // when
            FixtureBookTestData obj = new FixtureBook().GetObject<FixtureBookTestData>();

            // then
            Assert.AreEqual("hij", obj.Text);

            // expect
            Sheet1__複数のメソッドから呼び出しができること();
        }

        private void Sheet1__複数のメソッドから呼び出しができること()
        {
            // when
            FixtureBookTestData obj = new FixtureBook().GetObject<FixtureBookTestData>();

            // then
            Assert.AreEqual("xyz", obj.Text);
        }

        [TestInitialize]
        public void Sheet2__TestInitializeからの呼び出しができること()
        {
            // when
            FixtureBookTestData obj = new FixtureBook().GetObject<FixtureBookTestData>();

            // then
            Assert.AreEqual("klm", obj.Text);
        }

        [ClassInitialize]
        public static void Sheet2__ClassInitializeからの呼び出しができること(TestContext context)
        {
            // when
            FixtureBookTestData obj = new FixtureBook().GetObject<FixtureBookTestData>();

            // then
            Assert.AreEqual("nop", obj.Text);
        }

        [TestMethod]
        public void ValidateメソッドはSetupメソッドの暗黙呼び出しを行わないこと()
        {
            // setup
            Loggi.DebugEnabled = true;
            Database.ExecuteNonQuery(null, "delete from TypesTable");
            FixtureBook fixtureBook = new FixtureBook();

            // when
            Dictionary<string, string> obj = new Dictionary<string, string>();
            obj["Id"] = "1";
            fixtureBook.Validate(obj);

            // then
            Assert.AreEqual(0, Database.ExecuteQuery(null, "select * from TypesTable").Rows.Count);

            // when
            fixtureBook.Setup();

            // then
            Assert.AreEqual(1, Database.ExecuteQuery(null, "select * from TypesTable").Rows.Count);
        }

        [TestMethod]
        public void ValidateStorageの呼び出し時にはSetupメソッドの暗黙呼び出しはされないこと()
        {
            // setup
            Loggi.DebugEnabled = true;
            Database.ExecuteNonQuery(null, "delete from TypesTable");
            FixtureBook fixtureBook = new FixtureBook();

            // when
            try
            {
                fixtureBook.ValidateStorage();
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("*") == -1);
            }
        }

        [TestMethod]
        public void Validateで例外発生がテストできる() 
        {
            // expect : normal
            new FixtureBook().Validate<Exception>(() => { throw new Exception("zzz"); });
            new FixtureBook().Validate<Exception>(() => { throw new Exception("xxx"); }, "Result");

            // expect : error
            try
            {
                new FixtureBook().Validate<Exception>(() => { throw new Exception("ZZZ"); });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<ZZZ>") > -1);
            }
            try
            {
                new FixtureBook().Validate<Exception>(() => { throw new Exception("XXX"); }, "Result");
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Assert.IsTrue(e.Message.IndexOf("<xxx>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<XXX>") > -1);
            }
        }

        [TestMethod]
        [Fixture("Expect", "ExpectおよびExpectResultのテスト")]
        public void Expectは指定されたラムダ式の実行ができる()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.Expect(() => called = true);

            // then
            Assert.IsTrue(called);
        }

        [TestMethod]
        [Fixture("Expect", "ExpectおよびExpectResultのテスト")]
        public void Expect1はDパラメタに定義されたオブジェクトを1つの引数として取得ができる()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.Expect((FixtureBookTestData p1) => 
            { 
                called = true;
                Assert.AreEqual("abc", p1.Text);
            });

            // then
            Assert.IsTrue(called);
        }

        [TestMethod]
        [Fixture("Expect", "ExpectおよびExpectResultのテスト")]
        public void Expect2はDパラメタに定義されたオブジェクトを2つの引数として取得ができる()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.Expect((FixtureBookTestData p1, FixtureBookTestData p2) =>
            {
                called = true;
                Assert.AreEqual("abc", p1.Text);
                Assert.AreEqual("def", p2.Text);
            });

            // then
            Assert.IsTrue(called);
        }

        [TestMethod]
        [Fixture("Expect", "ExpectおよびExpectResultのテスト")]
        public void Expect3はDパラメタに定義されたオブジェクトを3つの引数として取得ができる()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.Expect((FixtureBookTestData p1, FixtureBookTestData p2, FixtureBookTestData p3) =>
            {
                called = true;
                Assert.AreEqual("abc", p1.Text);
                Assert.AreEqual("def", p2.Text);
                Assert.AreEqual("ghi", p3.Text);
            });

            // then
            Assert.IsTrue(called);
        }

        [TestMethod]
        [Fixture("Expect", "ExpectおよびExpectResultのテスト")]
        public void Expect4はDパラメタに定義されたオブジェクトを4つの引数として取得ができる()
        {
            // setup
            bool called = false;

            // when
            FixtureBook.Expect((FixtureBookTestData p1, FixtureBookTestData p2, 
                FixtureBookTestData p3, FixtureBookTestData p4) =>
            {
                called = true;
                Assert.AreEqual("abc", p1.Text);
                Assert.AreEqual("def", p2.Text);
                Assert.AreEqual("ghi", p3.Text);
                Assert.AreEqual("jkl", p4.Text);
            });

            // then
            Assert.IsTrue(called);
        }

        [TestMethod]
        [Fixture("Expect", "ExpectおよびExpectResultのテスト")]
        public void ExpectReturnは指定されたラムダ式の戻り値が検証できる()
        {
            // expect : normal
            FixtureBook.ExpectReturn(() => new FixtureBookTestData { Text = "zzz" });

            // expect : error
            try
            {
                FixtureBook.ExpectReturn(() => new FixtureBookTestData { Text = "zzp" });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzp>") > -1);
            }
        }

        [TestMethod]
        [Fixture("Expect", "ExpectおよびExpectResultのテスト")]
        public void ExpectReturn1はDパラメタに定義されたオブジェクトを1つの引数として取得ができ戻り値検証ができる()
        {
            // expect : normal
            FixtureBook.ExpectReturn((FixtureBookTestData p1) => 
            { 
                Assert.AreEqual("abc", p1.Text);
                return new FixtureBookTestData { Text = "zzz" };
            });
            
            // expect : error
            try
            {
                FixtureBook.ExpectReturn((FixtureBookTestData p1) => 
                {
                    return new FixtureBookTestData { Text = "zzq" };
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzq>") > -1);
            }
        }

        [TestMethod]
        [Fixture("Expect", "ExpectおよびExpectResultのテスト")]
        public void ExpectReturn2はDパラメタに定義されたオブジェクトを2つの引数として取得ができ戻り値検証ができる()
        {
            // expect : normal
            FixtureBook.ExpectReturn((FixtureBookTestData p1, FixtureBookTestData p2) =>
            {
                Assert.AreEqual("abc", p1.Text);
                Assert.AreEqual("def", p2.Text);
                return new FixtureBookTestData { Text = "zzz" };
            });

            // expect : error
            try
            {
                FixtureBook.ExpectReturn((FixtureBookTestData p1, FixtureBookTestData p2) =>
                {
                    return new FixtureBookTestData { Text = "zzr" };
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzr>") > -1);
            }
        }

        [TestMethod]
        [Fixture("Expect", "ExpectおよびExpectResultのテスト")]
        public void ExpectReturn3はDパラメタに定義されたオブジェクトを3つの引数として取得ができ戻り値検証ができる()
        {
            // expect : normal
            FixtureBook.ExpectReturn((FixtureBookTestData p1, FixtureBookTestData p2, FixtureBookTestData p3) =>
            {
                Assert.AreEqual("abc", p1.Text);
                Assert.AreEqual("def", p2.Text);
                Assert.AreEqual("ghi", p3.Text);
                return new FixtureBookTestData { Text = "zzz" };
            });

            // expect : error
            try
            {
                FixtureBook.ExpectReturn((FixtureBookTestData p1, FixtureBookTestData p2, FixtureBookTestData p3) =>
                {
                    return new FixtureBookTestData { Text = "zzs" };
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzs>") > -1);
            }
        }

        [TestMethod]
        [Fixture("Expect", "ExpectおよびExpectResultのテスト")]
        public void ExpectReturn4はDパラメタに定義されたオブジェクトを4つの引数として取得ができ戻り値検証ができる()
        {
            // expect : normal
            FixtureBook.ExpectReturn((FixtureBookTestData p1, FixtureBookTestData p2,
                FixtureBookTestData p3, FixtureBookTestData p4) =>
            {
                Assert.AreEqual("abc", p1.Text);
                Assert.AreEqual("def", p2.Text);
                Assert.AreEqual("ghi", p3.Text);
                Assert.AreEqual("jkl", p4.Text);
                return new FixtureBookTestData { Text = "zzz" };
            });

            // expect : error
            try
            {
                FixtureBook.ExpectReturn((FixtureBookTestData p1, FixtureBookTestData p2,
                    FixtureBookTestData p3, FixtureBookTestData p4) =>
                {
                    return new FixtureBookTestData { Text = "zzt" };
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzt>") > -1);
            }
        }

        [TestMethod]
        [Fixture("ExpectThrown", "ExpectThrownのテスト")]
        public void ExpectThrownは指定されたラムダ式の例外が検証できる()
        {
            // expect : normal
            FixtureBook.ExpectThrown<Exception>(() => { throw new Exception("zzz"); });

            // expect : error
            try
            {
                FixtureBook.ExpectThrown<Exception>(() => { throw new Exception("zzp"); });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzp>") > -1);
            }
        }

        [TestMethod]
        [Fixture("ExpectThrown", "ExpectThrownのテスト")]
        public void ExpectThrown1はDパラメタに定義されたオブジェクトを1つの引数として取得ができ例外検証ができる()
        {
            // expect : normal
            FixtureBook.ExpectThrown<FixtureBookTestData, Exception>(p1 => 
            { 
                Assert.AreEqual("abc", p1.Text);
                throw new Exception("zzz");
            });
            
            // expect : error
            try
            {
                FixtureBook.ExpectThrown<FixtureBookTestData, Exception>(p1 => 
                {
                    throw new Exception("zzq");
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzq>") > -1);
            }
        }

        [TestMethod]
        [Fixture("ExpectThrown", "ExpectThrownのテスト")]
        public void ExpectThrown2はDパラメタに定義されたオブジェクトを2つの引数として取得ができ例外検証ができる()
        {
            // expect : normal
            FixtureBook.ExpectThrown<FixtureBookTestData, FixtureBookTestData, Exception>((p1, p2) =>
            {
                Assert.AreEqual("abc", p1.Text);
                Assert.AreEqual("def", p2.Text);
                throw new Exception("zzz");
            });

            // expect : error
            try
            {
                FixtureBook.ExpectThrown<FixtureBookTestData, FixtureBookTestData, Exception>((p1, p2) =>
                {
                    throw new Exception("zzr");
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzr>") > -1);
            }
        }

        [TestMethod]
        [Fixture("ExpectThrown", "ExpectThrownのテスト")]
        public void ExpectThrown3はDパラメタに定義されたオブジェクトを3つの引数として取得ができ例外検証ができる()
        {
            // expect : normal
            FixtureBook.ExpectThrown<FixtureBookTestData, FixtureBookTestData, FixtureBookTestData, Exception>((p1, p2, p3) =>
            {
                Assert.AreEqual("abc", p1.Text);
                Assert.AreEqual("def", p2.Text);
                Assert.AreEqual("ghi", p3.Text);
                throw new Exception("zzz");
            });

            // expect : error
            try
            {
                FixtureBook.ExpectThrown<FixtureBookTestData, FixtureBookTestData, FixtureBookTestData, Exception>((p1, p2, p3) =>
                {
                    throw new Exception("zzs");
                });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzs>") > -1);
            }
        }

        [TestMethod]
        [Fixture("ExpectThrown", "ExpectThrownのテスト")]
        public void ExpectThrown4はDパラメタに定義されたオブジェクトを4つの引数として取得ができ例外検証ができる()
        {
            // expect : normal
            FixtureBook.ExpectThrown<FixtureBookTestData, FixtureBookTestData, FixtureBookTestData,
                FixtureBookTestData, Exception>((p1, p2, p3, p4) =>
                {
                    Assert.AreEqual("abc", p1.Text);
                    Assert.AreEqual("def", p2.Text);
                    Assert.AreEqual("ghi", p3.Text);
                    Assert.AreEqual("jkl", p4.Text);
                    throw new Exception("zzz");
                });

            // expect : error
            try
            {
                FixtureBook.ExpectThrown<FixtureBookTestData, FixtureBookTestData, FixtureBookTestData,
                    FixtureBookTestData, Exception>((p1, p2, p3, p4) =>
                    {
                        throw new Exception("zzt");
                    });
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
                Assert.IsTrue(e.Message.IndexOf("<zzz>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<zzt>") > -1);
            }
        }

        [TestMethod]
        [Fixture("ValidateParameter", "GetParameterAtおよびValidateParameterAtのテスト")]
        public void GetParameterAtは指定したインデックスの引数を取得する()
        {
            // setup
            FixtureBookTestData[] data = new FixtureBookTestData[4];
            FixtureBook fixtureBook = FixtureBook.Expect((FixtureBookTestData p1, FixtureBookTestData p2,
                FixtureBookTestData p3, FixtureBookTestData p4) => 
            {
                data[0] = p1;
                data[1] = p2;
                data[2] = p3;
                data[3] = p4;
            });

            // expect
            Assert.AreSame(data[0], fixtureBook.GetParameterAt<FixtureBookTestData>(0));
            Assert.AreSame(data[1], fixtureBook.GetParameterAt<FixtureBookTestData>(1));
            Assert.AreSame(data[2], fixtureBook.GetParameterAt<FixtureBookTestData>(2));
            Assert.AreSame(data[3], fixtureBook.GetParameterAt<FixtureBookTestData>(3));
            Assert.AreEqual("abc", fixtureBook.GetParameterAt<FixtureBookTestData>(0).Text);
            Assert.AreEqual("def", fixtureBook.GetParameterAt<FixtureBookTestData>(1).Text);
            Assert.AreEqual("ghi", fixtureBook.GetParameterAt<FixtureBookTestData>(2).Text);
            Assert.AreEqual("jkl", fixtureBook.GetParameterAt<FixtureBookTestData>(3).Text);
        }

        [TestMethod]
        [Fixture("ValidateParameter", "GetParameterAtおよびValidateParameterAtのテスト")]
        public void ValidateParameterAtは指定したインデックスの引数を検証する() 
        {
            // setup
            FixtureBook fixtureBook = FixtureBook.Expect((FixtureBookTestData p1, FixtureBookTestData p2,
                FixtureBookTestData p3, FixtureBookTestData p4) => {});

            // expect : normal
            fixtureBook.ValidateParameterAt(0);
            fixtureBook.ValidateParameterAt(1, 2, 3);
            

            fixtureBook.ValidateParameterAt(0, "Parameter1");
            fixtureBook.ValidateParameterAt(1, "Parameter2").
                ValidateParameterAt(2, "Parameter3").
                ValidateParameterAt(3, "Parameter4");

            // expect : error
            try
            {
                fixtureBook.ValidateParameterAt(0, "Parameter2");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("<abc>") > -1);
                Assert.IsTrue(e.Message.IndexOf("<def>") > -1);
            }

            // expect : error
            try
            {
                fixtureBook.ValidateParameterAt(0, "xxx");
            }
            catch (ConfigException e)
            {
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("xxx") > -1);
            }
        }
    }
}