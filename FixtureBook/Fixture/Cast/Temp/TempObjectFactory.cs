using System.Collections;
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
using System.Collections.Generic;
using XPFriend.Fixture.Role;
using XPFriend.Fixture.Staff;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class TempObjectFactory : IObjectFactory
    {
        internal DictionaryFactory dictionaryFactory;
        internal PocoFactory pocoFactory;
        internal DataSetFactory dataSetFactory;
        internal DataTableFactory dataTableFactory;

        public TempObjectFactory()
        {
            dictionaryFactory = new DictionaryFactory(this);
            pocoFactory = new PocoFactory(this);
            dataSetFactory = new DataSetFactory(this);
            dataTableFactory = new DataTableFactory(this);
        }

        public void Initialize(Case testCase)
        {
            dictionaryFactory.Initialize(testCase);
            pocoFactory.Initialize(testCase);
            dataSetFactory.Initialize(testCase);
            dataTableFactory.Initialize(testCase);
        }

        public bool HasRole<T>(params string[] typeName)
        {
            return pocoFactory.HasRole<T>(typeName) ||
                dataSetFactory.HasRole<T>(typeName) ||
                dataTableFactory.HasRole<T>(typeName) ||
                dictionaryFactory.HasRole<T>(typeName);
        }

        public T GetObject<T>(params string[] typeName)
        {
            return getFactory<T>(typeName).GetObject<T>(typeName);
        }

        public List<T> GetList<T>(string typeName)
        {
            return getFactory<T>(typeName).GetList<T>(typeName);
        }

        public T[] GetArray<T>(string typeName)
        {
            return getFactory<T>(typeName).GetArray<T>(typeName);
        }

        public void AddTo<T>(IList list, string typeName)
        {
            getFactory<T>(typeName).AddTo<T>(list, typeName);
        }

        private ObjectFactoryBase getFactory<T>(params string[] typeName)
        {
            if (dataSetFactory.HasRole<T>(typeName))
            {
                return dataSetFactory;
            }

            if (dataTableFactory.HasRole<T>(typeName))
            {
                return dataTableFactory;
            }

            if (dictionaryFactory.HasRole<T>(typeName))
            {
                return dictionaryFactory;
            }

            return pocoFactory;
        }

        public Section.SectionType SectionType 
        {
            set
            {
                pocoFactory.SectionType = value;
                dataSetFactory.SectionType = value;
                dataTableFactory.SectionType = value;
                dictionaryFactory.SectionType = value;
            }
        }
    }
}
