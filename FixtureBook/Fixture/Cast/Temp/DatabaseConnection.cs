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
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    /// <summary>
    /// データベースコネクション。
    /// </summary>
    internal class DatabaseConnection : IDisposable
    {
        internal const string Default = "____";

        private Dictionary<string, InternalDatabaseConnection> connections = new Dictionary<string, InternalDatabaseConnection>();
        private InternalDatabaseConnection currentConnection;

        public char ParameterPrefix { get { return currentConnection.parameterPrefix; } }
        public DbProviderFactory ProviderFactory { get { return currentConnection.factory; } }

        private InternalDatabaseConnection GetInstance()
        {
            if (currentConnection == null)
            {
                currentConnection = GetConnection(Default);
            }
            return currentConnection;
        }

        public void Use(string databaseName)
        {
            if (databaseName == null)
            {
                databaseName = Default;
            }
            currentConnection = GetConnection(databaseName);
        }

        private InternalDatabaseConnection GetConnection(string databaseName)
        {
            if (connections.ContainsKey(databaseName))
            {
                return connections[databaseName];
            }

            InternalDatabaseConnection connection = 
                new InternalDatabaseConnection(databaseName);
            connections[databaseName] = connection;
            return connection;
        }

        public bool IsSQLServer
        {
            get
            {
                return GetInstance().IsSQLServer;
            }
        }

        public void Commit()
        {
            foreach (InternalDatabaseConnection connection in connections.Values)
            {
                connection.Commit();
            }
        }

        public void Dispose()
        {
            foreach (InternalDatabaseConnection connection in connections.Values)
            {
                connection.Dispose();
            }
        }

        public DbCommand CreateCommand()
        {
            return GetInstance().CreateCommand();
        }

        public DbCommand CreateCommand(string commandText)
        {
            return GetInstance().CreateCommand(commandText);
        }
    }

    internal class InternalDatabaseConnection
    {
        private DbConnection connection;
        private DbTransaction transaction;

        internal char parameterPrefix = '@';
        private string provider;
        private string connectionString;
        internal DbProviderFactory factory;

        internal InternalDatabaseConnection(string databaseName)
        {
            InitializeProvider(databaseName);
            Connect();
        }

        private void InitializeProvider(string databaseName)
        {
            ConnectionStringSettings settings;
            if (DatabaseConnection.Default.Equals(databaseName))
            {
                if (DatabaseConnectionManager.ConnectionStrings.Count == 0)
                {
                    throw new ConfigException("M_Fixture_Temp_DatabaseConnection_NoConnectionSettings");
                }
                settings = DatabaseConnectionManager.ConnectionStrings[0];
            }
            else
            {
                settings = DatabaseConnectionManager.Find(databaseName);
                if (settings == null)
                {
                    throw new ConfigException("M_Fixture_Temp_DatabaseConnection_NoSuchName", databaseName);
                }
            }

            provider = settings.ProviderName;
            connectionString = settings.ConnectionString;
            Loggi.Debug("Connection: " + settings.Name + ", providerName=" + provider + ", connectionString=" + connectionString);

            factory = DbProviderFactories.GetFactory(provider);
            if (provider.IndexOf("Oracle") > -1)
            {
                parameterPrefix = ':';
            }
            else
            {
                IsSQLServer = true;
            }
        }

        internal bool IsSQLServer { get; set; }

        private void Connect()
        {
            connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        internal void Commit()
        {
            transaction.Commit();
            transaction.Dispose();
            transaction = connection.BeginTransaction();
        }

        internal void Dispose()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }

            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }

        internal DbCommand CreateCommand()
        {
            DbCommand command = factory.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            return command;
        }

        internal DbCommand CreateCommand(string commandText)
        {
            DbCommand command = CreateCommand();
            command.CommandText = commandText;
            return command;
        }
    }
}
