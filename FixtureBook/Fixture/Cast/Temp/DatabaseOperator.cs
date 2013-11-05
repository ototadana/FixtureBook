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
using System.Data;
using XPFriend.Fixture.Staff;

namespace XPFriend.Fixture.Cast.Temp
{
    internal abstract class DatabaseOperator
    {
        protected Case testCase;
        protected TempObjectFactory tempObjectFactory;

        public virtual void Initialize(Case testCase)
        {
            this.testCase = testCase;
            this.tempObjectFactory = new TempObjectFactory();
            this.tempObjectFactory.Initialize(testCase);
        }

        protected virtual DataSet GetDataSet(Section.SectionType sectionType)
        {
            string[] tableNames = testCase.GetSection(sectionType).TableNames.ToArray();
            tempObjectFactory.SectionType = sectionType;
            return tempObjectFactory.GetObject<DataSet>(tableNames);
        }
    }
}
