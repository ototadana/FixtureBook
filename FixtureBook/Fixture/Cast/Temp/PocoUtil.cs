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
using System.Collections.Generic;
using System.Reflection;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class Properties
    {
        private Dictionary<string, PropertyInfo> properties = 
            new Dictionary<string,PropertyInfo>();

        public Properties(Type type)
        {
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                properties[propertyInfos[i].Name.ToLower()] = propertyInfos[i];
            }
        }

        public PropertyInfo this[string name] 
        {
            get { return properties[name.ToLower()]; }
        }
    }
    
    internal class PocoUtil
    {
        private Dictionary<Type, Properties> properties = new Dictionary<Type,Properties>();

        public Properties this[Type type]
        {
            get {
                Properties p = null;
                properties.TryGetValue(type, out p);
                if (p == null)
                {
                    p = new Properties(type);
                    properties[type] = p;
                }
 
                return p; 
            }
        }
    }
}
