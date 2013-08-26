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
using System.Collections.Generic;

namespace XPFriend.Fixture.Staff
{
    /// <summary>
    /// 行データ。
    /// </summary>
    internal class Row
    {
        private Dictionary<string, string> values = new Dictionary<string, string>();
        private int index;
        private bool deleted;

        /// <summary>
        /// 行データを作成する。
        /// </summary>
        /// <param name="index">行番号</param>
        /// <param name="deleted">削除指定行かどうか</param>
        internal Row(int index, bool deleted)
        {
            this.index = index;
            this.deleted = deleted;
        }

        /// <summary>
        /// 行データ。
        /// </summary>
        public Dictionary<string, string> Values
        {
            get { return values; }
        }

        /// <summary>
        /// 行番号。
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        /// 削除指定行かどうか。
        /// </summary>
        public bool Deleted
        {
            get { return deleted; }
        }

        public override string ToString()
        {
            return index.ToString();
        }
    }
}
