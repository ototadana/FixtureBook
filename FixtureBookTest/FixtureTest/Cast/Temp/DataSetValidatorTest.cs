using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture;
using XPFriend.Fixture.Cast.Temp;
using XPFriend.Fixture.Staff;
using System.Data;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class DataSetValidatorTest
    {
        private FixtureBook fixtureBook = new FixtureBook();

        [TestMethod]
        public void HasRoleはInitializeで指定されたテストケースに取得データセクションがなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DataSetValidator validator = GetDataSetValidator(sheet.GetCase("ロールなし"));

            // expect
            Assert.IsFalse(validator.HasRole(null, null));
        }

        [TestMethod]
        public void HasRoleは引数のクラスがDictionaryでなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DataSetValidator validator = GetDataSetValidator(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsFalse(validator.HasRole("xxx", null));
        }

        private DataSetValidator GetDataSetValidator(Case testCase)
        {
            TempObjectValidator parent = new TempObjectValidator();
            parent.Initialize(testCase);
            return parent.dataSetValidator;
        }

        [TestMethod]
        public void Validateは指定されたオブジェクトが予想結果と等しいかどうかを調べる()
        {
            // setup
            DataSet actual = fixtureBook.GetObject<DataSet>("Data");

            // expect
            fixtureBook.Validate(actual, "Data");
        }

        [TestMethod]
        [Fixture("DataSetValidatorTest", "DataSetを利用する場合は複数のテーブルを指定できる")]
        public void DataSetを利用する場合は複数のテーブルを指定できる_明示的にテーブル定義名を指定する場合()
        {
            // setup
            DataSet actual = fixtureBook.GetObject<DataSet>("CustomerInfo", "Detail");

            // expect
            fixtureBook.Validate(actual, "CustomerInfo", "Detail");
        }

        [TestMethod]
        [Fixture("DataSetValidatorTest", "DataSetを利用する場合は複数のテーブルを指定できる")]
        public void DataSetを利用する場合は複数のテーブルを指定できる_テーブル定義名を全く指定しない場合()
        {
            // setup
            DataSet actual = fixtureBook.GetObject<DataSet>("CustomerInfo", "Detail");

            // expect
            fixtureBook.Validate(actual);
        }

        [TestMethod]
        [Fixture("DataSetValidatorTest", "DataSetを利用する場合は複数のテーブルを指定できる")]
        public void DataSetValidator__DataSetを利用する場合は複数のテーブルを指定できる_GetObjectでテーブル定義を指定せずにValidateで明示的に指定する場合()
        {
            // setup
            DataSet actual = fixtureBook.GetObject<DataSet>();

            // expect
            Assert.AreEqual(2, actual.Tables.Count);
            fixtureBook.Validate(actual, "CustomerInfo", "Detail");
        }

        [TestMethod]
        [Fixture("DataSetValidatorTest", "DataSetを利用する場合は複数のテーブルを指定できる")]
        public void DataSetValidator__DataSetを利用する場合は複数のテーブルを指定できる_GetObjectでもValidateでもテーブル定義を指定しない場合()
        {
            // setup
            DataSet actual = fixtureBook.GetObject<DataSet>();

            // expect
            Assert.AreEqual(2, actual.Tables.Count);
            fixtureBook.Validate(actual);
        }
    }
}
