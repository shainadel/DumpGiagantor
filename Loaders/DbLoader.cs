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
        private SQLiteCommand _cmd;

        public void Init(string DbPath)
        {
            SQLiteConnection connect = new SQLiteConnection(DbPath);
            connect.Open();
            _cmd = new SQLiteCommand(connect);


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

            //cmd.CommandText = @"SELECT * FROM messages_fts_segdir";
            //cmd.CommandText = @"SELECT * FROM messages_fts_stat";
           
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

            var manipulationArgsLong = tableRecorsdManipulationLogic.ManipulationArgsLong;
            var manipulationArgsString = tableRecorsdManipulationLogic.ManipulationArgsString;
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

            //_cmd.CommandText = @"SELECT * FROM cars";
            _cmd.CommandText = $@"SELECT * FROM {tableName}";
            var dataTable = new DataTable();

            SQLiteDataReader rdr = _cmd.ExecuteReader();
            dataTable.Load(rdr);

            List<string> columnList = GetColumnList(dataTable.Columns);
            string cloneQuery = CreateCloneQuery(dataTable.TableName, columnList);
            _cmd.CommandText = cloneQuery;

            //int j = 10000;
            Dictionary<string, object> lastRec = new Dictionary<string, object>();
            foreach (DataRow rec in dataTable.Rows)
            {
                var pumpRecord = CreatePumpRecord(rec, columnList);

                for (int k = 0; k < numofIntacts; k++)
                {
                    foreach (var colNameValue in pumpRecord)
                    {
                        var colName = colNameValue.Key;
                        var value = colNameValue.Value;
                        string type = value.GetType().Name;

                        if (type == "Int64" && manipulationArgsLong.ContainsKey(colName))
                        {
                            value = manipulationArgsLong[colName](value);
                        }
                        else if (type == "String" && manipulationArgsString.ContainsKey(colName))
                        {
                            value = manipulationArgsString[colName](value);
                        }

                        lastRec[colName] = value;

                        _cmd.Parameters.AddWithValue(colName, value);
                    }

                    pumpRecord = new Dictionary<string, object>(lastRec);
                    
                    _cmd.ExecuteNonQuery();
                }

            }
        }

        private Dictionary<string, object> CreatePumpRecord(DataRow rec, List<string> columnList)
        {
            var pumpRecord = new Dictionary<string, object>();
            for (int i = 0; i < columnList.Count; i++)
            {
                var colName = columnList[i];
                var value = rec.ItemArray[i];
                pumpRecord[colName] = value;
            }

            return pumpRecord;
        }
    }
}