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

namespace XPFriend.Junk
{
    /// <summary>
    /// 例外処理ユーティリティ。
    /// </summary>
    internal static class ExceptionHandler
    {
        /// <summary>
        /// 指定された例外を無視する。
        /// </summary>
        /// <param name="e">無視する例外</param>
        public static void Ignore(Exception e)
        {
            // なにもしない。
        }
    }
}
