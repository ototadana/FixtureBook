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
using System.Globalization;
using System.Resources;

namespace XPFriend.Junk.Temp
{
    internal class ResourceProvider
    {
        private List<ResourceManager> resourceManagers = new List<ResourceManager>();

        public static ResourceProvider Instance
        {
            get { return Classes.NewInstance<ResourceProvider>();}
        }

        public virtual void Add(ResourceManager resourceManager) 
        {
            if (resourceManagers.Contains(resourceManager))
            {
                resourceManagers.Remove(resourceManager);
            }
            resourceManagers.Add(resourceManager);
        }

        public virtual string Get(CultureInfo culture, string key, string defaultValue)
        {
            for (int i = resourceManagers.Count - 1; i >= 0; i--)
            {
                try
                {
                    string value = resourceManagers[i].GetString(key, culture);
                    if (value != null)
                    {
                        return value;
                    }
                }
                catch (Exception e)
                {
                    ExceptionHandler.Ignore(e);
                }
            }
            return defaultValue;
        }

        public virtual void Initialize()
        {
            resourceManagers.Clear();
        }
    }
}
