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
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture;
using XPFriend.Junk;
using XPFriend.Fixture.Cast.Temp;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class DatabaseValidatorTest
    {
        private FixtureBook fixtureBook = new FixtureBook();

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "検索列指定をしてデータベーステーブルの状態がチェックできる_SQLServer")]
        public void 検索列指定をしてデータベーステーブルの状態が予想結果と同じであることをチェックできる_SQLServer()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            // expect
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "検索列指定をしてデータベーステーブルの状態がチェックできる_Oracle")]
        public void 検索列指定をしてデータベーステーブルの状態が予想結果と同じであることをチェックできる_Oracle()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            // expect
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "検索列指定をしてデータベーステーブルの状態がチェックできる_SQLServer")]
        public void 指定した検索列が存在しない場合はエラー_SQLServer()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();
            Database.ExecuteNonQuery(null, "delete from TypesTable");

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(message);
                Assert.IsTrue(Regex.IsMatch(message, ".*TypesTable.*TypesTable.*"));
                Assert.IsTrue(message.IndexOf("*") == -1);
            }
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "検索列指定をしてデータベーステーブルの状態がチェックできる_Oracle")]
        public void 指定した検索列が存在しない場合はエラー_Oracle()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();
            Database.ExecuteNonQuery("Oracle", "delete from TYPES_TABLE");

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(message);
                Assert.IsTrue(Regex.IsMatch(message, ".*TYPES_TABLE.*TYPES_TABLE.*"));
                Assert.IsTrue(message.IndexOf("*") == -1);
            }
        }

        [TestMethod]
        public void 指定した検索列が存在しない場合はエラー()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(message);
                Assert.IsTrue(Regex.IsMatch(message, ".*TypesTable.*TypesTable.*"));
                Assert.IsTrue(message.IndexOf("*") == -1);
            }
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "検索列指定をしてデータベーステーブルの状態がチェックできる_SQLServer")]
        public void 検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer()
        {
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("bigint1", 2);
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("bit1", true);
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("date1", "2012-12-02");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("datetime1", "2012-12-02 12:34:57");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("datetime2", "2012-12-03 12:34:56.1234568 +09:00");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("datetimeoffset1", "2012-12-04 12:34:56.123456 +12:16");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("decimal", "123.457");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("float1", "12.4");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("int1", "5");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("money1", "1234.0001");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("mumeric1", "124");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("real1", "32.2");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("smalldatetime1", "2012-12-05 12:34:00");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("smallint1", "6");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("smallmoney1", "7.0000");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("sql_variant1", "8");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("text1", "b");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("time1", "12:34:57");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("tinyint1", "9");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("varchar1", "c");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("xml1", "b");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("ntext1", "う");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("nvarchar1", "え");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("char1", "b         ");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("nchar1", "い         ");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("binary1", new byte[] { 2, 2, 0, 0 });
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("image1", new byte[] { 1, 2, 3 });
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("varbinary1", new byte[] { 2 });
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer("uniqueidentifier1", new byte[] { 2 }, "00000002-0000-0000-0000-000000000000");
        }

        private void 検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer(string column, object value)
        {
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer(column, value, ToString(value));
        }

        private void 検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer(string column, object value, string valueAsText)
        {
            // setup
            fixtureBook.Setup();
            using (Database database = new Database())
            {
                DbCommand command = database.CreateCommand("update TypesTable set " + column + "=@" + column + " where Id=2");
                database.AddParameter(command, column, null, value);
                database.ExecuteNonQuery(command);
                database.Commit();
            }

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(message);
                Assert.IsTrue(message.IndexOf(column) > -1);
                Assert.IsTrue(message.IndexOf("<" + valueAsText + ">") > -1);
            }
        }

        private string ToString(object value)
        {
            if (value is byte[])
            {
                return ByteArrayToString((byte[])value);
            }
            return value.ToString();
        }

        private string ByteArrayToString(byte[] bytes)
        {
            string[] strings = new string[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                strings[i] = bytes[i].ToString();
            }
            return String.Join("|", strings);
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "検索列指定をしてデータベーステーブルの状態がチェックできる_Oracle")]
        public void 検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle()
        {
            Loggi.DebugEnabled = true;
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("CHAR1", "b", "b         ");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("VARCHAR1", "c");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("NCHAR1", "う         ");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("NVARCHAR1", "え");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("INT1", 2);
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("DECIMAL1", 123.457);
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("CLOB1", "b");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("BLOB1", "3");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("DATE1", DateTime.Parse("2012-12-02 12:34:57"), "2012-12-02 12:34:57");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("TIMESTAMP1", DateTime.Parse("2012-12-02 12:34:56.788"), "2012-12-02 12:34:56.788");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("TIMESTAMPTZ", DateTime.Parse("2012-12-03 01:00:01"), "2012-12-03 01:00:01");
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle("TIMESTAMPLTZ", DateTime.Parse("2012-12-03 01:00:01"), "2012-12-03 01:00:01");
        }

        private void 検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle(string column, object value)
        {
            検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle(column, value, ToString(value));
        }

        private void 検索列指定をしてデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle(string column, object value, string valueAsText)
        {
            // setup
            fixtureBook.Setup();
            using (Database database = new Database())
            {
                database.Use("Oracle");
                DbCommand command = database.CreateCommand("update TYPES_TABLE set " + column + "=:" + column + " where Id=2");
                database.AddParameter(command, column, null, value);
                database.ExecuteNonQuery(command);
                database.Commit();
            }

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(e);
                Console.WriteLine(message);
                Assert.IsTrue(message.IndexOf(column) > -1);
                Assert.IsTrue(message.IndexOf("<" + valueAsText + ">") > -1);
            }
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "検索列指定をせずにデータベーステーブルの状態がチェックできる_SQLServer")]
        public void 検索列指定をせずにデータベーステーブルの状態が予想結果と同じであることをチェックできる_SQLServer()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            // expect
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "検索列指定をせずにデータベーステーブルの状態がチェックできる_Oracle")]
        public void 検索列指定をせずにデータベーステーブルの状態が予想結果と同じであることをチェックできる_Oracle()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            // expect
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "検索列指定をせずにデータベーステーブルの状態がチェックできる_SQLServer")]
        public void 検索列指定をせずにデータベーステーブルの状態が予想結果と異なることをチェックできる_SQLServer()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();
            Database.ExecuteNonQuery(null, "update TypesTable set bigint1=2 where Id=2");

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(message);
                Assert.IsTrue(Regex.IsMatch(message, ".*TypesTable.*TypesTable.*"));
                Assert.IsTrue(message.IndexOf("*") > -1);
            }
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "検索列指定をせずにデータベーステーブルの状態がチェックできる_Oracle")]
        public void 検索列指定をせずにデータベーステーブルの状態が予想結果と異なることをチェックできる_Oracle()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();
            Database.ExecuteNonQuery("Oracle", "update TYPES_TABLE set VARCHAR1='c' where Id=2");

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(message);
                Assert.IsTrue(Regex.IsMatch(message, ".*TYPES_TABLE.*TYPES_TABLE.*"));
                Assert.IsTrue(message.IndexOf("*") > -1);
            }
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "削除済み指定（D）のデータが存在しない場合は正常終了となる")]
        public void 削除済み指定のデータが存在しない場合は正常終了となる()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            // expect
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "削除済み指定（D）のデータが存在しない場合は正常終了となる_Oracle")]
        public void 削除済み指定のデータが存在しない場合は正常終了となる_Oracle()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            // expect
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "削除済み指定（D）のデータとD指定のないデータの両方を同時に検証できる")]
        public void D指定のデータとD指定のないデータの両方を同時に検証できる()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            // expect
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "削除済み指定（D）のデータが存在する場合はエラーとなる")]
        public void 削除済み指定のデータが存在しない場合は正常終了となるその2()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();
            Database.ExecuteNonQuery(null, "delete from TypesTable where Id = 2");

            // expect
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "削除済み指定（D）のデータが存在する場合はエラーとなる_Oracle")]
        public void 削除済み指定のデータが存在しない場合は正常終了となるその2_Oracle()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();
            Database.ExecuteNonQuery("Oracle", "delete from TYPES_TABLE where ID = 2");

            // expect
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "削除済み指定（D）のデータが存在する場合はエラーとなる")]
        public void 削除済み指定のデータが存在する場合はエラーとなる()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(message);
                Assert.IsTrue(Regex.IsMatch(message, ".*TypesTable.*TypesTable.*"));
            }
        }

        [TestMethod]
        [Fixture("DatabaseValidatorTest", "削除済み指定（D）のデータが存在する場合はエラーとなる_Oracle")]
        public void 削除済み指定のデータが存在する場合はエラーとなる_Oracle()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(message);
                Assert.IsTrue(Regex.IsMatch(message, ".*TYPES_TABLE.*TYPES_TABLE.*"));
            }
        }

        [TestMethod]
        public void 指定した検索列条件で複数ヒットしてしまう場合はエラーとなる()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(message);
                Assert.IsTrue(Regex.IsMatch(message, ".*TypesTable.*TypesTable.*"));
                Assert.IsTrue(message.IndexOf("{bigint1=1}") > -1);
            }
        }

        [TestMethod]
        public void 指定した検索列条件で複数ヒットしてしまう場合はエラーとなる_Oracle()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            try
            {
                // when
                fixtureBook.ValidateStorage();
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                string message = e.Message;
                Console.WriteLine(message);
                Assert.IsTrue(Regex.IsMatch(message, ".*TYPES_TABLE.*TYPES_TABLE.*"));
                Assert.IsTrue(message.IndexOf("{VARCHAR1=b") > -1);
            }
        }

        [TestMethod]
        public void 複数の接続先に対して同時に操作できる()
        {
            // setup
            Loggi.DebugEnabled = true;
            fixtureBook.Setup();

            // expect
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        public void BLOB項目の値をチェックできる()
        {
            // setup
            Loggi.DebugEnabled = true;

            // when
            fixtureBook.Setup();

            // then
            fixtureBook.ValidateStorage();
        }

        [TestMethod]
        public void image項目の値をチェックできる()
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
