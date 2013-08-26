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
using XPFriend.Junk;

namespace XPFriend.JunkTest
{
    [TestClass]
    public class ConviTest
    {
        [TestMethod]
        public void ToStringにnullを渡すと空文字列を返す()
        {
            // expect
            Assert.AreEqual("", Convi.ToString(null));
        }

        [TestMethod]
        public void ToStringに文字列を渡すとそのまま返す()
        {
            // setup
            string s = "aaa";

            // expect
            Assert.AreSame(s, Convi.ToString(s));
        }

        [TestMethod]
        public void ToStringに日時を渡すとデフォルト書式で文字列変換する()
        {
            // setup
            DateTime dateTime = new DateTime(2013, 12, 31, 1, 1, 2);

            // expect
            Assert.AreEqual("2013-12-31 01:01:02", Convi.ToString(dateTime));
        }

        [TestMethod]
        public void ToStringにListを渡すとバーティカルバー区切りの文字列を返す()
        {
            // setup
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            // expect
            Assert.AreEqual("1|2|3", Convi.ToString(list));
        }

        [TestMethod]
        public void ToStringに配列を渡すとバーティカルバー区切りの文字列を返す()
        {
            // setup
            int[] array = {1, 2, 3};

            // expect
            Assert.AreEqual("1|2|3", Convi.ToString(array));
        }

        [TestMethod]
        public void ToStringにその他のオブジェクトを渡すとtoStringして返す()
        {
            // expect
            Assert.AreEqual("object...", Convi.ToString(new TestObject()));
        }

        private class TestObject
        {
            public override string ToString()
            {
                return "object...";
            }
        }
    }
}
