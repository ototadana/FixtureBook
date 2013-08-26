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
using System.Diagnostics;

namespace XPFriend.Junk.Temp
{
    internal class LoggiLogger
    {
        private TraceSwitch traceSwitch = new TraceSwitch("TraceSwitch", "TraceSwitch");

        public static LoggiLogger Instance
        {
            get { return Classes.NewInstance<LoggiLogger>(); }
        }

        public virtual void Debug(string message)
        {
            Trace.WriteLineIf(traceSwitch.TraceVerbose, message);
        }

        public virtual void Debug(Exception e)
        {
            Trace.WriteLineIf(traceSwitch.TraceVerbose, e.Message + Environment.NewLine + e.ToString());
        }

        public virtual void Debug(string message, Exception e)
        {
            Trace.WriteLineIf(traceSwitch.TraceVerbose, message + Environment.NewLine + e.ToString());
        }

        public virtual bool DebugEnabled {
            get {return traceSwitch.TraceVerbose;}
            set 
            {
                if (value)
                {
                    traceSwitch.Level = TraceLevel.Verbose;
                }
                else
                {
                    traceSwitch.Level = TraceLevel.Info;
                }
            }
        }

        public virtual void Info(string message)
        {
            Trace.TraceInformation(message);
        }

        public virtual void Info(Exception e)
        {
            Trace.TraceInformation(e.Message + Environment.NewLine + e.ToString());
        }

        public virtual void Info(string message, Exception e)
        {
            Trace.TraceInformation(message + Environment.NewLine + e.ToString());
        }

        public virtual void Warn(string message)
        {
            Trace.TraceWarning(message);
        }

        public virtual void Warn(Exception e)
        {
            Trace.TraceWarning(e.Message + Environment.NewLine + e.ToString());
        }

        public virtual void Warn(string message, Exception e)
        {
            Trace.TraceWarning(message + Environment.NewLine + e.ToString());
        }

        public virtual void Error(string message)
        {
            Trace.TraceError(message);
        }

        public virtual void Error(Exception e)
        {
            Trace.TraceError(e.Message + Environment.NewLine + e.ToString());
        }

        public virtual void Error(string message, Exception e)
        {
            Trace.TraceError(message + Environment.NewLine + e.ToString());
        }
    }
}
