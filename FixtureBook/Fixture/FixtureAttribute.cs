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

namespace XPFriend.Fixture
{
    /// <summary>
    /// フィクスチャ。
    /// </summary>
    public class FixtureAttribute : Attribute
    {
        /// <summary>
        /// テストカテゴリ。
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// テストケース記述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// テストで使用するシート及びテストケースを指定する。
        /// </summary>
        /// <param name="category">テストカテゴリ（シート名）</param>
        /// <param name="description">テストケース記述（テストケース名）</param>
        public FixtureAttribute(string category, string description)
        {
            Category = category;
            Description = description;
        }
    }
}
