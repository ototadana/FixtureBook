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
using XPFriend.Junk;

namespace XPFriend.JunkTest
{
    [TestClass]
    public class FormiTest
    {
    	private const String TimestampFormatConfigKey = "Junk.Formi.defaultTimestampFormat";

        [TestInitialize]
        public void Setup()
        {
            Config.Put(TimestampFormatConfigKey, null);
            Formi.Initialize();
        }

        [TestMethod]
        public void DefaultTimestampFormatはJunk_Formi_defaultTimestampFormatで設定した日付書式を取得する()
        {
            // when
            Config.Put(TimestampFormatConfigKey, "yyyy/MM/dd");
            Formi.Initialize();

            // then
            Assert.AreEqual("yyyy/MM/dd", Formi.DefaultTimestampFormat);
        }

        [TestMethod]
        public void DefaultTimestampFormatはJunk_Formi_defaultTimestampFormatの設定がないときデフォルト日時書式を返す()
        {
            // expect
            Assert.AreEqual("yyyy-MM-dd HH:mm:ss", Formi.DefaultTimestampFormat);
        }

        [TestMethod]
        public void FormatTimestampはDefaultTimestampFormatで取得できる書式で日時を文字列に変換する()
        {
            // setup
            DateTime dateTime = new DateTime(2012, 1, 1, 1, 1, 2);

            // expect
            Assert.AreEqual("2012-01-01 01:01:02", Formi.FormatTimestamp(dateTime));
        }

        [TestMethod]
        public void Formatは指定された書式で日時を文字列に変換する()
        {
            // setup
            DateTime dateTime = new DateTime(2012, 1, 1, 1, 1, 2);

            // expect
            Assert.AreEqual("20120101010102", Formi.Format(dateTime, "yyyyMMddHHmmss"));
        }

        [TestMethod]
        public void Formatは指定された書式とタイムゾーンで日時を文字列に変換する()
        {
            // setup
            TimeZoneInfo current = TimeZoneInfo.Local;
            TimeZoneInfo custom = TimeZoneInfo.CreateCustomTimeZone("test1", TimeSpan.FromHours(current.BaseUtcOffset.Hours + 1), "test1", "test1");
            DateTime dateTime = new DateTime(2012, 1, 1, 1, 1, 2);

            // expect
            Assert.AreEqual("20120101 020102", Formi.Format(dateTime, "yyyyMMdd HHmmss", custom));
        }
    }
}
