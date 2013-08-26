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
using System.IO;
using XPFriend.Fixture.Staff.Xlsx;

namespace XPFriend.FixtureTest.Staff.Xlsx
{
    [TestClass]
    public class XlsxSharedStringsTest
    {
        [TestMethod]
        public void コンストラクタは指定されたstreamから共有文字列を読み込む()
        {
            // setup
            using (FileStream stream = new FileStream(GetSharedStringsPath(), FileMode.Open, FileAccess.Read))
            {
                XlsxSharedStrings strings = new XlsxSharedStrings(stream);

                // expect
                Assert.AreEqual(6, strings.Count);
                Assert.AreEqual("aiueo", strings[0]);
                Assert.AreEqual("A1: aiueo", strings[1]);
                Assert.AreEqual("B1: あいうえお", strings[2]);
                Assert.AreEqual("A2: kakiku", strings[3]);
                Assert.AreEqual("B2: かきく", strings[4]);
                Assert.AreEqual("B3:今日は", strings[5]);
            }
        }

        [TestMethod]
        public void コンストラクタにnullが指定されると何も読み込まない()
        {
            // setup
            XlsxSharedStrings strings = new XlsxSharedStrings(null);

            // expect
            Assert.AreEqual(0, strings.Count);
        }

        private static string GetSharedStringsPath()
        {
            return XlsxTestUtil.GetXMLFilePath("sharedStrings.xml");
        }
    }
}
