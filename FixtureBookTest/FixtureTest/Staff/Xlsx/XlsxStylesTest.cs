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
    public class XlsxStylesTest
    {
        [TestMethod]
        public void GetFormatIdは指定したインデックスの書式IDを取得できる()
        {
            // setup
            using (FileStream stream = new FileStream(getStylesPath(), FileMode.Open, FileAccess.Read))
            {
                XlsxStyles styles = new XlsxStyles(stream);

                //expect
                Assert.AreEqual(0, styles.GetFormatId(0));
                Assert.AreEqual(14, styles.GetFormatId(1));
                Assert.AreEqual(177, styles.GetFormatId(2));
            }
        }

        [TestMethod]
        public void GetFormatStringは指定したインデックスの書式を取得できる()
        {
            // setup
            using (FileStream stream = new FileStream(getStylesPath(), FileMode.Open, FileAccess.Read))
            {
                XlsxStyles styles = new XlsxStyles(stream);

                //expect
                Assert.AreEqual("General", styles.GetFormatString(0));
                Assert.AreEqual("m/d/yy", styles.GetFormatString(1));
                Assert.AreEqual("#,##0.0000_ ", styles.GetFormatString(2));
            }
        }

        private static string getStylesPath()
        {
            return XlsxTestUtil.GetXMLFilePath("styles.xml");
        }
    }
}
