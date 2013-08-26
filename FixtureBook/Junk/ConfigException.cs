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
using System.Globalization;

namespace XPFriend.Junk
{
    /// <summary>
    /// 設定に関連する例外。
    /// </summary>
    internal class ConfigException : Exception
    {
        private object[] args;

        /// <summary>
        /// ConfigException をメッセージ付きで作成する。
        /// </summary>
        /// <param name="resourceKey">メッセージリソースのキー</param>
        /// <param name="args">メッセージパラメタ</param>
        public ConfigException(string resourceKey, params object[] args)
            : base(resourceKey)
        {
            this.args = args;            
        }

        /// <summary>
        /// ConfigException を原因例外付きで作成する。
        /// </summary>
        /// <param name="exception">この例外が発生する原因となった例外</param>
        public ConfigException(Exception exception) : base((exception != null)?exception.Message:null, exception)
        {
            this.args = new object[] { };
        }

        /// <summary>
        /// ConfigException を原因例外とメッセージ付きで作成する。
        /// </summary>
        /// <param name="exception">この例外が発生する原因となった例外</param>
        /// <param name="resourceKey">メッセージ</param>
        /// <param name="args">メッセージパラメタ</param>
        public ConfigException(Exception exception, string resourceKey, params object[] args)
            : base(resourceKey, exception)
        {
            this.args = args;
        }

        /// <summary>
        /// カルチャ情報。
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// 例外のメッセージ。
        /// </summary>
        public override string Message
        {
            get {
                string key = base.Message;
                string messageFormat = Resi.Get(Culture, key, key);
                return string.Format(messageFormat, args);
            }
        }

        /// <summary>
        /// コンストラクタで指定したメッセージリソースのキー。
        /// </summary>
        public virtual string ResourceKey { get { return base.Message; } }
    }
}
