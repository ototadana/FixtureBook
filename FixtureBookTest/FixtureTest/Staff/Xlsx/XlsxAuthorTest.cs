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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPFriend.Fixture.Staff.Xlsx;
using XPFriend.Fixture.Staff;

namespace XPFriend.FixtureTest.Staff.Xlsx
{
    [TestClass]
    public class XlsxAuthorTest
    {
        [TestMethod]
        public void CreateColumnはtextで指定された列名を含むColumnを返す()
        {
            // when
            Column column = Parser.CreateColumn("apple");

            // then
            Assert.AreEqual("apple", column.Name);
            Assert.IsNull(column.Type);
            Assert.IsNull(column.ComponentType);
        }

        [TestMethod]
        public void CreateColumnのtextにコロンが含まれる場合は列名と列タイプとして解釈する()
        {
            // when
            Column column = Parser.CreateColumn("apple:string");

            // then
            Assert.AreEqual("apple", column.Name);
            Assert.AreEqual("string", column.Type);
            Assert.IsNull(column.ComponentType);
        }

        [TestMethod]
        public void CreateColumnのtextにコロンと角カッコが含まれる場合は列名と列要素タイプとして解釈する()
        {
            CreateColumnのtextにコロンと角カッコが含まれる場合は列名と列要素タイプとして解釈する("apple:string[]");
            CreateColumnのtextにコロンと角カッコが含まれる場合は列名と列要素タイプとして解釈する("apple:string[");
        }

        private void CreateColumnのtextにコロンと角カッコが含まれる場合は列名と列要素タイプとして解釈する(string text)
        {
            // when
            Column column = Parser.CreateColumn(text);

            // then
            Assert.AreEqual("apple", column.Name);
            Assert.IsNull(column.Type);
            Assert.AreEqual("string", column.ComponentType);
        }

        [TestMethod]
        public void CreateColumnのtextにコロンと不等号が含まれる場合は列名と列タイプと列要素タイプとして解釈する()
        {
            CreateColumnのtextにコロンと不等号が含まれる場合は列名と列タイプと列要素タイプとして解釈する("apple:List<string>");
            CreateColumnのtextにコロンと不等号が含まれる場合は列名と列タイプと列要素タイプとして解釈する("apple:List<string");
        }

        private void CreateColumnのtextにコロンと不等号が含まれる場合は列名と列タイプと列要素タイプとして解釈する(string text)
        {
            // when
            Column column = Parser.CreateColumn(text);

            // then
            Assert.AreEqual("apple", column.Name);
            Assert.AreEqual("List", column.Type);
            Assert.AreEqual("string", column.ComponentType);
        }
    }
}
