using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture;
using XPFriend.Junk;
using XPFriend.Fixture.Cast.Temp;
using System.Collections.Generic;

namespace XPFriend.FixtureTest
{
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
            using (Database database = new Database())
            {
                database.ExecuteNonQuery("delete from TypesTable");
                database.Commit();
            }
            FixtureBook fixtureBook = new FixtureBook();

            // when
            Dictionary<string, string> obj = new Dictionary<string, string>();
            obj["Id"] = "1";
            fixtureBook.Validate(obj);

            // then
            using (Database database = new Database())
            {
                Assert.AreEqual(0, database.ExecuteQuery("select * from TypesTable").Rows.Count);
            }

            // when
            fixtureBook.Setup();

            // then
            using (Database database = new Database())
            {
                Assert.AreEqual(1, database.ExecuteQuery("select * from TypesTable").Rows.Count);
            }
        }

        [TestMethod]
        public void ValidateStorageの呼び出し時にはSetupメソッドの暗黙呼び出しはされないこと()
        {
            // setup
            Loggi.DebugEnabled = true;
            using (Database database = new Database())
            {
                database.ExecuteNonQuery("delete from TypesTable");
                database.Commit();
            }
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
            }
        }
    }

    public class FixtureBookTestData
    {
        public string Text { get; set; }
    }
}