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
using XPFriend.Fixture.Staff;

namespace XPFriend.Fixture.Staff.Xlsx
{
    /// <summary>
    /// .xlsx 形式ファイルからブックを作成する <see cref="Author"/>
    /// </summary>
    internal class XlsxAuthor : Author
    {
        private XlsxFile xlsxFile;
        private Parser parser;

        public override void Write(Book book)
        {
            parser = new Parser(Book.DebugEnabled);
            xlsxFile = new XlsxFile(book.FilePath, parser);
            foreach (string sheetName in xlsxFile.WorksheetNames)
            {
                CreateSheet(book, sheetName);
            }
        }

        public override void Write(Sheet sheet)
        {
            parser.ChangeSheet(sheet);
            xlsxFile.Read(sheet.Name);
        }
    }

    class Parser : XlsxCellParser
    {
        private Sheet sheet;
        private Case testCase;
        private Section section;
        private Table table;
        private List<Column> columns;
        private int columnNameRowIndex;
        private int columnValueRowIndex;
        private Dictionary<string, string> values;

        protected internal Parser(bool debugEnabled) : base(debugEnabled) {}

        internal override void Parse(int rowIndex, int columnIndex, string value)
        {
            if (columnIndex == 0)
            {
                return;
            }

            if (columnIndex == 1)
            {
                ChangeSection(value);
                return;
            }

            if (columnIndex == 2)
            {
                if(value.StartsWith("[") || IsTestCaseSection()) 
                {
                    ChangeCase(GetCaseName(value));
                }
                else if (IsFlag(value) && columns != null)
                {
                    UpdateTable(rowIndex, columnIndex, value);
                }
                else
                {
                    ChangeTable(value);
                }
                return;
            }
            UpdateTable(rowIndex, columnIndex, value);
        }

        private bool IsTestCaseSection()
        {
            return section != null && section.Number == 1 && !section.Name.StartsWith("1");
        }

        private void UpdateTable(int rowIndex, int columnIndex, string value)
        {
            if (table == null)
            {
                return;
            }

            if (columns == null)
            {
                columnNameRowIndex = rowIndex;
                columnValueRowIndex = -1;
                columns = table.Columns;
            }

            if (columnNameRowIndex == rowIndex)
            {
                AddColumnName(columnIndex, value);
                return;
            }

            if (columnValueRowIndex != rowIndex)
            {
                columnValueRowIndex = rowIndex;
                bool deleted = columnIndex == 2 && IsDeleteFlag(value);
                Row row = table.AddRow(rowIndex, deleted);
                values = row.Values;
            }

            String columnName = GetColumnName(columnIndex);
            if (columnName == null)
            {
                return;
            }
            values[columnName] = value;
        }

        private bool IsFlag(string value)
        {
            return value.Length == 1;
        }

        private bool IsDeleteFlag(string value)
        {
            return "D" == value;
        }

        private string GetColumnName(int columnIndex)
        {
            if (columnIndex >= columns.Count)
            {
                return null;
            }
            Column column = columns[columnIndex];
            if (column == null)
            {
                return null;
            }
            return column.Name;
        }

        private void AddColumnName(int columnIndex, string value)
        {
            for (int i = columns.Count; i < columnIndex + 1; i++)
            {
                columns.Add(null);
            }
            columns[columnIndex] = CreateColumn(value);
        }

        internal static Column CreateColumn(string text)
        {
            int colonIndex = text.IndexOf(':');
            if (colonIndex == -1)
            {
                return new Column(text, null, null);
            }
            int typeStartIndex = colonIndex + 1;

            String value = text.Substring(0, colonIndex);
            int bracketIndex = text.IndexOf('[', colonIndex);
            if (bracketIndex > -1)
            {
                return new Column(value, null, text.Substring(typeStartIndex, bracketIndex - typeStartIndex));
            }

            int ltIndex = text.IndexOf('<', colonIndex);
            if (ltIndex == -1)
            {
                return new Column(value, text.Substring(colonIndex + 1), null);
            }
            return CreateListTypeColumn(text, typeStartIndex, value, ltIndex);
        }

        private static Column CreateListTypeColumn(string text, int typeStartIndex, String value, int ltIndex)
        {
            int componentTypeStartIndex = ltIndex + 1;

            String type = text.Substring(typeStartIndex, ltIndex - typeStartIndex);
            int gtIndex = text.IndexOf('>', ltIndex);
            if (gtIndex == -1)
            {
                return new Column(value, type, text.Substring(componentTypeStartIndex));
            }
            else
            {
                return new Column(value, type, text.Substring(componentTypeStartIndex, gtIndex - componentTypeStartIndex));
            }
        }

        private string GetCaseName(string value)
        {
            return value.Trim('[', ']');
        }

        public void ChangeSheet(Sheet sheet)
        {
            this.sheet = sheet;
            this.section = null;
            ChangeCase(Case.Anonymous);
            ChangeTable((Table)null);
        }

        private void ChangeCase(string value)
        {
            this.testCase = Author.CreateCase(this.sheet, value);
            ChangeTable((Table)null);
        }

        private void ChangeSection(string value)
        {
            this.section = Author.CreateSection(this.testCase, value);
            ChangeTable((Table)null);
        }

        private void ChangeTable(string value)
        {
            ChangeTable(Author.CreateTable(this.section, value));
        }

        private void ChangeTable(Table table)
        {
            this.table = table;
            this.columns = null;
            this.columnNameRowIndex = -1;
            this.columnValueRowIndex = -1;
            this.values = null;
        }
    }
}
