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
using System.Collections.Specialized;
using System.Configuration;

namespace XPFriend.Junk.Temp
{
    internal class ClassManager
    {
        private Dictionary<Type, Type> classes = new Dictionary<Type, Type>();

        public ClassManager()
        {
            Initialize();
        }

        public virtual Type this[Type baseClass]
        {
            get 
            { 
                Type implClass = null;
                classes.TryGetValue(baseClass, out implClass);
                return implClass; 
            }
            set { classes[baseClass] = value; }
        }

        public virtual void Initialize()
        {
            classes.Clear();
            ConfigurationManager.RefreshSection("xjc.classes");
            NameValueCollection collection;
            try
            {
                collection = (NameValueCollection)ConfigurationManager.GetSection("xjc.classes");
                if (collection == null)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Ignore(e);
                return;
            }

            foreach (string key in collection)
            {
                Type baseClass = GetType(key);
                Type implClass = GetType(collection[key]);
                classes[baseClass] = implClass;
            }
        }

        private Type GetType(string typeName)
        {
            typeName = typeName.Trim();
            Type t = Types.GetType(typeName);
            if (t == null)
            {
                throw new SystemException("invalid classname : " + typeName);
            }
            return t;
        }
    }
}
