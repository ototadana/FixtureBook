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

namespace XPFriend.Fixture.Role
{
    /// <summary>
    /// テスト対象メソッド呼び出しの際のパラメタとして利用するオブジェクトを生成するためのアクター。
    /// </summary>
    internal interface IObjectFactory : IActor
    {
        /// <summary>
        /// 指定されたクラスのオブジェクトを生成可能かどうかを調べる。
        /// </summary>
        /// <typeparam name="T">生成するオブジェクトのクラス</typeparam>
        /// <param name="typeName">定義名</param>
        /// <returns>生成可能な場合は true</returns>
        bool HasRole<T>(params string[] typeName);

        /// <summary>
        /// オブジェクトを生成する。
        /// </summary>
        /// <typeparam name="T">生成するオブジェクトのクラス</typeparam>
        /// <param name="typeName">定義名</param>
        /// <returns>生成されたオブジェクト</returns>
        T GetObject<T>(params string[] typeName);

        /// <summary>
        /// オブジェクトのリストを生成する
        /// </summary>
        /// <typeparam name="T">生成するリスト要素オブジェクトクラス</typeparam>
        /// <param name="typeName">定義名</param>
        /// <returns>生成されたリスト</returns>
        List<T> GetList<T>(string typeName);

        /// <summary>
        /// オブジェクトの配列を生成する
        /// </summary>
        /// <typeparam name="T">生成する配列要素オブジェクトクラス</typeparam>
        /// <param name="typeName">定義名</param>
        /// <returns>生成された配列</returns>
        T[] GetArray<T>(string typeName);
    }
}
