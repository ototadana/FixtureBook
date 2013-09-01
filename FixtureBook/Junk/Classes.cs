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
using XPFriend.Junk.Temp;

namespace XPFriend.Junk
{
    /// <summary>
    /// クラス管理。
    /// </summary>
    internal static class Classes
    {
        private static ClassManager instance = new ClassManager();

        /// <summary>
        /// ある基底クラスの実装クラスを設定する。
        /// </summary>
        /// <param name="baseClass">基底クラス</param>
        /// <param name="implClass">実装クラス</param>
        public static void Put(Type baseClass, Type implClass)
        {
            instance[baseClass] = implClass;
        }

        /// <summary>
        /// <see cref="Put"/> で設定した実装クラスを取得する。
        /// <see cref="Put"/> で設定していない場合は、引数の基底クラスをそのまま返す。
        /// </summary>
        /// <param name="baseClass">基底クラス</param>
        /// <returns>実装クラス</returns>
        public static Type Get(Type baseClass)
        {
            Type cls = instance[baseClass];
            if (cls == null)
            {
                cls = baseClass;
            }
            return cls;
        }

        /// <summary>
        /// <see cref="Get"/> で取得できる実装クラスのインスタンスを作成する。
        /// </summary>
        /// <typeparam name="T">基底クラス</typeparam>
        /// <returns>実装クラスのインスタンス</returns>
        public static T NewInstance<T>()
        {
            Type cls = Classes.Get(typeof(T));
            return (T)cls.GetConstructor(new Type[] { }).Invoke(new object[]{});
        }

        internal static void Initialize()
        {
            instance.Initialize();
        }
    }
}
