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
using System;
using System.IO;
using XPFriend.Junk;

namespace XPFriend.JunkTest
{
    [TestClass]
    public class ConfigTest
    {
        private const string XmlTemplate =
            @"<?xml version='1.0' encoding='utf-8' ?>
              <xjc.config>
                  <add key='text01' value='aaa'/>
                  <add key='int01' value='123'/>
                  <add key='boolean01' value='true'/>
              </xjc.config>";

        private const string EmptyXmlTemplate =
            @"<?xml version='1.0' encoding='utf-8' ?>
              <xjc.config/>";

        private void InitializeClassesXml()
        {
            WriteClassesXml(EmptyXmlTemplate);
        }

        private void WriteClassesXml(string template)
        {
            using (StreamWriter writer = new StreamWriter("Config.xml"))
            {
                writer.WriteLine(template);
                writer.Flush();
            }
        }

        [TestInitialize]
        public void Setup()
        {
            InitializeClassesXml();
            Config.Initialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            InitializeClassesXml();
            Config.Initialize();
        }

        [TestMethod]
        public void Get_key_stringは設定ファイルから設定値を取得する()
        {
            // when
            WriteClassesXml(XmlTemplate);
            Config.Initialize();

            // then
            Assert.AreEqual("aaa", Config.Get("text01", ""));
        }

        [TestMethod]
        public void Get_key_boolは設定ファイルから設定値を取得する()
        {
            // when
            WriteClassesXml(XmlTemplate);
            Config.Initialize();

            // then
            Assert.AreEqual(true, Config.Get("boolean01", false));
        }

        [TestMethod]
        public void Get_key_intは設定ファイルから設定値を取得する()
        {
            // when
            WriteClassesXml(XmlTemplate);
            Config.Initialize();

            // then
            Assert.AreEqual(123, Config.Get("int01", 0));
        }


        [TestMethod]
        public void Get_key_stringは設定されていないキーが指定された場合デフォルト値を返す()
        {
            // expert
            Assert.AreEqual("xxx", Config.Get("text01", "xxx"));
        }

        [TestMethod]
        public void Get_key_boolは設定されていないキーが指定された場合デフォルト値を返す()
        {
            // expect
            Assert.AreEqual(false, Config.Get("boolean01", false));
        }

        [TestMethod]
        public void Get_key_intは設定されていないキーが指定された場合デフォルト値を返す()
        {
            // expect
            Assert.AreEqual(0, Config.Get("int01", 0));
        }

        [TestMethod]
        public void Putで設定した値はGetで取得できる()
        {
            // when
            Config.Put("aaa", "xxx");

            // then
            Assert.AreEqual("xxx", Config.Get("aaa", null));
            Assert.AreEqual("xxx", Config.Get("aaa"));
        }

        [TestMethod]
        public void Putでnullを設定すると値がクリアされる()
        {
            // setup
            Config.Put("aaa", "xxx");
            Assert.AreEqual("xxx", Config.Get("aaa", null));

            // when
            Config.Put("aaa", null);

            // then
            Assert.AreEqual(null, Config.Get("aaa", null));
        }

        [TestMethod]
        public void Putは何回実行しても同じ結果になる()
        {
            // when
            Config.Put("aaa", "aaa");
            Config.Put("aaa", "aaa");

            // then
            Assert.AreEqual("aaa", Config.Get("aaa", null));
        }

        [TestMethod]
        public void Get_keyは設定されていないキーが指定された場合ConfigExceptionをスローする()
        {
            try
            {
                // when
                Config.Get("xxxxxxxxxxxx");
                Assert.Fail("ここにはこない");
            }
            catch (ConfigException e)
            {
                // then
                Console.WriteLine(e);
            }
        }
    }
}
