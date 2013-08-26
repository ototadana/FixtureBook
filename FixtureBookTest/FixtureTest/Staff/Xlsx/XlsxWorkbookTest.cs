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
    public class XlsxWorkbookTest
    {
        [TestMethod]
        public void SheetNamesはworkbookに格納されるシート名のリストを取得する()
        {
            // setup
            using (FileStream stream = new FileStream(getWorkbookPath(), FileMode.Open, FileAccess.Read))
            {
                XlsxWorkbook workbook = new XlsxWorkbook(stream);

                // expect
                Assert.AreEqual(2, workbook.SheetNames.Count);
                Assert.AreEqual("test1", workbook.SheetNames[0]);
                Assert.AreEqual("テスト2", workbook.SheetNames[1]);
            }
        }

        [TestMethod]
        public void GetFileNameは引数で指定された名前のシートの内容を格納するファイル名を取得する()
        {
            // setup
            using (FileStream stream = new FileStream(getWorkbookPath(), FileMode.Open, FileAccess.Read))
            {
                XlsxWorkbook workbook = new XlsxWorkbook(stream);

                // expect
                Assert.AreEqual("xl/worksheets/sheet1.xml", workbook.GetFileName("test1"));
                Assert.AreEqual("xl/worksheets/sheet2.xml", workbook.GetFileName("テスト2"));
            }
        }

        private static string getWorkbookPath()
        {
            return XlsxTestUtil.GetXMLFilePath("workbook.xml");
        }
    }
}
