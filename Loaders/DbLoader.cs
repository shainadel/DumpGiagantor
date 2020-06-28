using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Loaders
{
    public abstract class DbLoader
    {
        public void Init(string DbPath)
        {
            SqliteConnection _SQL = new SqliteConnection(DbPath);

            _SQL.Open();
            
            SqliteCommand cmd = new SqliteCommand();
        }

    public abstract void PumpTable(RecordManipulationLogic table);

    }
}