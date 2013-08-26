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
using System.Diagnostics;

namespace XPFriend.Fixture.Toolkit
{
    /// <summary>
    /// 現在のメソッドを呼び出しているクラス・メソッドを取得する為のユーティリティ。
    /// </summary>
    internal class StackFrameFinder
    {
        private static readonly string MyClassName = typeof(StackFrameFinder).FullName;
        private const int MAX = 20;

        /// <summary>
        /// 指定されたクラスのStackFrameを取得する。
        /// </summary>
        /// <returns>StackFrame</returns>
        public static List<StackFrame> Find(Type type)
        {
            List<StackFrame> list = new List<StackFrame>();
            string className = type.FullName;
            for (int i = 0; i < MAX; i++)
            {
                StackFrame stackFrame = new StackFrame(i, true);
                if (stackFrame.GetMethod() == null)
                {
                    break;
                }
                string stackFrameClassName = stackFrame.GetMethod().ReflectedType.FullName;
                if (stackFrameClassName.Equals(className))
                {
                    list.Add(stackFrame);
                }
                else if(list.Count > 0)
                {
                    break;
                }
            }
            if (list.Count == 0)
            {
                throw new InvalidOperationException("cannot find a StackFrame of " + type.FullName);
            }
            return list;
        }

        /// <summary>
        /// 指定されたクラスの呼び出し元の StackFrame を取得する。
        /// </summary>
        public static List<StackFrame> FindCaller(Type calleeClass)
        {
            string calleeClassName = calleeClass.FullName;
            for (int i = 1; i < MAX; i++)
            {
                StackFrame stackFrame = new StackFrame(i);
                Type type = stackFrame.GetMethod().ReflectedType;
                string className = type.FullName;
                if (!MyClassName.Equals(className) && !calleeClassName.Equals(className))
                {
                    return Find(type);
                }
            }
            throw new InvalidOperationException("cannot find caller of " + calleeClass.FullName);
        }
    }
}
