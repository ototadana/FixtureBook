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
    public class BaseClass { }
    public class ImplClass : BaseClass { }
    public abstract class ErrorClass { }

    public class DummyBaseClass { }
    public class DummyImplClass : DummyBaseClass { }

    [TestClass]
    public class ClassesTest
    {
        private const string XmlTemplate =
            @"<?xml version='1.0' encoding='utf-8' ?>
                <xjc.classes>
                    <add key='{0}' value='{1}'/>
                </xjc.classes>";

        private void InitializeClassesXml()
        {
            WriteClassesXml(typeof(DummyBaseClass).AssemblyQualifiedName, typeof(DummyBaseClass).AssemblyQualifiedName);
        }

        private void WriteClassesXml(string key, string value)
        {
            using (StreamWriter writer = new StreamWriter("Classes.xml"))
            {
                writer.WriteLine(XmlTemplate, key, value);
                writer.Flush();
            }
        }

        [TestInitialize]
        public void Setup()
        {
            InitializeClassesXml();
        }

        [TestMethod]
        public void xjc_classes設定が存在する場合その内容で初期化が行われる()
        {
            try
            {
                // setup
                WriteClassesXml(typeof(BaseClass).AssemblyQualifiedName, typeof(ImplClass).AssemblyQualifiedName);
                Classes.Initialize();

                // expect
                Assert.AreSame(typeof(ImplClass), Classes.Get(typeof(BaseClass)));
            }
            finally
            {
                // cleanup
                InitializeClassesXml();
                Classes.Initialize();
            }
        }

        [TestMethod]
        public void xjc_classes設定の内容が間違っている場合例外が発生する()
        {
            xjc_classes設定の内容が間違っている場合例外が発生する(typeof(BaseClass).AssemblyQualifiedName + "xxx", typeof(ImplClass).AssemblyQualifiedName);
            xjc_classes設定の内容が間違っている場合例外が発生する("xxx", "yyy");
        }

        private void xjc_classes設定の内容が間違っている場合例外が発生する(string key, string value)
        {
            // setup
            WriteClassesXml(key, value);

            try
            {
                // when
                Classes.Initialize();

                // then
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                Console.WriteLine(e);
            }
            finally
            {
                // cleanup
                InitializeClassesXml();
                Classes.Initialize();
            }
        }

        [TestMethod]
        public void Putで設定した実装クラスはGetで取得できる()
        {
            try
            {
                // setup
                Classes.Initialize();

                // when
                Classes.Put(typeof(BaseClass), typeof(ImplClass));

                // then
                Assert.AreSame(typeof(ImplClass), Classes.Get(typeof(BaseClass)));
            }
            finally
            {
                // cleanup
                Classes.Initialize();
            }
        }

        [TestMethod]
        public void Putで設定していない場合Getを呼び出すと引数のクラスをそのまま返す()
        {
            // setup
            Classes.Initialize();

            // expect
            Assert.AreSame(typeof(BaseClass), Classes.Get(typeof(BaseClass)));
        }

        [TestMethod]
        public void NewInstanceはPutで設定したクラスをインスタンス化する()
        {
            try
            {
                // setup
                Classes.Initialize();

                // when
                Classes.Put(typeof(BaseClass), typeof(ImplClass));

                // then
                Assert.IsInstanceOfType(Classes.NewInstance<BaseClass>(), typeof(ImplClass));
            }
            finally
            {
                // cleanup
                Classes.Initialize();
            }
        }

        [TestMethod]
        public void NewInstanceはPutで設定されていない場合引数のクラスをインスタンス化する()
        {
            // setup
            Classes.Initialize();

            // expect
            Assert.AreSame(typeof(BaseClass), Classes.NewInstance<BaseClass>().GetType());
        }

        [TestMethod]
        public void NewInstanceはインスタンス作成失敗時に例外をスローする()
        {
            try
            {
                // setup
                Classes.Initialize();

                // when
                Classes.NewInstance<ErrorClass>();

                // then
                Assert.Fail("ここにはこない");
            }
            catch (Exception e)
            {
                // then
                Console.WriteLine(e);
            }
        }
    }
}
