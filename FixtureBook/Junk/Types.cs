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
using System.Reflection;

namespace XPFriend.Junk
{
    internal static class Types
    {
        /// <summary>
        /// 指定された名前のタイプを取得する。
        /// </summary>
        /// <param name="name">タイプ名</param>
        /// <returns>タイプ</returns>
        public static Type GetType(string name)
        {
            // TODO キャッシュ
            Type type = Type.GetType(name);
            if (type != null)
            {
                return type;
            }

            return GetTypeFromCurrentDomain(name);
        }


        private static Type GetTypeFromCurrentDomain(string name)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(name);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }
    }
}
