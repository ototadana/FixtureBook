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
    /// アプリケーション設定情報管理。
    /// </summary>
    internal static class Config
    {
        private static ConfigManager instance = ConfigManager.Instance;

        /// <summary>
        /// 指定された設定項目名で値を設定する。
        /// </summary>
        /// <param name="key">設定項目名</param>
        /// <param name="value">登録する値</param>
        public static void Put(string key, string value)
        {
            instance.Put(key, value);
        }


        /// <summary>
        /// 指定された設定項目の値を取得する。
        /// </summary>
        /// <param name="key">設定項目名</param>
        /// <param name="defaultValue">取得できなかった場合のデフォルト値</param>
        /// <returns>設定値</returns>
        public static string Get(string key, string defaultValue)
        {
            return instance.Get(key, defaultValue);
        }

        /// <summary>
        /// 指定された設定項目の値を取得する。
        /// </summary>
        /// <param name="key">設定項目名</param>
        /// <param name="defaultValue">取得できなかった場合のデフォルト値</param>
        /// <returns>設定値</returns>
        public static int Get(string key, int defaultValue)
        {
            string value = Get(key, (string)null);
            if (value == null)
            {
                return defaultValue;
            }
            return int.Parse(value);
        }

        /// <summary>
        /// 指定された設定項目の値を取得する。
        /// </summary>
        /// <param name="key">設定項目名</param>
        /// <param name="defaultValue">取得できなかった場合のデフォルト値</param>
        /// <returns>設定値</returns>
        public static bool Get(string key, bool defaultValue)
        {
            string value = Get(key, (string)null);
            if (value == null)
            {
                return defaultValue;
            }
            return bool.Parse(value);
        }

        /// <summary>
        /// 指定された設定項目の値を取得する。
        /// 指定された設定項目が設定されていない場合は <see cref="ConfigException"/> が発生する。
        /// </summary>
        /// <param name="key">設定項目名</param>
        /// <returns>設定値</returns>
        public static string Get(string key)
        {
            string value = Get(key, null);
            if (value == null)
            {
                throw new ConfigException("M_Junk_Config_Get", key);
            }
            return value;
        }

        internal static void Initialize()
        {
            instance.Initialize();
        }
    }
}
