using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
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
    }
}
