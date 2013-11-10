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
using XPFriend.Fixture;
using XPFriend.Junk;

namespace XPFriend.FixtureTest
{
    [TestClass]
    public class TestTargetClassExampleTest
    {
        [TestMethod]
        public void Save__引数なしのExpectはテストクラス名とテストカテゴリからテスト対象メソッドを類推して実行する() 
        {
            FixtureBook.Expect();
	    }

        [TestMethod]
        public void GetEmployees__引数なしのExpectReturnはテストクラス名とテストカテゴリからテスト対象メソッドを類推して実行する() 
        {
            FixtureBook.ExpectReturn();
        }

        [TestMethod]
        public void Delete__引数なしのExpectThrownはテストクラス名とテストカテゴリからテスト対象メソッドを類推して実行する() 
        {
            FixtureBook.ExpectThrown();
	    }

        [TestMethod]
        public void Xxxx__引数なしのExpectは類推したテスト対象メソッドが存在しない場合は例外をスローする()
        {
            try
            {
                // when
                FixtureBook.Expect();
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
                Assert.AreEqual("M_Fixture_FixtureBook_GetDefaultTargetMethod", e.ResourceKey);
            }
        }
    }
}
