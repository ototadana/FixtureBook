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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using XPFriend.Junk;

namespace XPFriend.JunkTest
{
    internal class TestTraceListener : TraceListener
    {
        private string prefix;
        private string message;

        public override void Write(string message)
        {
            this.prefix = message;
        }

        public override void WriteLine(string message)
        {
            this.message = message;
        }

        public void AssertMessage(string level, string message, Exception exception)
        {
            if (level == null)
            {
                Assert.IsNull(prefix);
            }
            else
            {
                Assert.IsTrue(prefix.IndexOf(level) > -1);
            }

            if (exception == null)
            {
                Assert.AreEqual(message, this.message);
            }
            else
            {
                int lineSeparatorIndex = this.message.IndexOf(Environment.NewLine);
                Assert.IsTrue(lineSeparatorIndex > -1);
                Assert.IsTrue(this.message.Length > lineSeparatorIndex + 1);
                Assert.AreEqual(message, this.message.Substring(0, lineSeparatorIndex));
                Assert.AreEqual(exception.ToString(), this.message.Substring(lineSeparatorIndex + Environment.NewLine.Length));
            }
        }
    }

    [TestClass]
    public class LoggiTest
    {
        private TestTraceListener traceListener = new TestTraceListener();

        [TestInitialize]
        public void Setup()
        {
            Loggi.DebugEnabled = false;
            Trace.Listeners.Add(traceListener);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Trace.Listeners.Remove(traceListener);
            Loggi.DebugEnabled = false;
        }

        [TestMethod]
        public void Info_stringはInformationレベルでログ出力を行う()
        {
            // when
            Loggi.Info("aaa");

            // then
            traceListener.AssertMessage("Information", "aaa", null);
        }

        [TestMethod]
        public void Info_ExceptionはInformationレベルでログ出力を行う()
        {
            // setup
            try
            {
                throwException("aaa");
            }
            catch (Exception e)
            {
                // when
                Loggi.Info(e);

                // then
                traceListener.AssertMessage("Information", "aaa", e);
            }
        }

        [TestMethod]
        public void Info_string_ExceptionはInformationレベルでログ出力を行う()
        {
            // setup
            try
            {
                throwException("eee");
            }
            catch (Exception e)
            {
                // when
                Loggi.Info("aaa", e);

                // then
                traceListener.AssertMessage("Information", "aaa", e);
            }
        }


        [TestMethod]
        public void Warn_stringはWarningレベルでログ出力を行う()
        {
            // when
            Loggi.Warn("aaa");

            // then
            traceListener.AssertMessage("Warning", "aaa", null);
        }

        [TestMethod]
        public void Warn_ExceptionはWarningレベルでログ出力を行う()
        {
            // setup
            try
            {
                throwException("aaa");
            }
            catch (Exception e)
            {
                // when
                Loggi.Warn(e);

                // then
                traceListener.AssertMessage("Warning", "aaa", e);
            }
        }

        [TestMethod]
        public void Warn_string_ExceptionはWarningレベルでログ出力を行う()
        {
            // setup
            try
            {
                throwException("eee");
            }
            catch (Exception e)
            {
                // when
                Loggi.Warn("aaa", e);

                // then
                traceListener.AssertMessage("Warning", "aaa", e);
            }
        }


        [TestMethod]
        public void Error_stringはErrorレベルでログ出力を行う()
        {
            // when
            Loggi.Error("aaa");

            // then
            traceListener.AssertMessage("Error", "aaa", null);
        }

        [TestMethod]
        public void Error_ExceptionはErrorレベルでログ出力を行う()
        {
            // setup
            try
            {
                throwException("aaa");
            }
            catch (Exception e)
            {
                // when
                Loggi.Error(e);

                // then
                traceListener.AssertMessage("Error", "aaa", e);
            }
        }

        [TestMethod]
        public void Error_string_ExceptionはErrorレベルでログ出力を行う()
        {
            // setup
            try
            {
                throwException("eee");
            }
            catch (Exception e)
            {
                // when
                Loggi.Error("aaa", e);

                // then
                traceListener.AssertMessage("Error", "aaa", e);
            }
        }


        [TestMethod]
        public void Debug_stringはDebugEnabledがtrueの場合ログ出力を行う()
        {
            // setup
            Loggi.DebugEnabled = true;

            // when
            Loggi.Debug("aaa");

            // then
            traceListener.AssertMessage(null, "aaa", null);
        }

        [TestMethod]
        public void Debug_ExceptionはDebugEnabledがtrueの場合ログ出力を行う()
        {
            // setup
            Loggi.DebugEnabled = true;
            try
            {
                throwException("aaa");
            }
            catch (Exception e)
            {
                // when
                Loggi.Debug(e);

                // then
                traceListener.AssertMessage(null, "aaa", e);
            }
        }

        [TestMethod]
        public void Debug_string_ExceptionはDebugEnabledがtrueの場合ログ出力を行う()
        {
            // setup
            Loggi.DebugEnabled = true;
            try
            {
                throwException("eee");
            }
            catch (Exception e)
            {
                // when
                Loggi.Debug("aaa", e);

                // then
                traceListener.AssertMessage(null, "aaa", e);
            }
        }

        [TestMethod]
        public void DebugEnabledはtrueをセットするとtrueを返す()
        {
            // when
            Loggi.DebugEnabled = true;

            // then
            Assert.IsTrue(Loggi.DebugEnabled);

            // when
            Loggi.DebugEnabled = false;

            // then
            Assert.IsFalse(Loggi.DebugEnabled);
        }

        [TestMethod]
        public void Debug_stringはDebugEnabledがfalseの場合ログ出力を行わない()
        {
            // setup
            Loggi.DebugEnabled = false;

            // when
            Loggi.Debug("aaa");

            // then
            traceListener.AssertMessage(null, null, null);
        }

        [TestMethod]
        public void Debug_ExceptionはDebugEnabledがfalseの場合ログ出力を行わない()
        {
            // setup
            Loggi.DebugEnabled = false;
            try
            {
                throwException("aaa");
            }
            catch (Exception e)
            {
                // when
                Loggi.Debug(e);

                // then
                traceListener.AssertMessage(null, null, null);
            }
        }

        [TestMethod]
        public void Debug_string_ExceptionはDebugEnabledがfalseの場合ログ出力を行わない()
        {
            // setup
            Loggi.DebugEnabled = false;
            try
            {
                throwException("eee");
            }
            catch (Exception e)
            {
                // when
                Loggi.Debug("aaa", e);

                // then
                traceListener.AssertMessage(null, null, null);
            }
        }

        private void throwException(string message)
        {
            throw new Exception(message);
        }
    }
}
