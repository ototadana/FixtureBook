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
    /// .xlsx ファイル中の共有文字列情報（xl/sharedStrings.xml）。
    /// </summary>
    internal class XlsxSharedStrings
    {
        private List<string> stringsTable;

        private string value = "";
        private bool rPhIsOpen;
        private bool tIsOpen;

        /// <summary>
        /// <see cref="XlsxSharedStrings"/> を作成する。
        /// </summary>
        /// <param name="stream">共有文字列情報を格納したファイル（xl/sharedStrings.xml）の入力ストリーム</param>
        public XlsxSharedStrings(Stream stream)
        {
            if (stream == null)
            {
                stringsTable = new List<string>(0);
            }
            else
            {
                XlsxUtil.Load(stream, HandleStartElement, HandleEndElement, HandleText);
            }
        }

        /// <summary>
        /// 指定された文字列番号の文字列を取得する。
        /// </summary>
        /// <param name="index">文字列番号</param>
        /// <returns>文字列</returns>
        public string this[int index]
        { 
            get { return stringsTable[index]; }
        }

        internal int Count
        {
            get { return stringsTable.Count; }
        }

        private void HandleStartElement(XmlTextReader reader)
        {
            if (reader.Name == "sst")
            {
                int capacity = 16;
                string uniqueCount = reader.GetAttribute("uniqueCount");
                if (uniqueCount != null)
                {
                    capacity = int.Parse(uniqueCount);
                }
                stringsTable = new List<string>(capacity);
            }
            else if (reader.Name == "si")
            {
                value = "";
            }
            else if (reader.Name == "t")
            {
                tIsOpen = true;
            }
            else if (reader.Name == "rPh")
            {
                rPhIsOpen = true;
            }
        }

        private void HandleEndElement(XmlTextReader reader)
        {
            if (reader.Name == "si")
            {
                stringsTable.Add(value);
            }
            else if (reader.Name == "t")
            {
                tIsOpen = false;
            }
            else if (reader.Name == "rPh")
            {
                rPhIsOpen = false;
            }
        }

        private void HandleText(XmlTextReader reader)
        {
            if (tIsOpen && !rPhIsOpen)
            {
                value = reader.Value;
            }
        }
    }
}
