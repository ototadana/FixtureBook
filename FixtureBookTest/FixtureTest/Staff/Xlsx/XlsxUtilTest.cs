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
using XPFriend.Fixture.Staff.Xlsx;

namespace XPFriend.FixtureTest.Staff.Xlsx
{
    [TestClass]
    public class XlsxUtilTest
    {
        [TestMethod]
        public void IsDateFormatはtext引数がnullのときfalseを返す()
        {
            Assert.IsFalse(XlsxUtil.IsDateFormat(999, null));
        }

        [TestMethod]
        public void IsDateFormatは不正なindexが指定されたときfalseを返す()
        {
            Assert.IsFalse(XlsxUtil.IsDateFormat(-1, null));
        }

        [TestMethod]
        public void GetNormalizedFormatStringはnullをわたすと0を返す()
        {
            Assert.AreEqual("0", XlsxUtil.GetNormalizedFormatString(null));
        }

        [TestMethod]
        public void GetNormalizedFormatStringは数値書式文字列を整形して返す()
        {
            Assert.AreEqual("0.000", XlsxUtil.GetNormalizedFormatString(".000"));
        }
    }
}
