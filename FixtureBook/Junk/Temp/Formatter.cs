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

namespace XPFriend.Junk.Temp
{
    internal class Formatter
    {
        private String defaultTimestampFormat;

        public static Formatter Instance 
        {
            get { return Classes.NewInstance<Formatter>(); }
        }

        public Formatter()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            defaultTimestampFormat = Config.Get("Junk.Formi.defaultTimestampFormat", "yyyy-MM-dd HH:mm:ss");
        }

        public virtual string DefaultTimestampFormat
        {
            get { return defaultTimestampFormat; }
        }

        public virtual string Format(DateTime dateTime, string formatText, TimeZoneInfo timeZone)
        {
            if (timeZone != null)
            {
                dateTime = TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local, timeZone);
            }
            return dateTime.ToString(formatText);
        }
    }
}
