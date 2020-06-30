using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace Loaders
{
    public abstract class DbLoader
    {
        public void Init(string DbPath)
        {
            SQLiteConnection connect = new SQLiteConnection(DbPath);
            connect.Open();
            var cmd = new SQLiteCommand(connect);
            DataTable dataTable = new DataTable();


            //cmd.CommandText = @"CREATE TABLE cars(id INTEGER PRIMARY KEY, name TEXT, price INT)";
            //cmd.CommandText = @"INSERT INTO cars(name, price) VALUES('Audi', 52642)";
            //cmd.CommandText = @"INSERT INTO cars(name, price) VALUES(@name,@price)";
            //cmd.Parameters.AddWithValue("@name", "BMW");
            //cmd.Parameters.AddWithValue("@price", 7);
            //cmd.CommandText = @"INSERT INTO cars VALUES(9,'TOYOTA',654)";
            //cmd.CommandText = @"INSERT INTO cars VALUES(79,'TOYOTA4',654)";

            //cmd.CommandText = @"INSERT INTO cars VALUES(@id, @name, @price)";
            //cmd.Parameters.AddWithValue("@id", 199);
            //cmd.Parameters.AddWithValue("@name", "KIA");
            //cmd.Parameters.AddWithValue("@price", 132);
            //cmd.ExecuteNonQuery();

            cmd.CommandText = @"SELECT * FROM messages_fts_segdir";
            //cmd.CommandText = @"SELECT * FROM messages_fts_stat";
            //cmd.CommandText = @"SELECT * FROM cars";

            SQLiteDataReader rdr = cmd.ExecuteReader();
            dataTable.Load(rdr);

            List<string> columnList = GetColumnList(dataTable.Columns);
            string cloneQuery = CreateCloneQuery(dataTable.TableName, columnList);
            cmd.CommandText = cloneQuery;

            foreach (DataRow row in dataTable.Rows)
            {
                
                for (int i = 0; i < columnList.Count; i++)
                {
                    var value = row.ItemArray[i];
                    var colName = columnList[i];

                    cmd.Parameters.AddWithValue(colName, value);
                }
                cmd.ExecuteNonQuery();
            }
        }

        private List<string> GetColumnList(DataColumnCollection dataColumnCollection)
        {
            var columnList = new List<string>();
            foreach (var colName in dataColumnCollection)
            {
                columnList.Add("@" + colName.ToString());
            }

            return columnList;
        }
        private string CreateCloneQuery(string tableName, List<string> columnList)
        {
            StringBuilder cloneQuery = new StringBuilder();
            cloneQuery.Append($"INSERT INTO {tableName} VALUES(");
            bool firstValue = true;
            foreach (string colName in columnList)
            {
                if (!firstValue)
                {
                    cloneQuery.Append(",");
                }
                else
                {
                    firstValue = false;
                }

                cloneQuery.Append(colName);
            }

            cloneQuery.Append(")");
            return cloneQuery.ToString();
        }

        private object GetManipulateValue(object value, string columnName)
        {
            return value;
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
    }
}