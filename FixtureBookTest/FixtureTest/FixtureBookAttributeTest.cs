using System;
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
        public void テストメソッドでFixtureBookのパスを上書きできること()
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
