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
using System;
using System.Collections.Generic;
namespace XPFriend.FixtureTest.Cast.Temp.Datas
{
    public class Data
    {
        public string Text1 { get; set; }
        public string Text2 { get { return ""; } }
        public int Number1 { get; set; }
        public int? Number2 { get; set; }
        public DateTime Date1 { get; set; }
        public byte[] Bytes { get; set; }
        public IList<Data> List { get; set; }

        public override string ToString()
        {
            return Text1 + "," + Number1;
        }
    }
}
