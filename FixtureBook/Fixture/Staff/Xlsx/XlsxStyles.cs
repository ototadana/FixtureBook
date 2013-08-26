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
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace XPFriend.Fixture.Staff.Xlsx
{
    /// <summary>
    /// .xlsx ファイル中の書式情報（xl/styles.xml）。
    /// </summary>
    internal class XlsxStyles
    {
        private List<string> formatString = new List<string>();
        private List<int> formatId = new List<int>();

        private Dictionary<string, string> numFmts = new Dictionary<string, string>();
        private bool cellXfsIsOpen = false;

        /// <summary>
        /// <see cref="XlsxStyles"/> を作成する。
        /// </summary>
        /// <param name="stream">.xlsx ファイル中の書式情報を格納したファイル（xl/styles.xml）の入力ストリーム</param>
        public XlsxStyles(Stream stream)
        {
            XlsxUtil.Load(stream, HandleStartElement, HandleEndElement);
        }

        /// <summary>
        /// 指定した番号の書式文字列を取得する。
        /// </summary>
        /// <param name="index">書式番号</param>
        /// <returns>書式文字列</returns>
        public string GetFormatString(int index)
        {
            return formatString[index];
        }

        /// <summary>
        /// 指定した番号の書式IDを取得する。
        /// </summary>
        /// <param name="index">書式番号</param>
        /// <returns>書式ID</returns>
        public int GetFormatId(int index)
        {
            return formatId[index];
        }

        private void HandleStartElement(XmlTextReader reader)
        {
            if (reader.Name == "cellXfs")
            {
                cellXfsIsOpen = true;
            }
            else if (reader.Name == "numFmt")
            {
                numFmts[reader.GetAttribute("numFmtId")] = reader.GetAttribute("formatCode");
            }
            else if (cellXfsIsOpen && reader.Name == "xf")
            {
                string numFmtId = reader.GetAttribute("numFmtId");
                int id = int.Parse(numFmtId);
                String format = XlsxUtil.Get(id);
                if (format == null)
                {
                    format = numFmts[numFmtId];
                }
                formatString.Add(format);
                formatId.Add(id);
            }
        }

        private void HandleEndElement(XmlTextReader reader)
        {
            if (reader.Name == "cellXfs")
            {
                cellXfsIsOpen = false;
            }
        }
    }
}
