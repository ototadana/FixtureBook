using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using XPFriend.Fixture;
using XPFriend.Fixture.Cast.Temp;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class DatabaseUpdaterTest
    {
        private FixtureBook fixtureBook = new FixtureBook();

        [TestMethod]
        public void 指定されたデータをデータベーステーブルに投入できる_SQLServer()
        {
            // setup
            Loggi.DebugEnabled = true;

            // when
            fixtureBook.Setup();

            // then
            using (Database database = new Database())
            {
                DataTable table = database.ExecuteQuery("select * from TypesTable order by Id");
                fixtureBook.Validate(table, "TypesTable");
            }
        }

        [TestMethod]
        public void 指定されたデータをデータベーステーブルに投入できる_Oracle()
        {
            // setup
            Loggi.DebugEnabled = true;

            // when
            fixtureBook.Setup();

            // then
            using (Database database = new Database())
            {
                database.Use("Oracle");
                DataTable table = database.ExecuteQuery("select * from TYPES_TABLE order by ID");
                fixtureBook.Validate(table, "TYPES_TABLE");
            }
        }

        [TestMethod]
        public void BLOB項目にbyte配列の読み込みができる()
        {
            // setup
            Loggi.DebugEnabled = true;

            // when
            fixtureBook.Setup();

            // then
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        public void image項目にbyte配列の読み込みができる()
        {
            // setup
            Loggi.DebugEnabled = true;

            // when
            fixtureBook.Setup();

            // then
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        public void Bテストデータクリア条件で存在しないテーブル名を指定した場合は例外が発生する()
        {
            try
            {
                // when
                fixtureBook.Setup();
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e.Message);
                Assert.AreEqual("M_Fixture_Temp_Database_DeleteRow", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("xxxx") > -1);
            }
        }

        [TestMethod]
        public void Cテストデータで存在しないテーブル名を指定した場合は例外が発生する()
        {
            try
            {
                // when
                fixtureBook.Setup();
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e.Message);
                Assert.AreEqual("M_Fixture_Temp_Database_GetMetaData", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("xxxx") > -1);
            }
        }

        [TestMethod]
        public void 重複データ登録時には例外が発生する()
        {
            try
            {
                // when
                fixtureBook.Setup();
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Temp_Database_InsertRow", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("TypesTable") > -1);
            }
        }

        [TestMethod]
        public void Bテストデータクリア条件で存在しない列名を指定した場合例外が発生する()
        {
            try
            {
                // when
                fixtureBook.Setup();
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e.Message);
                Assert.AreEqual("M_Fixture_Temp_Database_DeleteRow", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("TypesTable") > -1);
            }
        }

        [TestMethod]
        public void Bテストデータクリア条件で不正な列値を指定した場合例外が発生する()
        {
            try
            {
                // when
                fixtureBook.Setup();
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e.Message);
                Assert.AreEqual("M_Fixture_Temp_Database_DeleteRow", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("TypesTable") > -1);
            }
        }

        [TestMethod]
        public void Bテストデータクリア条件では部分一致指定が可能()
        {
            // setup
            Loggi.DebugEnabled = true;
            DatabaseUpdaterTest__Bテストデータクリア条件では部分一致指定が可能_Setup();

            // when
            fixtureBook.Setup();

            // then
            fixtureBook.ValidateStorage();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void DatabaseUpdaterTest__Bテストデータクリア条件では部分一致指定が可能_Setup()
        {
            new FixtureBook().Setup();
        }

        [TestMethod]
        public void Bテストデータクリア条件では複数条件指定が可能()
        {
            // setup
            Loggi.DebugEnabled = true;
            DatabaseUpdaterTest__Bテストデータクリア条件では複数条件指定が可能_Setup();

            // when
            fixtureBook.Setup();

            // then
            fixtureBook.ValidateStorage();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void DatabaseUpdaterTest__Bテストデータクリア条件では複数条件指定が可能_Setup()
        {
            new FixtureBook().Setup();
        }

        [TestMethod]
        public void IDENTITY列に対して明示的に値を設定できる()
        {
            // when
            fixtureBook.Setup();

            // then
            fixtureBook.ValidateStorage();
        }
    }
}
