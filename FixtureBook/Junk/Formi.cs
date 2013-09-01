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
    /// 書式を使った文字列とオブジェクトとのコンバータ。
    /// </summary>
    internal static class Formi
    {
        private static Formatter instance = Formatter.Instance;

        /// <summary>
        /// デフォルトの日時書式。
        /// </summary>
        public static string DefaultTimestampFormat
        {
            get { return instance.DefaultTimestampFormat; }
        }

        /// <summary>
        /// <see cref="DefaultTimestampFormat"/>で取得できる書式に従って指定された日時型データを文字列に変換する。
        /// </summary>
        /// <param name="dateTime">変換する日時型データ</param>
        /// <returns>文字列変換された日時</returns>
        public static string FormatTimestamp(DateTime dateTime)
        {
            return Format(dateTime, DefaultTimestampFormat);
        }

        /// <summary>
        /// 指定された日付型データを指定された書式の文字列に変換する。
        /// </summary>
        /// <param name="dateTime">変換する日時型データ</param>
        /// <param name="formatText">文字列変換で使用する書式</param>
        /// <returns>文字列変換された日時</returns>
        public static string Format(DateTime dateTime, string formatText)
        {
            return Format(dateTime, formatText, null);
        }

        /// <summary>
        /// 指定された日付型データを指定されたタイムゾーンと書式の文字列に変換する。
        /// </summary>
        /// <param name="dateTime">変換する日時型データ</param>
        /// <param name="formatText">文字列変換で使用する書式</param>
        /// <param name="timeZone">タイムゾーン</param>
        /// <returns>文字列変換された日時</returns>
        public static string Format(DateTime dateTime, string formatText, TimeZoneInfo timeZone)
        {
            return instance.Format(dateTime, formatText, timeZone);
        }

        internal static void Initialize()
        {
            instance.Initialize();
        }
    }
}
