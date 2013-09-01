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

        [TestMethod]
        public void 引数で指定されたテーブル定義名よりもDataSetの中に含まれるDataTableが少ないと例外が発生する() 
        {
            // setup
            DataSet dataSet = fixtureBook.GetObject<DataSet>();

            // when
            try
            {
                fixtureBook.Validate(dataSet, "Table1", "Table2", "Table3");
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("Table1, Table2, Table3") > -1);
            }
        }

        [TestMethod]
        public void 引数でテーブル名指定をしていない場合はDataSetのテーブル名と一致するテーブル定義を使って検証を行う()
        {
            // 正常終了
            {
                // setup
                DataSet dataSet = fixtureBook.GetObject<DataSet>();

                // expect
                fixtureBook.Validate(dataSet);
            }

            // エラー
            {
                // setup
                DataSet dataSet = fixtureBook.GetObject<DataSet>();
                dataSet.Tables[1].Rows[0]["text"] = "ccc";

                // when
                try
                {
                    fixtureBook.Validate(dataSet);
                }
                catch (AssertFailedException e)
                {
                    Console.WriteLine(e.Message);
                    Assert.IsTrue(e.Message.IndexOf("<bbb>") > -1);
                    Assert.IsTrue(e.Message.IndexOf("<ccc>") > -1);
                }
            }
        }

        [TestMethod]
        public void 引数でテーブル名指定をしていない場合にDataSetのテーブル数がE取得データのテーブル数よりも多いと例外が発生する()
        {
            // setup
            DataSet dataSet = fixtureBook.GetObject<DataSet>();

            // when
            try
            {
                fixtureBook.Validate(dataSet);
                throw new Exception("ここにはこない");
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("DataSet") > -1);
                Assert.IsTrue(e.Message.IndexOf("DataTable") > -1);
                Assert.IsTrue(e.Message.IndexOf("(2)") > -1);
                Assert.IsTrue(e.Message.IndexOf("(1)") > -1);
            }
        }

        [TestMethod]
        public void 明示的にテーブル名指定をしていない場合でDataSetのテーブル名とE取得データのテーブル定義名が一致しなければ定義順で検証する()
        {
            // 正常終了
            {
                // setup
                DataSet dataSet = fixtureBook.GetObject<DataSet>();

                // expect
                fixtureBook.Validate(dataSet);
            }

            // エラー
            {
                // setup
                DataSet dataSet = fixtureBook.GetObject<DataSet>();
                dataSet.Tables[0].Rows[0]["text"] = "bbb";

                // when
                try
                {
                    fixtureBook.Validate(dataSet);
                    throw new Exception("ここにはこない");
                }
                catch (AssertFailedException e)
                {
                    Console.WriteLine(e);
                    Assert.IsTrue(e.Message.IndexOf("<aaa>") > -1);
                    Assert.IsTrue(e.Message.IndexOf("<bbb>") > -1);
                }
            }
        }
    }
}
