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
using System.Data;
using System.Collections.Generic;
using XPFriend.Fixture.Cast;
using XPFriend.Fixture.Role;
using XPFriend.Junk;

namespace XPFriend.Fixture.Staff
{
    /// <summary>
    /// テストケース定義。
    /// </summary>
    internal class Case
    {
        /// <summary>
        /// 名前のついていないテストケースの名前。
        /// </summary>
        public static readonly string Anonymous = " ";

        private Sheet sheet;
        private String caseName;
        private DressingRoom dressingRoom;
        private Section[] sections = new Section[Section.MaxNumber + 1];

        /// <summary>
        /// テストケース定義を作成する。
        /// </summary>
        /// <param name="sheet">このテストケースが属すシート</param>
        /// <param name="caseName">テストケース名</param>
        /// <param name="director">ディレクタ</param>
        internal Case(Sheet sheet, string caseName, Director director)
        {
            this.sheet = sheet;
            this.caseName = caseName;
            this.dressingRoom = director.AssignActors(this);
        }

        /// <summary>
        /// テストケース名。
        /// </summary>
        public string Name { get { return caseName; } }

        /// <summary>
        /// このテストケースが属すシート。
        /// </summary>
        public Sheet Sheet { get { return sheet; } }

        internal Section CreateSection(string sectionName) 
        {
            Section temp = new Section(this, sectionName);
            int number = temp.Number;
            Section section = sections[number];
            if (section == null)
            {
                sections[number] = temp;
                section = temp;
            }
            return section;
        }

        /// <summary>
        /// 指定された名前のセクションを取得する。
        /// </summary>
        /// <param name="sectionName">セクション名</param>
        /// <returns>セクション</returns>
        public Section GetSection(string sectionName)
        {
            return GetSection(new Section(this, sectionName).Number);
        }

        /// <summary>
        /// 指定されたタイプのセクションを取得する。
        /// </summary>
        /// <param name="sectionType">セクションタイプ</param>
        /// <returns>セクション</returns>
        public Section GetSection(Section.SectionType sectionType)
        {
            return GetSection((int)sectionType);
        }

        /// <summary>
        /// 指定された番号のセクションを取得する。
        /// </summary>
        /// <param name="sectionNumber">セクション番号</param>
        /// <returns>セクション</returns>
        internal Section GetSection(int sectionNumber)
        {
            if (sectionNumber >= sections.Length || sectionNumber < 0)
            {
                throw new ConfigException("M_Fixture_Case_GetSection", sectionNumber, this);
            }
            return sections[sectionNumber];
        }

        /// <summary>
        /// オブジェクトを生成する。
        /// </summary>
        /// <typeparam name="T">生成するオブジェクトのクラス</typeparam>
        /// <param name="typeName">定義名</param>
        /// <returns>生成されたオブジェクト</returns>
        public T GetObject<T>(params string[] typeName)
        {
            foreach (IObjectFactory actor in dressingRoom.ObjectFactories)
            {
                if (actor.HasRole<T>(typeName))
                {
                    return actor.GetObject<T>(typeName);
                }
            }
            throw new ConfigException("M_Fixture_Case_GetObject", this);
        }

        /// <summary>
        /// オブジェクトのリストを生成する。
        /// </summary>
        /// <typeparam name="T">生成するリスト要素オブジェクトクラス</typeparam>
        /// <param name="typeName">定義名</param>
        /// <returns>生成されたリスト</returns>
        public List<T> GetList<T>(string typeName)
        {
            foreach (IObjectFactory actor in dressingRoom.ObjectFactories)
            {
                if (actor.HasRole<T>(typeName))
                {
                    return actor.GetList<T>(typeName);
                }
            }
            throw new ConfigException("M_Fixture_Case_GetList", this);
        }

        /// <summary>
        /// オブジェクトの配列を生成する。
        /// </summary>
        /// <typeparam name="T">生成する配列要素オブジェクトクラス</typeparam>
        /// <param name="typeName">定義名</param>
        /// <returns>生成された配列</returns>
        public T[] GetArray<T>(string typeName)
        {
            foreach (IObjectFactory actor in dressingRoom.ObjectFactories)
            {
                if (actor.HasRole<T>(typeName))
                {
                    return actor.GetArray<T>(typeName);
                }
            }
            throw new ConfigException("M_Fixture_Case_GetArray", this);
        }

        /// <summary>
        /// 現在のストレージ状態が予想結果と適合するかどうかを調べる。
        /// 予想結果と適合しない場合は例外がスローされる。
        /// </summary>
        public void ValidateStorage()
        {
            bool validated = false;
            foreach (IStorageValidator actor in dressingRoom.StorageValidators)
            {
                if (actor.HasRole())
                {
                    validated = true;
                    actor.Validate();
                }
            }
            if (!validated)
            {
                throw new ConfigException("M_Fixture_Case_Validate_Storage", this);
            }
        }

        /// <summary>
        /// 指定されたオブジェクトが予想結果と適合するかどうかを調べる。
        /// 予想結果と適合しない場合は例外がスローされる。
        /// </summary>
        /// <param name="obj">調べるオブジェクト</param>
        /// <param name="typeName">定義名</param>
        public void Validate(object obj, params string[] typeName) 
        {
            bool validated = false;
            foreach (IObjectValidator actor in dressingRoom.ObjectValidators)
            {
                if (actor.HasRole(obj, typeName))
                {
                    validated = true;
                    actor.Validate(obj, typeName);
                }
            }
            if (!validated)
            {
                throw new ConfigException("M_Fixture_Case_Validate_Object", this);
            }
        }

        /// <summary>
        /// ストレージに対する更新を行う。
        /// </summary>
        public void Setup()
        {
            foreach (IStorageUpdater actor in dressingRoom.StorageUpdaters)
            {
                if (actor.HasRole())
                {
                    actor.Setup();
                }
            }
        }

        public override string ToString()
        {
            return sheet.ToString() + "[" + Name + "]";
        }

        public void Validate<TException>(Action action, string typeName) where TException : Exception
        {
            bool validated = false;
            foreach (IObjectValidator actor in dressingRoom.ObjectValidators)
            {
                if (actor.HasRole(action, typeName))
                {
                    validated = true;
                    actor.Validate<TException>(action, typeName);
                }
            }
            if (!validated)
            {
                throw new ConfigException("M_Fixture_Case_Validate_Object", this);
            }
        }
    }
}
