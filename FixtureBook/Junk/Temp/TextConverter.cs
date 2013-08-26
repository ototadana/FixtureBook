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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XPFriend.Junk.Temp
{
    internal class TextConverter
    {
        public static readonly string TimestampFormat = Formi.DefaultTimestampFormat;

        public static TextConverter Instance
        {
            get { return Classes.NewInstance<TextConverter>(); }
        }

        public virtual string ToString(object obj) 
        {
            if (obj == null) { return ""; }
            else if (obj is string) { return (string)obj; }
            else if (obj is DateTime) { return Formi.FormatTimestamp((DateTime)obj); }
            else if (obj is IEnumerable) { return IEnumerableToString((IEnumerable)obj); }
            else { return obj.ToString(); }
        }

        protected virtual string IEnumerableToString(IEnumerable list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object obj in list)
            {
                sb.Append(ToString(obj));
                sb.Append("|");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }
}
