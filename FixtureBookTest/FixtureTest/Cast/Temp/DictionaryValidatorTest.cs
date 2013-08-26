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
using System.Collections.Generic;
using XPFriend.Fixture.Staff;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class DictionaryValidatorTest
    {
        private FixtureBook fixtureBook = new FixtureBook();

        [TestMethod]
        public void HasRoleはInitializeで指定されたテストケースに取得データセクションがなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DictionaryValidator validator = GetDictionaryValidator(sheet.GetCase("ロールなし"));

            // expect
            Assert.IsFalse(validator.HasRole(null, null));
        }

        [TestMethod]
        public void HasRoleは引数のクラスがDictionaryでなければfalseを返す()
        {
            // setup
            Sheet sheet = TempActors.Book.GetSheet("HasRole");
            DictionaryValidator validator = GetDictionaryValidator(sheet.GetCase("ロールあり"));

            // expect
            Assert.IsFalse(validator.HasRole("xxx", null));
        }

        [TestMethod]
        public void Validateは指定されたオブジェクトが予想結果と等しいかどうかを調べる()
        {
            // setup
            Dictionary<string, object> actual = fixtureBook.GetObject<Dictionary<string, object>>("Data");

            // expect
            fixtureBook.Validate(actual, "Data");
        }

        private DictionaryValidator GetDictionaryValidator(Case testCase)
        {
            TempObjectValidator parent = new TempObjectValidator();
            parent.Initialize(testCase);
            return parent.dictionaryValidator;
        }
    }
}
