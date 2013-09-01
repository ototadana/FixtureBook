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
using System;
using System.Collections.Generic;
using XPFriend.Fixture;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.FixtureTest.Staff.Xlsx
{
    [TestClass]
    public class XlsxWorksheetHandlerTest
    {
        [TestInitialize]
        public void Setup()
        {
            Book.DebugEnabled = true;
            Loggi.DebugEnabled = true;
        }

        [TestCleanup]
        public void Cleanup()
        {
            Loggi.DebugEnabled = false;
            Book.DebugEnabled = false;
        }

        [TestMethod]
        public void Boolean項目が読み込めること()
        {
            FixtureBook.Expect((List<Data> list) =>
            {
                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(list[0].Bool1);
                Assert.IsFalse(list[1].Bool1);
                Assert.IsFalse(list[2].Bool1);
                Assert.IsTrue(list[3].Bool1);
            });
        }

        [TestMethod]
        public void 数値項目が読み込めること()
        {
            FixtureBook.Expect((List<Data> list) =>
            {
                Assert.AreEqual(4, list.Count);
                Assert.AreEqual((decimal)1000.00, list[0].Decimal1);
                Assert.AreEqual((decimal)1000.01, list[1].Decimal1);
                Assert.AreEqual((decimal)1000.02, list[2].Decimal1);
                Assert.AreEqual((decimal)1000.03, list[3].Decimal1);
            });
        }

        [TestMethod]
        public void 日時項目が読み込めること()
        {
            FixtureBook.Expect((List<Data> list) =>
            {
                Assert.AreEqual(6, list.Count);
                Assert.AreEqual(new DateTime(2013, 12, 11, 0, 1, 1), list[0].DateTime1);
                Assert.AreEqual(new DateTime(2013, 12, 11, 0, 1, 2), list[1].DateTime1);
                Assert.AreEqual(new DateTime(2013, 12, 11, 0, 1, 3), list[2].DateTime1);
                Assert.AreEqual(new DateTime(2013, 12, 11, 0, 1, 4), list[3].DateTime1);
                Assert.AreEqual(new DateTime(2013, 12, 11, 0, 1, 5), list[4].DateTime1);
                Assert.AreEqual(new DateTime(2013, 12, 11, 0, 1, 6), list[5].DateTime1);
            });
        }

        public class Data
        {
            public bool Bool1 { get; set; }
            public decimal Decimal1 { get; set; }
            public DateTime DateTime1 { get; set; }
        }
    }
}
