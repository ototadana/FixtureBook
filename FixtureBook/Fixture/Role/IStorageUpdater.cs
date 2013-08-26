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

using XPFriend.Fixture.Staff;

namespace XPFriend.Fixture.Role
{
    /// <summary>
    /// テスト実行前にストレージに対して更新を行う役割をもつアクター。
    /// </summary>
    internal interface IStorageUpdater : IActor
    {
        /// <summary>
        /// 指定されたシートの内容を使ってこのアクターを初期化する。
        /// </summary>
        /// <param name="sheet">初期化に使用するシート</param>
        void Initialize(Sheet sheet);

        /// <summary>
        /// このアクターに役割があるかどうかを調べる。
        /// </summary>
        /// <returns>役割がある場合は true</returns>
        bool HasRole();

        /// <summary>
        /// ストレージに対する更新を行う。
        /// </summary>
        void Setup();
    }
}
