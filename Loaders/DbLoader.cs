using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Loaders
{
    public abstract class DbLoader
    {
        public void Init(string DbPath)
        {
            SqliteConnection _SQL = new SqliteConnection(DbPath);

            _SQL.Open();

            SqliteCommand cmd = new SqliteCommand();
            // TODO: 

        }

        public void Parse(IEnumerable<TableRecordManipulationLogic> tablesRecorsdManipulationLogic)
        {
            foreach (var tableRecorsdManipulationLogic in tablesRecorsdManipulationLogic)
            {
                PumpTable(tableRecorsdManipulationLogic);
            }
        }

        public void Parse(TableRecordManipulationLogic tableRecorsdManipulationLogic)
        {
            PumpTable(tableRecorsdManipulationLogic);
        }

        public void PumpTable(TableRecordManipulationLogic tableRecorsdManipulationLogic)
        {
            // TODO
        }

    }
}