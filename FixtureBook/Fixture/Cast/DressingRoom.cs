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

namespace XPFriend.Fixture.Cast
{
    /// <summary>
    /// アクターの待機場所。
    /// </summary>
    internal class DressingRoom
    {
        private List<IObjectFactory> objectFactories = new List<IObjectFactory>();
        private List<IObjectValidator> objectValidators = new List<IObjectValidator>();
        private List<IStorageUpdater> storageUpdaters = new List<IStorageUpdater>();
        private List<IStorageValidator> storageValidators = new List<IStorageValidator>();

        public IConductor Conductor { get; set; }

        /// <summary>
        /// <see cref="IObjectFactory"/> のリスト。
        /// </summary>
        public List<IObjectFactory> ObjectFactories { get { return objectFactories; } }

        /// <summary>
        /// <see cref="IObjectValidator"/> のリスト。
        /// </summary>
        public List<IObjectValidator> ObjectValidators { get { return objectValidators; } }

        /// <summary>
        /// <see cref="IStorageUpdater"/> のリスト。
        /// </summary>
        public List<IStorageUpdater> StorageUpdaters { get { return storageUpdaters; } }

        /// <summary>
        /// <see cref="IStorageValidator"/> のリスト。
        /// </summary>
        public List<IStorageValidator> StorageValidators { get { return storageValidators; } }
    }
}
