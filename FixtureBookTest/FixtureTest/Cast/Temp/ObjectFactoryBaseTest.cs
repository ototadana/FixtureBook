using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.FixtureTest.Cast.Temp.Datas;
using XPFriend.Fixture;
using System.Collections.Generic;
using XPFriend.Junk;
using System.Collections;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class ObjectFactoryBaseTest
    {
        [TestMethod]
        public void NULLおよびEMPTYの指定ができること()
        {
            // when
            List<Data> list = new FixtureBook().GetList<Data>();

            // then
            Assert.IsNull(list[0].Text1);
            Assert.IsNull(list[1].Text1);
            Assert.AreEqual("", list[2].Text1);
        }

        [TestMethod]
        public void セル値を変換できない場合は例外が発生すること()
        {
            // when
            try
            {
                new FixtureBook().GetObject<Data>();
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                Console.WriteLine(e.Message);
                Assert.AreEqual("M_Fixture_Temp_ObjectFactory_ConvertError", e.ResourceKey);
            }
        }
    }
}
