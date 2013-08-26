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
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.FixtureTest.Staff
{
    [TestClass]
    public class BookTest
    {
        public const string BookFilePath = @"..\..\FixtureTest\Staff\BookTest.xlsx";

        [TestMethod]
        public void GetInstanceは引数で指定したExcelファイルを読み込んでBookインスタンスを作成する()
        {
            // expect
            Assert.IsNotNull(Book.GetInstance(BookFilePath));
        }

        [TestMethod]
        public void GetInstanceで同一ファイル名を指定して複数回呼び出しを行うと毎回同じBookインスタンスを返す() 
        {
            // when
            var instance1 = Book.GetInstance(BookFilePath);
            var instance2 = Book.GetInstance(BookFilePath);

            // then
            Assert.AreSame(instance1, instance2);
        }

        [TestMethod]
        public void ClearCacheはBookインスタンスのキャッシュをクリアする()
        {
            // setup
            var instance1 = Book.GetInstance(BookFilePath);

            // when
            Book.ClearCache();
            var instance2 = Book.GetInstance(BookFilePath);

            // then
            Assert.AreNotSame(instance1, instance2);
        }

        [TestMethod]
        public void FilePathで取得できるファイル名はGetInstance引数で指定された名前を絶対パスに変換したものになる()
        {
            // expect
            Assert.AreEqual(Path.GetFullPath(BookFilePath), Book.GetInstance(BookFilePath).FilePath);
        }

        [TestMethod]
        public void ToStringはフォルダ情報のないファイル名を返す()
        {
            // setup
            var book = Book.GetInstance(BookFilePath);

            // expect
            Assert.AreEqual("BookTest.xlsx", book.ToString());
        }

        [TestMethod]
        public void GetSheetでは指定された名前のSheetが取得できる()
        {
            GetSheetでは指定された名前のSheetが取得できる("Sheet01");
            GetSheetでは指定された名前のSheetが取得できる("Sheet02");
        }

        private void GetSheetでは指定された名前のSheetが取得できる(string sheetName)
        {
            // setup
            var book = Book.GetInstance(BookFilePath);

            // when
            var sheet = book.GetSheet(sheetName);

            // then
            Assert.AreEqual(sheetName, sheet.Name);
        }

        [TestMethod]
        public void GetSheetではブックに存在しないシート名を指定すると例外が発生する()
        {
            // setup
            var book = Book.GetInstance(BookFilePath);

            try
            {
                // when
                book.GetSheet("xxx");
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_Book_GetSheet", e.ResourceKey);
                Assert.IsTrue(e.Message.IndexOf("xxx") > -1);
                Assert.IsTrue(e.Message.IndexOf("BookTest.xlsx") > -1);
            }
        }

        [TestMethod]
        public void DebugEnabledでtrueを指定するとDebugEnabledがtrueを返しfalseを指定するとfalseが返る()
        {
            DebugEnabledでtrueを指定するとDebugEnabledがtrueを返しfalseを指定するとfalseが返る(true);
            DebugEnabledでtrueを指定するとDebugEnabledがtrueを返しfalseを指定するとfalseが返る(false);
        }

        private void DebugEnabledでtrueを指定するとDebugEnabledがtrueを返しfalseを指定するとfalseが返る(bool debugEnabled)
        {
            // when
            Book.DebugEnabled = debugEnabled;

            // then
            Assert.AreEqual(debugEnabled, Book.DebugEnabled);
        }
    }
}
