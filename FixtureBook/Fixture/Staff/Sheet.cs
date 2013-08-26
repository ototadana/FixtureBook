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
using XPFriend.Fixture.Cast;
using XPFriend.Fixture.Role;
using XPFriend.Junk;

namespace XPFriend.Fixture.Staff
{
    /// <summary>
    /// フィクスチャを定義したシート。
    /// </summary>
    internal class Sheet
    {
        private Book book;
        private string sheetName;
        private Director director;
        private Dictionary<string, Case> caseMap = new Dictionary<string, Case>();
        private bool written;
        private bool notYet = true;

        internal Sheet(Book book, String sheetName, Director director)
        {
            this.book = book;
            this.sheetName = sheetName;
            this.director = director;
        }

        /// <summary>
        /// シートの名前。
        /// </summary>
        public string Name { get { return sheetName; } }

        /// <summary>
        /// このシートを格納しているブック。
        /// </summary>
        public Book Book { get { return book; } }

        /// <summary>
        /// ストレージに対する更新を行う。
        /// </summary>
        public void Setup()
        {
            Write(this);
            List<IStorageUpdater> storageUpdaters = director.AssignStorageUpdaters(this);
            foreach(IStorageUpdater storageUpdater in storageUpdaters) 
            {
                storageUpdater.Setup();
            }
        }

        /// <summary>
        /// セットアップをまだ一度もおこなっていない場合はセットアップ実行する。
        /// </summary>
        internal void SetupIfNotYet()
        {
            if (notYet)
            {
                Setup();
                notYet = false;
            }
        }

        /// <summary>
        /// テストケース定義を取得する。
        /// </summary>
        /// <param name="caseName">テストケース名</param>
        /// <returns>テストケース定義</returns>
        public Case GetCase(string caseName)
        {
            Write(this);
            Case testCase = null;
            caseMap.TryGetValue(caseName, out testCase);
            if (testCase == null)
            {
                throw new ConfigException("M_Fixture_Sheet_GetCase", caseName, this);
            }
            return testCase;
        }

        private void Write(Sheet sheet)
        {
            if (written)
            {
                return;
            }
            book.Author.Write(this);
            written = true;
        }

        /// <summary>
        /// テストケース定義を取得する。
        /// </summary>
        /// <param name="caseName">テストケース名</param>
        /// <returns>テストケース定義</returns>
        internal Case CreateCase(string caseName)
        {
            Case testCase = null;
            caseMap.TryGetValue(caseName, out testCase);
            if (testCase == null)
            {
                testCase = new Case(this, caseName, director);
                caseMap[caseName] = testCase;
            }
            return testCase;
        }

        public override string ToString()
        {
            return book.ToString() + "#" + Name;
        }
    }
}
