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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture;

namespace XPFriend.FixtureTest.Tutorial
{
    [TestClass]
    public class EmployeeStoreTest
    {
        [TestMethod]
        public void Save__データベーステーブルEMPLOYEEに従業員データを新規追加できる()
        {
            FixtureBook.Expect();
        }

        [TestMethod]
        public void Delete__指定した従業員データのIDをキーにしてデータベーステーブルEMPLOYEE上のデータが削除される()
        {
            FixtureBook.Expect();
        }

        [TestMethod]
        public void GetAllEmployees__データベーステーブルEMPLOYEE上の全データが取得できる()
        {
            FixtureBook.ExpectReturn();
        }

        [TestMethod]
        public void GetEmployees__引数の退職フラグが1の場合データベーステーブルEMPLOYEE上の退職者のみが取得できる()
        {
            FixtureBook.ExpectReturn();
        }

        [TestMethod]
        public void GetEmployees__引数の退職フラグが0の場合データベーステーブルEMPLOYEE上の未退職者のみが取得できる()
        {
            FixtureBook.ExpectReturn();
        }

        [TestMethod]
        [Fixture("Delete", @"指定した従業員データのIDが null ならば ""Invalid ID"" というメッセージを持つ ApplicationException が発生する")]
        public void Delete__指定した従業員データのIDがnullならばInvalid_IDというメッセージを持つApplicationExceptionが発生する()
        {
            FixtureBook.ExpectThrown();
        }
    }
}
