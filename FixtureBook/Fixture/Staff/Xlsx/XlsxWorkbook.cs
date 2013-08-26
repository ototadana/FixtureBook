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
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace XPFriend.Fixture.Staff.Xlsx
{
    /// <summary>
    /// .xlsx ファイル中のワークブック情報（xl/wookbook.xml）。
    /// </summary>
    internal class XlsxWorkbook
    {
        private Dictionary<string, string> sheetNameMap = new Dictionary<string,string>();
        private List<string> sheetNames = new List<string>();

        /// <summary>
        /// <see cref="XlsxWorkbook"/> を作成する。
        /// </summary>
        /// <param name="stream">.xlsx ファイル中のワークブック情報を格納したファイル（xl/wookbook.xml）の入力ストリーム</param>
        public XlsxWorkbook(Stream stream)
        {
            XlsxUtil.Load(stream, HandleStartElement);
        }

        /// <summary>
        /// シート名のリストを取得する。
        /// </summary>
        public List<string> SheetNames { get { return sheetNames; } }

        /// <summary>
        /// 指定したシート名に対応するファイル名を取得する。
        /// </summary>
        /// <param name="sheetName">シート名</param>
        /// <returns>ファイル名</returns>
        public string GetFileName(string sheetName)
        {
            return sheetNameMap[sheetName];
        }

        private void HandleStartElement(XmlTextReader reader)
        {
            if (reader.Name == "sheet")
            {
                string name = reader.GetAttribute("name");
                string rid = reader.GetAttribute("r:id");
                string sheetName = "xl/worksheets/sheet" + rid.Substring(3) + ".xml";
                sheetNameMap[name] = sheetName;
                sheetNames.Add(name);
            }
        }
        
    }
}
