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
namespace XPFriend.Fixture.Role
{
    /// <summary>
    /// テスト対象メソッド呼び出し後のオブジェクト状態を検証するためのアクター。
    /// </summary>
    internal interface IObjectValidator : IActor
    {
        /// <summary>
        /// 指定されたオブジェクトの検証が可能かどうかを調べる。
        /// </summary>
        /// <param name="obj">調べるオブジェクト</param>
        /// <param name="typeName">定義名</param>
        /// <returns>検証可能な場合は true</returns>
        bool HasRole(object obj, params string[] typeName);

        /// <summary>
        /// 指定されたオブジェクトが予想結果と適合するかどうかを調べる。
        /// 予想結果と適合しない場合は例外がスローされる。
        /// </summary>
        /// <param name="obj">調べるオブジェクト</param>
        /// <param name="typeName">定義名</param>
        void Validate(object obj, params string[] typeName);

        /// <summary>
        /// 指定された処理で発生した例外が予想結果と適合することを調べる。
        /// </summary>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">処理</param>
        /// <param name="typeName">定義名</param>
        void Validate<TException>(Action action, string typeName) where TException : Exception;
    }
}
