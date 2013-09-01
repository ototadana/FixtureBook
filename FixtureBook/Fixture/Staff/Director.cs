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
using XPFriend.Fixture.Staff.Xlsx;
using XPFriend.Fixture.Cast;
using XPFriend.Fixture.Cast.Temp;
using XPFriend.Fixture.Role;

namespace XPFriend.Fixture.Staff
{
    /// <summary>
    /// ディレクタ。
    /// <see cref="Author"/>および<see cref="IActor"/>のファクトリ。
    /// </summary>
    internal class Director
    {
        /// <summary>
        /// ディレクタを作成する。
        /// </summary>
        public Director()
        {
        }

        /// <summary>
        /// ブックの作者をアサインする。
        /// </summary>
        /// <param name="book">ブック</param>
        /// <returns>ブックの作者</returns>
        protected internal virtual Author AssignAuthor(Book book)
        {
            return new XlsxAuthor();
        }

        /// <summary>
        /// 指定されたシートにふさわしい <see cref="IStorageUpdater"/> をアサインする。
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns>シート</returns>
        public virtual List<IStorageUpdater> AssignStorageUpdaters(Sheet sheet)
        {
            IStorageUpdater databaseUpdater = new DatabaseUpdater();
            databaseUpdater.Initialize(sheet);
            List<IStorageUpdater> list = new List<IStorageUpdater>(1);
            list.Add(databaseUpdater);
            return list;
        }

        /// <summary>
        /// 指定されたテストケースにふさわしいアクターをアサインする。
        /// </summary>
        /// <param name="testCase">テストケース</param>
        /// <returns>アクター</returns>
        public virtual DressingRoom AssignActors(Case testCase)
        {
            DressingRoom dressingRoom = new DressingRoom();

            dressingRoom.StorageUpdaters.Add(Initialize(new DatabaseUpdater(), testCase));
            dressingRoom.StorageValidators.Add(Initialize(new DatabaseValidator(), testCase));
            dressingRoom.ObjectFactories.Add(Initialize(new TempObjectFactory(), testCase));
            dressingRoom.ObjectValidators.Add(Initialize(new TempObjectValidator(), testCase));
            dressingRoom.Conductor = Initialize(new TempConductor(), testCase);

            return dressingRoom;
        }

        private T Initialize<T>(T actor, Case testCase) where T : IActor
        {
            actor.Initialize(testCase);
            return actor;
        }

    }
}
