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

namespace XPFriend.Fixture.Staff.Xlsx
{
    /// <summary>
    /// セル処理。
    /// </summary>
    internal abstract class XlsxCellParser
    {
        /// <summary>
        /// <see cref="XlsxCellParser"/> を作成する。
        /// </summary>
        /// <param name="debugEnabled">デバッグ出力が有効かどうか</param>
        protected XlsxCellParser(bool debugEnabled)
        {
            DebugEnabled = debugEnabled;
        }

        /// <summary>
        /// デバッグ出力が有効かどうか。
        /// </summary>
        protected internal bool DebugEnabled { get; set; }

        /// <summary>
        /// 指定されたセルの値を処理する。
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="column">列番号</param>
        /// <param name="value">セルの値</param>
        internal abstract void Parse(int row, int column, string value);
    }
}
