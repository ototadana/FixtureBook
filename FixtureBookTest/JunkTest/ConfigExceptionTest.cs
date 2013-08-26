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
using System.Globalization;
using System.Resources;
using XPFriend.Junk;

namespace XPFriend.JunkTest
{
    [TestClass]
    public class ConfigExceptionTest
    {
        private ResiTest resiTest = new ResiTest();

        [TestInitialize]
        public void setup()
        {
            resiTest.setup();
        }

        [TestCleanup]
        public void cleanup()
        {
            resiTest.cleanup();
        }

        [TestMethod]
        public void Cultureプロパティに設定した値はCultureプロパティで取得できる()
        {
            Cultureプロパティに設定した値はCultureプロパティで取得できる(null);
            Cultureプロパティに設定した値はCultureプロパティで取得できる(CultureInfo.GetCultureInfo("ca"));
            Cultureプロパティに設定した値はCultureプロパティで取得できる(CultureInfo.GetCultureInfo("fr"));
        }

        private void Cultureプロパティに設定した値はCultureプロパティで取得できる(CultureInfo culture)
        {
            // setup
            ConfigException exception = new ConfigException("");

            // when
            exception.Culture = culture;

            // then
            Assert.AreEqual(culture, exception.Culture);
        }

        [TestMethod]
        public void コンストラクタ_message_args_で設定したメッセージはMessageプロパティで取得できる()
        {
            コンストラクタ_message_args_で設定したメッセージはMessageプロパティで取得できる("message1");
            コンストラクタ_message_args_で設定したメッセージはMessageプロパティで取得できる("message2");
        }

        private void コンストラクタ_message_args_で設定したメッセージはMessageプロパティで取得できる(String message)
        {
            // when
            ConfigException exception = new ConfigException(message);

            // then
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(message, exception.ResourceKey);
        }

        [TestMethod]
        public void コンストラクタ_message_args_で設定したメッセージがnullのときMessageプロパティはデフォルトメッセージを返す()
        {
            // when
            ConfigException exception = new ConfigException((string)null);

            // then
            Assert.IsNotNull(exception.Message);
        }

        [TestMethod]
        public void コンストラクタ_exceptionで設定した例外はInnerExceptionプロパティで取得できる()
        {
            コンストラクタ_exceptionで設定した例外はInnerExceptionプロパティで取得できる(new Exception());
            コンストラクタ_exceptionで設定した例外はInnerExceptionプロパティで取得できる(new SystemException());
            コンストラクタ_exceptionで設定した例外はInnerExceptionプロパティで取得できる(null);
        }

        private void コンストラクタ_exceptionで設定した例外はInnerExceptionプロパティで取得できる(Exception cause)
        {
            // when
            ConfigException exception = new ConfigException(cause);

            // then
            Assert.AreSame(cause, exception.InnerException);
        }


        [TestMethod]
        public void コンストラクタ_exception_message_args_で設定したメッセージはMessageプロパティで取得できる()
        {
            コンストラクタ_exception_message_args_で設定したメッセージはMessageプロパティで取得できる("message1");
            コンストラクタ_exception_message_args_で設定したメッセージはMessageプロパティで取得できる("message2");
        }

        private void コンストラクタ_exception_message_args_で設定したメッセージはMessageプロパティで取得できる(String message)
        {
            // when
            ConfigException exception = new ConfigException(new Exception(), message);

            // then
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(message, exception.ResourceKey);
        }

        [TestMethod]
        public void コンストラクタ_exception_message_args_で設定したメッセージがnullのときMessageプロパティはデフォルトメッセージを返す()
        {
            // when
            ConfigException exception = new ConfigException(new Exception(), null);

            // then
            Assert.IsNotNull(exception.Message);
        }

        [TestMethod]
        public void コンストラクタ_exception_message_args_で設定した例外はInnerExceptionプロパティで取得できる()
        {
            コンストラクタ_exception_message_args_で設定した例外はInnerExceptionプロパティで取得できる(new Exception());
            コンストラクタ_exception_message_args_で設定した例外はInnerExceptionプロパティで取得できる(new SystemException());
            コンストラクタ_exception_message_args_で設定した例外はInnerExceptionプロパティで取得できる(null);
        }

        private void コンストラクタ_exception_message_args_で設定した例外はInnerExceptionプロパティで取得できる(Exception cause)
        {
            // when
            ConfigException exception = new ConfigException(cause, null);

            // then
            Assert.AreSame(cause, exception.InnerException);
        }

        [TestMethod]
        public void コンストラクタ_message_args_で設定したリソースキーはMessageプロパティでメッセージ変換される()
        {
            コンストラクタ_message_args_で設定したリソースキーはMessageプロパティでメッセージ変換される("aaa", "あああ");
            コンストラクタ_message_args_で設定したリソースキーはMessageプロパティでメッセージ変換される("bbb", "いいい");
        }

        private void コンストラクタ_message_args_で設定したリソースキーはMessageプロパティでメッセージ変換される(String key, String result)
        {
            try
            {
                // when
                setup();
                Resi.Add(new ResourceManager("XPFriend.JunkTest.testres.Resources_test01", typeof(ResiTest).Assembly));
                ConfigException exception = new ConfigException(key);

                // then
                Assert.AreEqual(result, exception.Message);
                Assert.AreEqual(key, exception.ResourceKey);
            }
            finally
            {
                cleanup();
            }
        }

        [TestMethod]
        public void コンストラクタ_exception_message_args_で設定したリソースキーはMessageプロパティでメッセージ変換される()
        {
            コンストラクタ_exception_message_args_で設定したリソースキーはMessageプロパティでメッセージ変換される("aaa", "あああ");
            コンストラクタ_exception_message_args_で設定したリソースキーはMessageプロパティでメッセージ変換される("bbb", "いいい");
        }

        private void コンストラクタ_exception_message_args_で設定したリソースキーはMessageプロパティでメッセージ変換される(String key, String result)
        {
            try
            {
                // when
                setup();
                Resi.Add(new ResourceManager("XPFriend.JunkTest.testres.Resources_test01", typeof(ResiTest).Assembly));
                ConfigException exception = new ConfigException(new Exception(), key);

                // then
                Assert.AreEqual(result, exception.Message);
                Assert.AreEqual(key, exception.ResourceKey);
            }
            finally
            {
                cleanup();
            }
        }

        [TestMethod]
        public void Cultureプロパティで設定されたカルチャでMessageプロパティはメッセージ変換する()
        {
            // setup
            Resi.Add(new ResourceManager("XPFriend.JunkTest.testres.Resources_test01", typeof(ResiTest).Assembly));

            // expect
            Cultureプロパティで設定されたカルチャでMessageプロパティはメッセージ変換する(CultureInfo.GetCultureInfo("en"), "AAA");
            Cultureプロパティで設定されたカルチャでMessageプロパティはメッセージ変換する(CultureInfo.GetCultureInfo("ja"), "あああ");
        }

        private void Cultureプロパティで設定されたカルチャでMessageプロパティはメッセージ変換する(CultureInfo culture, string result)
        {
            // when
            ConfigException exception = new ConfigException("aaa");
            exception.Culture = culture;

            // then
            Assert.AreEqual(result, exception.Message);
        }

        [TestMethod]
        public void コンストラクタ_message_args_で設定したメッセージ引数を使ってMessageプロパティはメッセージ変換する()
        {
            // setup
            Resi.Add(new ResourceManager("XPFriend.JunkTest.testres.Resources_test01", typeof(ResiTest).Assembly));

            // expect
            コンストラクタ_message_args_で設定したメッセージ引数を使ってMessageプロパティはメッセージ変換する("X", "aXc");
            コンストラクタ_message_args_で設定したメッセージ引数を使ってMessageプロパティはメッセージ変換する("Y", "aYc");
        }

        private void コンストラクタ_message_args_で設定したメッセージ引数を使ってMessageプロパティはメッセージ変換する(String args, String result)
        {
            // when
            ConfigException exception = new ConfigException("ddd", args);

            // then
            Assert.AreEqual(result, exception.Message);
        }

        [TestMethod]
        public void コンストラクタ_exception_message_args_で設定したメッセージ引数を使ってMessageプロパティはメッセージ変換する()
        {
            // setup
            Resi.Add(new ResourceManager("XPFriend.JunkTest.testres.Resources_test01", typeof(ResiTest).Assembly));

            // expect
            コンストラクタ_exception_message_args_で設定したメッセージ引数を使ってMessageプロパティはメッセージ変換する("X", "aXc");
            コンストラクタ_exception_message_args_で設定したメッセージ引数を使ってMessageプロパティはメッセージ変換する("Y", "aYc");
        }

        private void コンストラクタ_exception_message_args_で設定したメッセージ引数を使ってMessageプロパティはメッセージ変換する(String args, String result)
        {
            // when
            ConfigException exception = new ConfigException(new Exception(), "ddd", args);

            // then
            Assert.AreEqual(result, exception.Message);
        }

    }
}
