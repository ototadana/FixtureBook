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
using System.IO;
using System.Text;
using System.Xml;
using XPFriend.Junk;

namespace XPFriend.Fixture.Staff.Xlsx
{
    /// <summary>
    /// .xlsx 中のワークシート（xl/worksheets/sheet*.xml）処理。
    /// </summary>
    internal class XlsxWorksheetHandler
    {
        enum DataType { Boolean, SstString, Number }

        private XlsxStyles styles;
        private XlsxSharedStrings sharedStrings;
        private XlsxCellParser cellParser;
        private string value;

        private bool vIsOpen;
        private DataType dataType;

        private string formatString;
        private int formatIndex;
        private string cellRef;

        /// <summary>
        /// <see cref="XlsxWorksheetHandler"/> を作成する。
        /// </summary>
        /// <param name="styles">書式情報</param>
        /// <param name="sharedStrings">共有文字列情報</param>
        /// <param name="xlsxCellParser">セル処理</param>
        public XlsxWorksheetHandler(XlsxStyles styles, XlsxSharedStrings sharedStrings, XlsxCellParser xlsxCellParser)
        {
            this.styles = styles;
            this.sharedStrings = sharedStrings;
            this.cellParser = xlsxCellParser;
        }

        internal void Parse(Stream stream)
        {
            XlsxUtil.Load(stream, HandleStartElement, HandleEndElement, handleText);
        }

        private void HandleStartElement(XmlTextReader reader)
        {
            string name = reader.Name;
            if (IsTextTag(name)) 
            {
                vIsOpen = true;
                value = "";
            }
            else if(name == "c")
            {
                cellRef = reader.GetAttribute("r");
                string cellType = reader.GetAttribute("t");
                if (cellType == "b")
                {
                    dataType = DataType.Boolean;
                }
                else if (cellType == "s")
                {
                    dataType = DataType.SstString;
                }
                else
                {
                    dataType = DataType.Number;
                }

                string cellStyle = reader.GetAttribute("s");
                if (cellStyle != null)
                {
                    int index = int.Parse(cellStyle);
                    formatString = styles.GetFormatString(index);
                    formatIndex = styles.GetFormatId(index);
                }
                else
                {
                    formatString = null;
                    formatIndex = -1;
                }

		    } 
        }

        private bool IsTextTag(string name)
        {
            return name == "v" || name == "t";
        }

        private void HandleEndElement(XmlTextReader reader)
        {
            if (IsTextTag(reader.Name))
            {
                vIsOpen = false;
                output(cellRef, getValue());
            }
        }

        private void output(string cellReference, string formattedValue) 
        {
		    if (cellParser.DebugEnabled) {
                Loggi.Debug("[" + cellReference + "]" + formattedValue);                
		    }
            cellParser.Parse(ToRow(cellReference), ToColumn(cellReference), formattedValue);
	    }

        private int ToRow(string reference)
        {
            StringBuilder sb = new StringBuilder(reference.Length - 1);
            for (int i = 0; i < reference.Length; i++)
            {
                char c = reference[i];
                if (c >= '0' && c <= '9')
                {
                    sb.Append(c);
                }
            }
            return int.Parse(sb.ToString());
        }

        private int ToColumn(string reference)
        {
            int position = 0;
            int value = 0;
            for (int i = reference.Length - 1; i >= 0; i--)
            {
                char c = reference[i];
                if (c >= 'A' && c <= 'Z')
                {
                    int shift = (int)Math.Pow(26, position);
                    value += (c - 'A' + 1) * shift;
                    position++;
                }
            }
            return value - 1;
        }
         
        private string getValue()
        {
            if (dataType == DataType.Boolean)
            {
                return (value[0] == '0')? "false" : "true";
            }
            else if (dataType == DataType.SstString)
            {
                int index = int.Parse(value.ToString());
                return sharedStrings[index];
            }
            else
            {
                string s = value.ToString();
                if (formatString != null)
                {
                    try
                    {
                        return Format(double.Parse(s));
                    }
                    catch (Exception)
                    {
                        return s;
                    }
                }
                else
                {
                    return s;
                }
            }
        }

        private string Format(double value)
        {
            if (XlsxUtil.IsDateFormat(formatIndex, formatString))
            {
                return Convi.ToString(XlsxUtil.GetDateTime(value));
            }
            else
            {
                string normalizedFormatString = XlsxUtil.GetNormalizedFormatString(formatString);
                return value.ToString(normalizedFormatString);
            }
        }

        private void handleText(XmlTextReader reader)
        {
            if (vIsOpen)
            {
                value = reader.Value;
            }
        }
    }
}
