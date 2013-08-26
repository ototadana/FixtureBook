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
using System.Globalization;
using System.Resources;
using XPFriend.Junk.Temp;

namespace XPFriend.Junk
{
    /// <summary>
    /// リソース管理。
    /// </summary>
    public sealed class Resi
    {
        private static ResourceProvider provider = ResourceProvider.Instance;

        static Resi()
        {
            Add(Properties.Resources.ResourceManager);
        }

        private Resi() {}

        /// <summary>
        /// 指定した ResourceManager のリソースを管理対象として追加する。
        /// </summary>
        /// <param name="resourceManager"></param>
        public static void Add(ResourceManager resourceManager)
        {
            provider.Add(resourceManager);
        }

        /// <summary>
        /// 指定したキーで登録されている文字列を取得する。
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>文字列</returns>
        public static string Get(string key)
        {
            return Get(key, key);
        }

        /// <summary>
        /// 指定したキーで登録されている文字列を取得する。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="defaultValue">指定したキーで登録されていなかった場合に使用する文字列</param>
        /// <returns>文字列</returns>
        public static string Get(string key, string defaultValue)
        {
            return Get(null, key, defaultValue);
        }

        /// <summary>
        /// CultureInfo を指定し、指定したキーで登録されている文字列を取得する。
        /// </summary>
        /// <param name="culture">カルチャ情報</param>
        /// <param name="key">キー</param>
        /// <param name="defaultValue">指定したキーで登録されていなかった場合に使用する文字列</param>
        /// <returns>文字列</returns>
        public static string Get(CultureInfo culture, string key, string defaultValue)
        {
            return provider.Get(culture, key, defaultValue);
        }

        internal static void Initialize()
        {
            provider.Initialize();
        }
    }
}
