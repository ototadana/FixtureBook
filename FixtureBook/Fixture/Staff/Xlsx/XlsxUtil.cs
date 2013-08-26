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
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace XPFriend.Fixture.Staff.Xlsx
{
    delegate void HandleNode(XmlTextReader reader);

    /// <summary>
    /// .xlsx ファイルを解釈する際に使用するユーティリティメソッド集。
    /// </summary>
    internal sealed class XlsxUtil
    {
        private class Format
        {
            public readonly string text;
            public readonly bool date;
            public Format(string text, bool date)
            {
                this.text = text;
                this.date = date;
            }
        }

        private static readonly Format[] formats = new Format[0x32];

        static XlsxUtil() {
            formats[0x0] = new Format("General", false);
            formats[0x1] = new Format("0", false);
            formats[0x2] = new Format("0.00", false);
            formats[0x3] = new Format("#,##0", false);
            formats[0x4] = new Format("#,##0.00", false);
            formats[0x5] = new Format("$#,##0_($#,##0)", false);
            formats[0x6] = new Format("$#,##0_[Red]($#,##0)", false);
            formats[0x7] = new Format("$#,##0.00_($#,##0.00)", false);
            formats[0x8] = new Format("$#,##0.00_[Red]($#,##0.00)", false);
            formats[0x9] = new Format("0%", false);
            formats[0xa] = new Format("0.00%", false);
            formats[0xb] = new Format("0.00E+00", false);
            formats[0xc] = new Format("# ?/?", false);
            formats[0xd] = new Format("# ??/??", false);
            formats[0xe] = new Format("m/d/yy", true);
            formats[0xf] = new Format("d-mmm-yy", true);
            formats[0x10] = new Format("d-mmm", true);
            formats[0x11] = new Format("mmm-yy", true);
            formats[0x12] = new Format("h:mm AM/PM", true);
            formats[0x13] = new Format("h:mm:ss AM/PM", true);
            formats[0x14] = new Format("h:mm", true);
            formats[0x15] = new Format("h:mm:ss", true);
            formats[0x16] = new Format("m/d/yy h:mm", true);
            formats[0x25] = new Format("#,##0_(#,##0)", false);
            formats[0x26] = new Format("#,##0_[Red](#,##0)", false);
            formats[0x27] = new Format("#,##0.00_(#,##0.00)", false);
            formats[0x28] = new Format("#,##0.00_[Red](#,##0.00)", false);
            formats[0x29] = new Format("_(*#,##0__(*(#,##0_(* \"-\"__(@_)", false);
            formats[0x2a] = new Format("_($*#,##0__($*(#,##0_($* \"-\"__(@_)", false);
            formats[0x2b] = new Format("_(*#,##0.00__(*(#,##0.00_(*\"-\"??__(@_)", false);
            formats[0x2c] = new Format("_($*#,##0.00__($*(#,##0.00_($*\"-\"??__(@_)", false);
            formats[0x2d] = new Format("mm:ss", true);
            formats[0x2e] = new Format("[h]:mm:ss", true);
            formats[0x2f] = new Format("mm:ss.0", true);
            formats[0x30] = new Format("##0.0E+0", false);
            formats[0x31] = new Format("@", false);
        }

        private static List<string> numberFormatList = new List<string>();

        /// <summary>
        /// 指定された書式番号の書式文字列を取得する。
        /// </summary>
        /// <param name="index">書式番号</param>
        /// <returns>書式文字列</returns>
        public static string Get(int index)
        {
            if (index >= formats.Length || index < 0)
            {
                return null;
            }
            return formats[index].text;
        }

        /// <summary>
        /// 指定された書式番号・書式文字列が日時書式かどうかを調べる。
        /// </summary>
        /// <param name="index">書式番号</param>
        /// <param name="text">書式文字列</param>
        /// <returns></returns>
        public static bool IsDateFormat(int index, String text)
        {
            try
            {
                if (index < formats.Length && formats[index].date)
                {
                    return true;
                }

                if (text == null)
                {
                    return false;
                }

                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    if (c == 'y' || c == 'm' || c == 'd' || c == 'h')
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 指定された書式文字列から数値文字列を取得する。
        /// </summary>
        /// <param name="formatString">書式文字列</param>
        /// <returns>数値文字列</returns>
        public static string GetNormalizedFormatString(string formatString)
        {
            int cnt = CountZero(formatString);
            if (numberFormatList.Count <= cnt)
            {
                for (int i = numberFormatList.Count; i < cnt; i++)
                {
                    numberFormatList.Add(CreateNumberFormat(i));
                }
                numberFormatList.Add(CreateNumberFormat(cnt));
            }
            return numberFormatList[cnt];
        }

        private static string CreateNumberFormat(int cnt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0");
            if (cnt > 0)
            {
                sb.Append(".");
                for (int i = 0; i < cnt; i++)
                {
                    sb.Append("0");
                }
            }
            return sb.ToString();
        }

        private static int CountZero(string formatString)
        {
            if (formatString == null)
            {
                return 0;
            }

            int dotIndex = formatString.IndexOf('.');
            if (dotIndex > -1)
            {
                int count = 0;
                for (int i = dotIndex + 1; i < formatString.Length; i++)
                {
                    if (formatString[i] != '0')
                    {
                        return count;
                    }
                    count++;
                }
                return count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 指定された数値を日時型データに変換する。
        /// </summary>
        /// <param name="date">時間を表す数値</param>
        /// <returns>日時型データ</returns>
        public static DateTime GetDateTime(double date)
        {
            return DateTime.FromOADate(date);
        }

        /// <summary>
        /// 指定されたXMLストリームの読み込みを行う。
        /// </summary>
        /// <param name="stream">XMLストリーム</param>
        /// <param name="handleStartElement">開始タグの処理</param>
        /// <param name="handleEndElement">終了タグの処理</param>
        /// <param name="handleText">タグ間テキスト処理</param>
        public static void Load(Stream stream,
            HandleNode handleStartElement = null,
            HandleNode handleEndElement = null,
            HandleNode handleText = null)
        {
            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (handleStartElement != null)
                        {
                            handleStartElement(reader);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (handleEndElement != null)
                        {
                            handleEndElement(reader);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Text || 
                        reader.NodeType == XmlNodeType.Whitespace ||
                        reader.NodeType == XmlNodeType.SignificantWhitespace)
                    {
                        if (handleText != null)
                        {
                            handleText(reader);
                        }
                    }
                }
            }
        }
    }
}
