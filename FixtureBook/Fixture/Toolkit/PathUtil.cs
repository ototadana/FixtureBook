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
using System.IO;
using XPFriend.Junk;

namespace XPFriend.Fixture.Toolkit
{
    internal class PathUtil
    {
        internal const string TopDirectoryKey = "Fixture.TestContext.topDirectory";
        internal const string DefaultTopDirectory = @"..\..";
        private static string topDirectory;

        static PathUtil()
        {
            InitTopDirectory();
        }

        private static void InitTopDirectory()
        {
            topDirectory = Config.Get(TopDirectoryKey, DefaultTopDirectory);
        }

        public static string EditFilePath(string filePath)
        {
            if (File.Exists(filePath))
            {
                return filePath;
            }
            return Path.Combine(topDirectory, filePath);
        }

        public static string GetFilePath(string textValue)
        {
            try
            {
                if (File.Exists(textValue))
                {
                    return textValue;
                }
                string filePath = EditFilePath(textValue);
                if (File.Exists(filePath))
                {
                    return filePath;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Ignore(e);
            }
            return null;
        }
    }
}
