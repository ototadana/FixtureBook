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

namespace XPFriend.FixtureTest
{
    class ExceptionEditors
    {
        public void Dummy1(ApplicationException e) {}
        static void Dummy2(ApplicationException e) { }
        public static object Dummy3(object o) { return null; }
        public static object Dummy4() { return null; }
        public static object Dummy5(ApplicationException e, object o) { return null; }

        public static object EditApplicationException(ApplicationException e)
        {
            Console.WriteLine(e);
            Assert.AreEqual("app", e.Message);
            return new Dictionary<string, string>() { { "Message", "zzz" } };
        }

        public static object EditSystemException(SystemException e)
        {
            Console.WriteLine(e);
            Assert.AreEqual("sys", e.Message);
            return new Dictionary<string, string>() { { "Message", "zzz" } };
        }
    }
}
