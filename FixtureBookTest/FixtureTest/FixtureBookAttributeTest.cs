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
using XPFriend.Junk;

namespace XPFriend.FixtureTest
{
    [TestClass]
    [FixtureBook]
    public class FixtureBookAttributeTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            Loggi.DebugEnabled = false;
        }

        [TestMethod]
        public void 明示的にパス指定をしなくても命名規約によりFixtureBookのパスを判断し読み込みができること()
        {
            // expect
            Assert.AreEqual("ABC", new FixtureBook().GetObject<FixtureBookAttributeTestData>().Text);
        }

        [TestMethod]
        [FixtureBook(@"FixtureTest\FixtureBookAttributeTest_02.xlsx")]
        private void テストメソッドでFixtureBookのパスを上書きできること()
        {
            // expect
            Assert.AreEqual("DEF", new FixtureBook().GetObject<FixtureBookAttributeTestData>().Text);
        }

        [TestMethod]
        [FixtureBook(@"..\..\FixtureTest\FixtureBookAttributeTest_02.xlsx")]
        [Fixture("FixtureBookAttributeTest", "テストメソッドでFixtureBookのパスを上書きできること")]
        public void binフォルダからの相対パス指定ができること()
        {
            // expect
            Assert.AreEqual("DEF", new FixtureBook().GetObject<FixtureBookAttributeTestData>().Text);
        }

    }

    public class FixtureBookAttributeTestData
    {
        public string Text { get; set; }
    }
}
