using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture;
using XPFriend.Fixture.Cast.Temp;
using XPFriend.Fixture.Staff;
using System.Data;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class DataTableValidatorTest
    {
        private FixtureBook fixtureBook = new FixtureBook();

        [TestMethod]
        public void HasRoleはInitializeで指定されたテストケースに取得データセクションがなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DataTableValidator validator = GetDataTableValidator(sheet.GetCase("ロールなし"));

            // expect
            Assert.IsFalse(validator.HasRole(null, null));
        }

        [TestMethod]
        public void HasRoleは引数のクラスがDictionaryでなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DataTableValidator validator = GetDataTableValidator(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsFalse(validator.HasRole("xxx", null));
        }

        private DataTableValidator GetDataTableValidator(Case testCase)
        {
            TempObjectValidator parent = new TempObjectValidator();
            parent.Initialize(testCase);
            return parent.dataTableValidator;
        }

        [TestMethod]
        public void Validateは指定されたオブジェクトが予想結果と等しいかどうかを調べる()
        {
            // setup
            DataTable actual = fixtureBook.GetObject<DataTable>("Data");

            // expect
            fixtureBook.Validate(actual, "Data");
        }

        [TestMethod]
        [Fixture("DataTableValidatorTest", "Validateは指定されたオブジェクトが予想結果と等しいかどうかを調べる")]
        public void Validateの第二引数が指定されなかった場合DataTableのTableNameプロパティが使用される()
        {
            // setup
            DataTable actual = fixtureBook.GetObject<DataTable>("Data");

            Console.WriteLine(actual.TableName);

            // expect
            fixtureBook.Validate(actual);
        }

        [TestMethod]
        public void DBNull値の検証ができる()
        {
            // setup
            DataTable actual = fixtureBook.GetObject<DataTable>("Data");
            Assert.AreEqual(DBNull.Value, actual.Rows[0]["s1"]); // 念のため
            Assert.AreEqual(DBNull.Value, actual.Rows[0]["d1"]); // 念のため

            // expect
            fixtureBook.Validate(actual, "Data");
        }

    }
}
