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
using XPFriend.Fixture.Cast.Temp;

namespace XPFriend.FixtureTest.Staff
{
    [TestClass]
    [FixtureBook(@"FixtureTest\Staff\BookExtraTest.xlsx")]
    public class Book1Test
    {
        [TestMethod]
        public void BookTest__Bookのインスタンスはファイルパス単位ではなくテストクラス単位でおこなわれる()
        {
            // expect : キー重複例外が発生しないこと
            Assert.AreEqual("a", new FixtureBook().GetObject<string>());

            // setup : わざとテーブルにデータ追加しておく
            Database.ExecuteNonQuery(null, "insert into typesTable(id) values(1)");
        }
    }

    [TestClass]
    [FixtureBook(@"FixtureTest\Staff\BookExtraTest.xlsx")]
    public class Book2Test
    {
        [TestMethod]
        public void BookTest__Bookのインスタンスはファイルパス単位ではなくテストクラス単位でおこなわれる()
        {
            // expect : キー重複例外が発生しないこと
            Assert.AreEqual("a", new FixtureBook().GetObject<string>());

            // setup : わざとテーブルにデータ追加しておく
            Database.ExecuteNonQuery(null, "insert into typesTable(id) values(1)");
        }
    }

}
