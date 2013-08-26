using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture.Cast.Temp;
using XPFriend.Fixture;
using XPFriend.Fixture.Staff;
using System.Data;
using System.Collections.Generic;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class DataTableFactoryTest
    {
        private FixtureBook fixtureBook = new FixtureBook();

        [TestMethod]
        public void HasRoleはInitializeで指定されたテストケースにパラメタセクションがなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DataTableFactory factory = GetDataTableFactory(sheet.GetCase("ロールなし"));

            // expect
            Assert.IsFalse(factory.HasRole<object>(null));
        }

        [TestMethod]
        public void HasRoleは引数のクラスがDictionaryでなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DataTableFactory factory = GetDataTableFactory(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsFalse(factory.HasRole<object>(null));
        }

        [TestMethod]
        public void HasRoleは引数のクラスがDataTableでありかつInitializeで指定されたテストケースにパラメタセクションがあればtrueを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DataTableFactory factory = GetDataTableFactory(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsTrue(factory.HasRole<DataTable>(null));
        }

        private DataTableFactory GetDataTableFactory(Case testCase)
        {
            TempObjectFactory parent = new TempObjectFactory();
            parent.Initialize(testCase);
            return parent.dataTableFactory;
        }

        [TestMethod]
        public void GetObjectは指定された名前の定義を読み込んでオブジェクト作成する()
        {
            // when
            DataTable dataTable = fixtureBook.GetObject<DataTable>("Data");

            // then
            Validate(dataTable);
        }

        internal static void Validate(DataTable dataTable)
        {
            Assert.AreEqual(2, dataTable.Rows.Count);

            DataRow dataRow = dataTable.Rows[0];
            Assert.AreSame(dataTable, dataRow.Table);

            Validate(dataRow["s1"], "a", typeof(string));
            Validate(dataRow["s2"], "b", typeof(string));
            Validate(dataRow["decimal1"], 1.111m, typeof(decimal));
            Validate(dataRow["decimal2"], 2.222m, typeof(decimal));
            Validate(dataRow["int1"], 3, typeof(int));
            Validate(dataRow["int2"], 4, typeof(int));
            Validate(dataRow["long1"], 5L, typeof(long));
            Validate(dataRow["short1"], (short)6, typeof(short));
            Validate(dataRow["float1"], 7.77f, typeof(float));
            Validate(dataRow["double1"], 8.88d, typeof(double));
            Validate(dataRow["byte1"], (byte)9, typeof(byte));

            Validate(dataRow["sbyte1"], (sbyte)1, typeof(sbyte));
            Validate(dataRow["ushort1"], (ushort)2, typeof(ushort));
            Validate(dataRow["int16a"], (short)3, typeof(Int16));
            Validate(dataRow["uint16a"], (ushort)4, typeof(UInt16));
            Validate(dataRow["uinta"], 5u, typeof(uint));
            Validate(dataRow["int32a"], 6, typeof(Int32));
            Validate(dataRow["uint32a"], 7u, typeof(UInt32));
            Validate(dataRow["ulonga"], 8ul, typeof(ulong));
            Validate(dataRow["int64a"], 9L, typeof(Int64));
            Validate(dataRow["uint64a"], 10ul, typeof(UInt64));
            Validate(dataRow["singlea"], 11f, typeof(Single));

            Validate(dataRow["char1"], 'c', typeof(char));
            Validate(dataRow["char2"], 'd', typeof(char));
            Validate(dataRow["boolean1"], true, typeof(bool));
            Validate(dataRow["boolean2"], false, typeof(bool));
            Validate(dataRow["timestamp1"], ToDateTime("2013-01-02 12:13:14"), typeof(DateTime));
            Validate(dataRow["timestamp2"], ToDateTime("2013-01-03 12:13:15"), typeof(DateTime));
            Validate(dataRow["date1"], ToDateTime("2012-12-31 00:00:00"), typeof(DateTime));

            // .NET の場合、時間に関しては本日の時間となるが、変換仕様としては「日付部分は不定」とする。
            Validate(dataRow["time1"], ToDateTime(DateTime.Today.ToString("yyyy-MM-dd") + " 10:10:11"), typeof(DateTime));

            DataRow nullRow = dataTable.Rows[1];
            int count = nullRow.Table.Columns.Count;
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(nullRow.IsNull(i));
                Assert.AreSame(DBNull.Value, nullRow[i]);
            }
        }

        private static object ToDateTime(string value)
        {
            return Convert.ToDateTime(value);
        }

        private static void Validate(object actual, object expected, Type type)
        {
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(type, actual.GetType());
        }

        [TestMethod]
        public void 親子構造になったデータを取得できる()
        {
            // when
            DataTable dataTable = fixtureBook.GetObject<DataTable>("Order");

            // then
            ValidateNested(dataTable);
        }

        internal static void ValidateNested(DataTable dataTable)
        {
            Assert.AreEqual(1, dataTable.Rows.Count);
            DataRow order = dataTable.Rows[0];
            Assert.AreEqual("H001", order["orderNo"]);

            Dictionary<string, object> customerInfo = (Dictionary<string, object>)order["customerInfo"];
            Assert.AreEqual("C001", customerInfo["code"]);
            Assert.AreEqual("XX商事", customerInfo["name"]);
            Assert.AreEqual("045-999-9999", customerInfo["telno"]);

            List<Dictionary<string, object>> detail = (List<Dictionary<string, object>>)order["detail"];
            Assert.AreEqual(2, detail.Count);
            Assert.AreEqual("001", detail[0]["detailNo"]);
            Assert.AreEqual("X01", detail[0]["itemCd"]);
            Assert.AreEqual(10, detail[0]["qty"]);
            Assert.AreEqual("002", detail[1]["detailNo"]);
            Assert.AreEqual("X02", detail[1]["itemCd"]);
            Assert.AreEqual(20, detail[1]["qty"]);
        }

        [TestMethod]
        public void バーティカルバー区切りの値で配列およびリストを表現できる()
        {
            // when
            DataTable dataTable = fixtureBook.GetObject<DataTable>("ArrayData");
            DataRow arrayData = dataTable.Rows[0];

            // then
            ValidateArrayData(arrayData);
        }

        internal static void ValidateArrayData(DataRow arrayData)
        {
            string[] stringArray = (string[])arrayData["stringArray"];
            Assert.AreEqual(3, stringArray.Length);
            Assert.AreEqual("a", stringArray[0]);
            Assert.AreEqual("b", stringArray[1]);
            Assert.AreEqual("c", stringArray[2]);

            int[] intArray = (int[])arrayData["intArray"];
            Assert.AreEqual(3, intArray.Length);
            Assert.AreEqual(1, intArray[0]);
            Assert.AreEqual(2, intArray[1]);
            Assert.AreEqual(3, intArray[2]);

            List<string> stringList1 = (List<string>)arrayData["stringList1"];
            Assert.AreEqual(3, stringList1.Count);
            Assert.AreEqual("e", stringList1[0]);
            Assert.AreEqual("f", stringList1[1]);
            Assert.AreEqual("g", stringList1[2]);

            List<object> stringList2 = (List<object>)arrayData["stringList2"];
            Assert.AreEqual(3, stringList2.Count);
            Assert.AreEqual("h", stringList2[0]);
            Assert.AreEqual("i", stringList2[1]);
            Assert.AreEqual("j", stringList2[2]);
        }
    }
}
