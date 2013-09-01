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
    /// ロギング。
    /// </summary>
    internal static class Loggi
    {
        private static LoggiLogger logger = LoggiLogger.Instance;

        /// <summary>
        /// デバッグレベルのログ出力を行なう。
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        public static void Debug(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// デバッグレベルのログ出力を行なう。
        /// </summary>
        /// <param name="e">例外情報</param>
        public static void Debug(Exception e)
        {
            logger.Debug(e);
        }

        /// <summary>
        /// デバッグレベルのログ出力を行なう。
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        /// <param name="e">例外情報</param>
        public static void Debug(string message, Exception e)
        {
            logger.Debug(message, e);
        }

        /// <summary>
        /// デバッグログ出力可不可設定。
        /// </summary>
        public static bool DebugEnabled
        {
            get { return logger.DebugEnabled; }
            set { logger.DebugEnabled = value; }
        }

        /// <summary>
        /// 情報レベルのログ出力を行なう。
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        public static void Info(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// 情報レベルのログ出力を行なう。
        /// </summary>
        /// <param name="e">例外情報</param>
        public static void Info(Exception e)
        {
            logger.Info(e);
        }

        /// <summary>
        /// 情報レベルのログ出力を行なう。
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        /// <param name="e">例外情報</param>
        public static void Info(string message, Exception e)
        {
            logger.Info(message, e);
        }

        /// <summary>
        /// 警告レベルのログ出力を行なう。
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        public static void Warn(string message)
        {
            logger.Warn(message);
        }

        /// <summary>
        /// 警告レベルのログ出力を行なう。
        /// </summary>
        /// <param name="e">例外情報</param>
        public static void Warn(Exception e)
        {
            logger.Warn(e);
        }

        /// <summary>
        /// 警告レベルのログ出力を行なう。
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        /// <param name="e">例外情報</param>
        public static void Warn(string message, Exception e)
        {
            logger.Warn(message, e);
        }

        /// <summary>
        /// エラーレベルのログ出力を行なう。
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        public static void Error(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// エラーレベルのログ出力を行なう。
        /// </summary>
        /// <param name="e">例外情報</param>
        public static void Error(Exception e)
        {
            logger.Error(e);
        }

        /// <summary>
        /// エラーレベルのログ出力を行なう。
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        /// <param name="e">例外情報</param>
        public static void Error(string message, Exception e)
        {
            logger.Error(message, e);
        }
    }
}
