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
            int numofIntacts = tableRecorsdManipulationLogic.Intacts;
            int numofDeletedes = tableRecorsdManipulationLogic.Deletedes;
            string tableName = tableRecorsdManipulationLogic.TableName;
            // load the table from DB

            //for (Record rec in table)
            //{
            //    //lastRec = rec
            //    for (int i = 0; i < numofIntacts + numofDeletedes; i++)
            //    {
            //        //bool isIntact/isDeleted = //
            //        // newRec = UpdateRecord(lastRec,tableRecorsdManipulationLogic)
            //        // Insert to list dbRecords / dbDeletedRecoreds
            //        // lastRec=newRec
            //    }
            //}

            //for (rec in dbRecords)
            //{
            //    //add
            //}

            //for (rec in dbDeletedRecoreds)
            //{
            //    //add
            //    //delete
            //}


        }
        public void UpdateRecord(List<string> lastRec,TableRecordManipulationLogic logic)
        {

        }
    }
}