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
using System.Globalization;
using System.Resources;
using System.Threading;
using XPFriend.Junk;

namespace XPFriend.JunkTest
{
    [TestClass]
    public class ResiTest
    {
        private CultureInfo temp;

        [TestInitialize]
        public void setup()
        {
            temp = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ja");
            Resi.Initialize();
        }

        [TestCleanup]
        public void cleanup()
        {
            Thread.CurrentThread.CurrentUICulture = temp;
            Resi.Initialize();
        }

        [TestMethod]
        public void Get_key_は指定したキーに対応する文字列をAddで追加したResourceManagerから取得する()
        {
            Get_key_は指定したキーに対応する文字列をAddで追加したResourceManagerから取得する("aaa", "あああ");
            Get_key_は指定したキーに対応する文字列をAddで追加したResourceManagerから取得する("bbb", "いいい");
        }

        private void Get_key_は指定したキーに対応する文字列をAddで追加したResourceManagerから取得する(string key, string result)
        {
            setup();
            try
            {
                // when
                Resi.Add(new ResourceManager("XPFriend.JunkTest.testres.Resources_test01", typeof(ResiTest).Assembly));

                // then
                Assert.AreEqual(result, Resi.Get(key));
            }
            finally
            {
                cleanup();
            }
        }

        [TestMethod]
        public void Get_key_はより後でAddしたリソースの内容を優先する()
        {
            Get_key_はより後でAddしたリソースの内容を優先する("aaa", "ううう");
            Get_key_はより後でAddしたリソースの内容を優先する("bbb", "いいい");
            Get_key_はより後でAddしたリソースの内容を優先する("ccc", "えええ");
        }

        private void Get_key_はより後でAddしたリソースの内容を優先する(string key, string result)
        {
            setup();
            try
            {
                // when
                Resi.Add(new ResourceManager("XPFriend.JunkTest.testres.Resources_test01", typeof(ResiTest).Assembly));
                Resi.Add(new ResourceManager("XPFriend.JunkTest.testres.Resources_test02", typeof(ResiTest).Assembly));

                // then
                Assert.AreEqual(result, Resi.Get(key));
            }
            finally
            {
                cleanup();
            }
        }

        [TestMethod]
        public void Get_key_は一度設定されたものでも再度後にAddしたリソースの内容を優先する()
        {
            Get_key_は一度設定されたものでも再度後にAddしたリソースの内容を優先する("aaa", "ううう");
            Get_key_は一度設定されたものでも再度後にAddしたリソースの内容を優先する("bbb", "いいい");
            Get_key_は一度設定されたものでも再度後にAddしたリソースの内容を優先する("ccc", "えええ");
        }

        private void Get_key_は一度設定されたものでも再度後にAddしたリソースの内容を優先する(string key, string result)
        {
            setup();
            try
            {
                // when
                ResourceManager res1 = new ResourceManager("XPFriend.JunkTest.testres.Resources_test02", typeof(ResiTest).Assembly);
                ResourceManager res2 = new ResourceManager("XPFriend.JunkTest.testres.Resources_test01", typeof(ResiTest).Assembly);
                Resi.Add(res1);
                Resi.Add(res2);
                Resi.Add(res1);

                // then
                Assert.AreEqual(result, Resi.Get(key));
            }
            finally
            {
                cleanup();
            }
        }

        [TestMethod]
        public void Get_key_は指定したキーに対応する文字列が登録されていない場合キーをそのまま返す()
        {
            // expect
            Assert.AreEqual("xxx", Resi.Get("xxx"));
        }

        [TestMethod]
        public void Get_key_defaultValue_は指定したキーに対応する文字列が登録されていない場合defaultValueを返す()
        {
            // expect
            Assert.AreEqual("yyy", Resi.Get("xxx", "yyy"));
        }

        [TestMethod]
        public void Get_locale_key_defaultValueは指定したロケールに対応するリソースから取得する() 
        {
            Get_locale_key_defaultValueは指定したロケールに対応するリソースから取得する(CultureInfo.GetCultureInfo("en"), "AAA");
            Get_locale_key_defaultValueは指定したロケールに対応するリソースから取得する(CultureInfo.GetCultureInfo("ja"), "あああ");
        }

        private void Get_locale_key_defaultValueは指定したロケールに対応するリソースから取得する(CultureInfo cultureInfo, string result)
        {
            setup();
            Resi.Add(new ResourceManager("XPFriend.JunkTest.testres.Resources_test01", typeof(ResiTest).Assembly));
            try
            {
                // expect
                Assert.AreEqual(result, Resi.Get(cultureInfo, "aaa", ""));
            }
            finally
            {
                cleanup();
            }
        }
    }
}
