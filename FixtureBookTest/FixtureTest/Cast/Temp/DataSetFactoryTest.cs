using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture;
using XPFriend.Fixture.Staff;
using XPFriend.Fixture.Cast.Temp;
using System.Data;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class DataSetFactoryTest
    {
        private FixtureBook fixtureBook = new FixtureBook();

        [TestMethod]
        public void HasRoleはInitializeで指定されたテストケースにパラメタセクションがなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DataSetFactory factory = GetDataSetFactory(sheet.GetCase("ロールなし"));

            // expect
            Assert.IsFalse(factory.HasRole<object>(null));
        }

        [TestMethod]
        public void HasRoleは引数のクラスがDictionaryでなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DataSetFactory factory = GetDataSetFactory(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsFalse(factory.HasRole<object>(null));
        }

        [TestMethod]
        public void HasRoleは引数のクラスがDataSetでありかつInitializeで指定されたテストケースにパラメタセクションがあればtrueを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DataSetFactory factory = GetDataSetFactory(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsTrue(factory.HasRole<DataSet>(null));
        }

        private DataSetFactory GetDataSetFactory(Case testCase)
        {
            TempObjectFactory parent = new TempObjectFactory();
            parent.Initialize(testCase);
            return parent.dataSetFactory;
        }

        [TestMethod]
        public void GetObjectは指定された名前の定義を読み込んでオブジェクト作成する()
        {
            // when
            DataSet dataSet = fixtureBook.GetObject<DataSet>("Data");

            // then
            Assert.AreEqual(1, dataSet.Tables.Count);
            DataTableFactoryTest.Validate(dataSet.Tables[0]);
        }

        [TestMethod]
        public void 親子構造になったデータを取得できる()
        {
            // when
            DataSet dataSet = fixtureBook.GetObject<DataSet>("Order");

            // then
            Assert.AreEqual(1, dataSet.Tables.Count);
            DataTableFactoryTest.ValidateNested(dataSet.Tables[0]);
        }

        [TestMethod]
        public void バーティカルバー区切りの値で配列およびリストを表現できる()
        {
            // when
            DataTable dataTable = fixtureBook.GetObject<DataTable>("ArrayData");
            DataRow arrayData = dataTable.Rows[0];

            // then
            DataTableFactoryTest.ValidateArrayData(arrayData);
        }

        [TestMethod]
        [Fixture("DataSetFactoryTest", "親子構造になったデータを取得できる")]
        public void 複数のデータを取得できる()
        {
            // when
            DataSet dataSet = fixtureBook.GetObject<DataSet>("CustomerInfo", "Detail");

            // then
            Assert.AreEqual(2, dataSet.Tables.Count);

            DataRow customerInfo = dataSet.Tables[0].Rows[0];
            Assert.AreEqual("C001", customerInfo["code"]);
            Assert.AreEqual("XX商事", customerInfo["name"]);
            Assert.AreEqual("045-999-9999", customerInfo["telno"]);

            DataRowCollection detail = dataSet.Tables[1].Rows;
            Assert.AreEqual(2, detail.Count);
            Assert.AreEqual("001", detail[0]["detailNo"]);
            Assert.AreEqual("X01", detail[0]["itemCd"]);
            Assert.AreEqual(10, detail[0]["qty"]);
            Assert.AreEqual("002", detail[1]["detailNo"]);
            Assert.AreEqual("X02", detail[1]["itemCd"]);
            Assert.AreEqual(20, detail[1]["qty"]);
        }
    }
}
