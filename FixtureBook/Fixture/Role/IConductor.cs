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
    /// フィクスチャ操作のコンダクター。
    /// </summary>
    internal interface IConductor : IActor
    {
        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// ValidateStorage の実行を行う。
        /// </summary>
        /// <param name="action">テスト対象処理</param>
        /// <param name="types">actionに渡す引数の型</param>
        void Expect(Delegate action, params Type[] types);

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして func を実行し、
        /// Validate で戻り値を検証し、ValidateStorage の実行を行う。
        /// </summary>
        /// <param name="func">テスト対象処理</param>
        /// <param name="types">funcに渡す引数の型</param>
        void ExpectReturn(Delegate func, params Type[] types);

        /// <summary>
        /// GetObject / GetList / GetArray で取得したオブジェクトを引数にして action を実行し、
        /// Validate&lt;TException&gt; で想定される例外を検証し、ValidateStorage の実行を行う。
        /// </summary>
        /// <typeparam name="TException">発生が予想される例外</typeparam>
        /// <param name="action">テスト対象処理</param>
        /// <param name="types">actionに渡す引数の型</param>
        void ExpectThrown<TException>(Delegate action, params Type[] types) where TException : Exception;

        void ValidateParameterAt(int index, string name);

        void ValidateParameterAt(int index);

        T GetParameterAt<T>(int index);

        void Expect(Type targetClass, string targetMethod, Type[] targetMethodParameter);

        void ExpectReturn(Type targetClass, string targetMethod, Type[] targetMethodParameter);

        void ExpectThrown<TException>(Type targetClass, string targetMethod, Type[] targetMethodParameter) where TException : Exception;
    }
}
