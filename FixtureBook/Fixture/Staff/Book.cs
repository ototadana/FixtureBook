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
using XPFriend.Junk;

namespace XPFriend.Fixture.Staff
{
    /// <summary>
    /// フィクスチャブック。
    /// フィクスチャを定義したシートの集まり。
    /// </summary>
    internal class Book
    {
        private static Dictionary<string, Book> bookMap = new Dictionary<string, Book>();
        private string filePath;
        private Director director;
        private Author author;
        private Dictionary<string, Sheet> sheetMap = new Dictionary<string, Sheet>();

        /// <summary>
        /// 指定されたファイルを読み込み、ブックのインスタンスを取得する。
        /// 
        /// 一度読み込んだファイル内容はキャッシュに蓄えられるため、
        /// 同一のテストクラスとファイルパスファイルを指定して
        /// GetInstance を指定した場合、二度目以降はファイルの読み込みは行われない。
        /// </summary>
        /// <param name="testClass">テストクラス</param>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <returns>ブックのインスタンス</returns>
        public static Book GetInstance(Type testClass, string filePath)
        {
            return GetInstance(testClass, filePath, new Director());
        }

        public static Book GetInstance(string filePath)
        {
            return GetInstance(typeof(Book), filePath, new Director());
        }

        /// <summary>
        /// 指定されたファイルを読み込み、ブックのインスタンスを取得する。
        /// 
        /// 一度読み込んだファイル内容はキャッシュに蓄えられるため、
        /// 同一のテストクラスとファイルパスファイルを指定して
        /// GetInstance を指定した場合、二度目以降はファイルの読み込みは行われない。
        /// </summary>
        /// <param name="testClass">テストクラス</param>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <param name="director">ディレクタ</param>
        /// <returns>Book のインスタンス</returns>
        public static Book GetInstance(Type testClass, string filePath, Director director)
        {
            string canonicalPath = Path.GetFullPath(filePath);
            string id = testClass.FullName + "@" + canonicalPath;
            lock (bookMap)
            {
                Book book = null;
                bookMap.TryGetValue(id, out book);
                if (book == null)
                {
                    book = new Book(canonicalPath, director);
                    bookMap[id] = book;
                }
                return book;
            }
        }

        /// <summary>
        /// ブックインスタンスのキャッシュをクリアする。
        /// </summary>
        public static void ClearCache()
        {
            lock (bookMap)
            {
                bookMap.Clear();
            }
        }

        /// <summary>
        /// デバッグ出力が有効かどうか。
        /// </summary>
        public static bool DebugEnabled { get; set;}

        /// <summary>
        /// 指定されたファイルを読み込みブックを作成する。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="director">ディレクタ</param>
        private Book(string filePath, Director director)
        {
            this.filePath = filePath;
            this.director = director;
            Author author = director.AssignAuthor(this);
            author.Write(this);
            this.author = author;
        }

        /// <summary>
        /// 現在のブックのファイルパス。
        /// </summary>
        public string FilePath { get { return filePath; } }

        /// <summary>
        /// 指定された名前のシートを取得する。
        /// </summary>
        /// <param name="sheetName">シート名</param>
        /// <returns>シート</returns>
        public Sheet GetSheet(string sheetName)
        {
            string key = ToMapKey(sheetName);
            Sheet sheet = null;
            sheetMap.TryGetValue(key, out sheet);
            if (sheet == null)
            {
                throw new ConfigException("M_Fixture_Book_GetSheet", sheetName, this);
            }
            return sheet;
        }

        internal Sheet CreateSheet(string sheetName)
        {
            string key = ToMapKey(sheetName);
            lock (sheetMap)
            {
                Sheet sheet = new Sheet(this, sheetName, director);
                sheetMap[key] = sheet;
                return sheet;
            }
        }

        private string ToMapKey(string sheetName)
        {
            return sheetName.ToLower();
        }

        internal Author Author { get { return author; } }

        public override string ToString()
        {
            return Path.GetFileName(filePath);
        }
    }
}