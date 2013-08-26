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

namespace XPFriend.Fixture.Staff
{
    /// <summary>
    /// ブックの作者。
    /// </summary>
    internal abstract class Author
    {
        /// <summary>
        /// ブックを作成する。
        /// </summary>
        /// <param name="book">作成するブック</param>
        public abstract void Write(Book book);

        /// <summary>
        /// シートを作成する。
        /// </summary>
        /// <param name="sheet">作成するシート</param>
        public abstract void Write(Sheet sheet);

        /// <summary>
        /// 指定されたブックに指定された名前でシートを作成する。
        /// </summary>
        /// <param name="book">ブック</param>
        /// <param name="sheetName">シート名</param>
        /// <returns>作成したシート</returns>
        public static Sheet CreateSheet(Book book, string sheetName)
        {
            return book.CreateSheet(sheetName);
        }

        /// <summary>
        /// 指定されたシートに指定された名前のテストケースを作成する。
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="caseName">テストケース名</param>
        /// <returns>作成したテストケース</returns>
        public static Case CreateCase(Sheet sheet, string caseName)
        {
            return sheet.CreateCase(caseName);
        }

        /// <summary>
        /// 指定されたテストケースに指定された名前のセクションを作成する。
        /// </summary>
        /// <param name="testCase">テストケース</param>
        /// <param name="sectionName">セクション名</param>
        /// <returns>作成したセクション</returns>
        public static Section CreateSection(Case testCase, string sectionName)
        {
            return testCase.CreateSection(sectionName);
        }

        /// <summary>
        /// 指定されたセクションに指定された名前のテーブルを作成する。
        /// </summary>
        /// <param name="section">セクション</param>
        /// <param name="tableName">テーブル名</param>
        /// <returns>作成したテーブル</returns>
        public static Table CreateTable(Section section, string tableName)
        {
            return section.CreateTable(tableName);
        }
    }
}
