/*
 * Copyright 2014 XPFriend Community.
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
using System.Configuration;
using System.Linq;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class DatabaseConnectionManager
    {
        static DatabaseConnectionManager()
        {
            List<ConnectionStringSettings> connectionStrings = new List<ConnectionStringSettings>();
            foreach (ConnectionStringSettings strings in ConfigurationManager.ConnectionStrings)
            {
                connectionStrings.Add(strings);
            }
            ConnectionStrings = connectionStrings;
        }

        public static List<ConnectionStringSettings> ConnectionStrings { get; set; }
        public static ConnectionStringSettings Find(string name)
        {
            try
            {
                return ConnectionStrings.First((settings) => settings.Name == name);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
