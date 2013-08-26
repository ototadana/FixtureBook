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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture.Staff;

namespace XPFriend.FixtureTest.Staff
{
    [TestClass]
    public class ColumnTest
    {
        [TestMethod]
        public void Nameはコンストラクタで指定された列名を返す()
        {
            // when
            Column column = new Column("test1", null, null);

            // then
            Assert.AreEqual("test1", column.Name);
        }

        [TestMethod]
        public void Typeはコンストラクタで指定された列タイプを返す()
        {
            // when
            Column column = new Column(null, "type1", null);

            // then
            Assert.AreEqual("type1", column.Type);
        }

        [TestMethod]
        public void ComponentTypeはコンストラクタで指定された列要素タイプを返す()
        {
            // when
            Column column = new Column(null, null, "ctype1");

            // then
            Assert.AreEqual("ctype1", column.ComponentType);
        }

        [TestMethod]
        public void IsArrayは列タイプがnullで列要素タイプがnullでない場合にtrueを返す()
        {
            IsArrayは列タイプがnullで列要素タイプがnullでない場合にtrueを返す(null, "x",  true);
            IsArrayは列タイプがnullで列要素タイプがnullでない場合にtrueを返す(null, null, false);
            IsArrayは列タイプがnullで列要素タイプがnullでない場合にtrueを返す("x",  null, false);
            IsArrayは列タイプがnullで列要素タイプがnullでない場合にtrueを返す("x",  "x",  false);
        }

        private void IsArrayは列タイプがnullで列要素タイプがnullでない場合にtrueを返す(string type, string ctype, bool expected)
        {
            // when
            Column column = new Column(null, type, ctype);

            // then
            Assert.AreEqual(expected, column.IsArray());
        }

        [TestMethod]
        public void ToStringは列タイプも列要素タイプも指定されていない場合には列名を表す文字列を返す()
        {
            // when
            Column column = new Column("x", null, null);

            // then
            Assert.AreEqual("x", column.ToString());
        }

        [TestMethod]
        public void ToStringは列名と列タイプのみが指定されている場合には列名コロン列タイプ型式の文字列を返す()
        {
            // when
            Column column = new Column("x", "t", null);

            // then
            Assert.AreEqual("x:t", column.ToString());
        }

        [TestMethod]
        public void ToStringは列名と列タイプと列要素タイプが指定されている場合には列名コロン列タイプ小なり列要素タイプ大なり型式の文字列を返す()
        {
            // when
            Column column = new Column("x", "t", "ct");

            // then
            Assert.AreEqual("x:t<ct>", column.ToString());
        }

        [TestMethod]
        public void は列名と列要素タイプのみが指定されている場合には列名コロン列要素タイプ大カッコ型式の文字列を返す()
        {
            // when
            Column column = new Column("x", null, "ct");

            // then
            Assert.AreEqual("x:ct[]", column.ToString());
        }
    }
}
