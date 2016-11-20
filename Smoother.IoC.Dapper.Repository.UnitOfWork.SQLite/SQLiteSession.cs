﻿using System.Data.SQLite;
using Dapper.FastCrud;
using Smoother.IoC.Dapper.Repository.UnitOfWork.Data;

namespace Smoother.IoC.Dapper.Repository.UnitOfWork.SQLite
{
    public class SqliteSession : Session, ISession
    {
        public SqliteSession(IDbFactory factory,string connectionString ) : base(factory, SqlDialect.SqLite )
        {
            if (factory != null && !string.IsNullOrWhiteSpace(connectionString))
            {
                Connect(connectionString);
            }
        }

        private void Connect(string connectionString)
        {
            if (Connection != null)
            {
                return;
            }
            Connection = new SQLiteConnection(connectionString);
            Connection?.Open();
        }
    }
}