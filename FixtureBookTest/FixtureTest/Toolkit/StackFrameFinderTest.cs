﻿/*
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
using System.Diagnostics;
using XPFriend.Fixture.Toolkit;

namespace XPFriend.FixtureTest.Toolkit
{
    [TestClass]
    public class StackFrameFinderTest
    {
        [TestMethod]
        public void Findは指定されたクラスのStackFrameを取得する()
        {
            CallerCaller.Call1();
        }
    }

    internal class CallerCaller
    {
        public static void Call1()
        {
            new FinderCaller().Call2();
        }
    }

    internal class FinderCaller
    {
        public void Call2()
        {
            List<StackFrame> frames = StackFrameFinder.Find(typeof(StackFrameFinderTest));
            foreach (StackFrame frame in frames)
            {
                Console.WriteLine("called : " + frame);
                Assert.AreEqual("Findは指定されたクラスのStackFrameを取得する", frame.GetMethod().Name);
            }
        }
    }
}
