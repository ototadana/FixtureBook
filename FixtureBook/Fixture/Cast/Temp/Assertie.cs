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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class Assertie
    {
        public static void Fail(string resourceKey, params object[] parameters)
        {
            Assert.Fail(GetMessage(resourceKey, parameters));
        }

        public static void AreEqual(object expected, object actual, string resourceKey, params object[] parameters)
        {
            Assert.AreEqual(expected, actual, GetMessage(resourceKey, parameters));
        }

        public static void AreEqual(string expected, string actual, string resourceKey, params object[] parameters)
        {
            Assert.AreEqual(expected, actual, GetMessage(resourceKey, parameters));
        }

        public static void AreEqual(int expected, int actual, string resourceKey, params object[] parameters)
        {
            Assert.AreEqual(expected, actual, GetMessage(resourceKey, parameters));
        }

        private static string GetMessage(string key, params object[] args)
        {
            string messageFormat = Resi.Get(key, key);
            return String.Format(messageFormat, args);
        }
    }
}
