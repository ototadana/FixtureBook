using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture.Cast.Temp;
using XPFriend.Fixture;
using XPFriend.Junk;
using System.Collections.Generic;

namespace XPFriend.FixtureTest.Cast.Temp
{
    [TestClass]
    public class DatabaseConnectionTest
    {
        [TestMethod]
        public void 間違った名前を指定するとConfigExceptionが発生する()
        {
            try
            {
                // when
                using (DatabaseConnection connection = new DatabaseConnection())
                {
                    connection.Use("xxx");
                    Assert.Fail("ここにはこない");
                }
            }
            catch (ConfigException e)
            {
                Console.WriteLine(e.Message);
                Assert.AreEqual("M_Fixture_Temp_DatabaseConnection_NoSuchName", e.ResourceKey);
            }
        }
    }
}
