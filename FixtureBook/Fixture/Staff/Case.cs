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
        private bool notYet = true;

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
            SetupIfNotYet();
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
            SetupIfNotYet();
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
            SetupIfNotYet();
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
            if (!ValidateStorageInternal())
            {
                throw new ConfigException("M_Fixture_Case_Validate_Storage", this);
            }
        }

        internal bool ValidateStorageInternal()
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
            return validated;
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
            sheet.SetupIfNotYet();
            foreach (IStorageUpdater actor in dressingRoom.StorageUpdaters)
            {
                if (actor.HasRole())
                {
                    actor.Setup();
                }
            }
        }

        private void SetupIfNotYet()
        {
            if (notYet)
            {
                Setup();
                notYet = false;
            }
        }

        public override string ToString()
        {
            return sheet.ToString() + "[" + Name + "]";
        }

        /// <summary>
        /// 指定された処理で発生した例外が「E.取得データ」に記述された予想結果と適合することを調べる。
        /// </summary>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">テスト対象の処理</param>
        /// <param name="typeName">テーブル定義名</param>
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

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// ValidateStorage の実行を行う。
        /// </summary>
        /// <param name="action">テスト対象処理</param>
        /// <param name="types">actionに渡す引数の型</param>
        public void Expect(Delegate action, params Type[] types)
        {
            dressingRoom.Conductor.Expect(action, types);
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして func を実行し、
        /// Validate で戻り値を検証し、ValidateStorage の実行を行う。
        /// </summary>
        /// <param name="func">テスト対象処理</param>
        /// <param name="types">funcに渡す引数の型</param>
        public void ExpectReturn(Delegate func, params Type[] types)
        {
            dressingRoom.Conductor.ExpectReturn(func, types);
        }

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// Validate&lt;TException&gt; で想定される例外を検証し、ValidateStorage の実行を行う。
        /// </summary>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <param name="types">actionに渡す引数の型</param>
        public void ExpectThrown<TException>(Delegate action, params Type[] types) where TException : Exception
        {
            dressingRoom.Conductor.ExpectThrown<TException>(action, types);
        }

        /// <summary>
        /// Expect, ExpectReturn, ExpectThrown を実行した際の引数を取得する。
        /// </summary>
        /// <typeparam name="T">取得する引数の型</typeparam>
        /// <param name="index">取得する引数のインデックス</param>
        /// <returns>引数の値</returns>
        public T GetParameterAt<T>(int index)
        {
            return dressingRoom.Conductor.GetParameterAt<T>(index);
        }

        /// <summary>
        /// Expect, ExpectReturn, ExpectThrown を実行した際の引数の値を
        /// 「E.取得データ」に記述された値と同じかどうかを検証する。
        /// このメソッドでは、「E.取得データ」のテーブル定義名は「D.パラメタ」の定義名と同じものとみなされる。
        /// </summary>
        /// <param name="index">検証する引数のインデックス</param>
        public void ValidateParameterAt(int index)
        {
            dressingRoom.Conductor.ValidateParameterAt(index);
        }

        /// <summary>
        /// Expect, ExpectReturn, ExpectThrown を実行した際の引数の値を
        /// 「E.取得データ」に記述された値と同じかどうかを検証する。
        /// </summary>
        /// <param name="index">検証する引数のインデックス</param>
        /// <param name="name">テーブル定義名</param>
        public void ValidateParameterAt(int index, string name)
        {
            dressingRoom.Conductor.ValidateParameterAt(index, name);
        }
    }
}
