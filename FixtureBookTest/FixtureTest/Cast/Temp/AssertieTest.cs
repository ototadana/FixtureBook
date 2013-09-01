using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture.Cast.Temp;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class AssertieTest
    {
        [TestMethod]
        public void AreEqual_stringは文字列が同じかどうかを調べる()
        {
            // expect
            Assertie.AreEqual("a", "a", "xxxx");

            try
            {
                // when
                Assertie.AreEqual("a", "b", "xxxx");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("xxxx") > -1);
            }
        }

        [TestMethod]
        public void AreEqual_intは数値が同じかどうかを調べる()
        {
            // expect
            Assertie.AreEqual(1, 1, "xxxx");

            try
            {
                // when
                Assertie.AreEqual(1, 2, "xxxx");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("xxxx") > -1);
            }
        }

        [TestMethod]
        public void AreEqual_objectはオブジェクトが同じかどうかを調べる()
        {
            // expect
            Assertie.AreEqual(1L, 1L, "xxxx");

            try
            {
                // when
                Assertie.AreEqual(1L, 2L, "xxxx");
            }
            catch (AssertFailedException e)
            {
                // then
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.IndexOf("xxxx") > -1);
            }
        }
    }
}
