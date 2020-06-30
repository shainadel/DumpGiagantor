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
            int numofIntactsPerRecord = tableRecorsdManipulationLogic.Intacts;
            int numofDeletedesPerRecord = tableRecorsdManipulationLogic.Deletedes;
            string tableName = tableRecorsdManipulationLogic.TableName;
            string primaryKey = tableRecorsdManipulationLogic.PrimaryKey;

            var manipulationArgsLong = tableRecorsdManipulationLogic.ManipulationArgsLong;
            var manipulationArgsString = tableRecorsdManipulationLogic.ManipulationArgsString;


            //_cmd.CommandText =;


            long maxValuePrimaryKey = 1 + GetMaxValuePrimaryKey(tableName, primaryKey);

            _cmd.CommandText = $@"SELECT * FROM {tableName}";
            var dataTable = new DataTable();

            SQLiteDataReader rdr = _cmd.ExecuteReader();
            dataTable.Load(rdr);

            List<string> columnList = GetColumnList(dataTable.Columns);
            string cloneQuery = CreateCloneQuery(dataTable.TableName, columnList);
            _cmd.CommandText = cloneQuery;

            long numofDeletedes = numofDeletedesPerRecord * dataTable.Rows.Count;

            Dictionary<string, object> lastRec = new Dictionary<string, object>();
            List<long> deletedPrimaryKeys = new List<long>();

            foreach (DataRow rec in dataTable.Rows)
            {
                var pumpRecord = CreatePumpRecord(rec, columnList);

                for (int k = 0; k < numofIntactsPerRecord + numofDeletedesPerRecord; k++)
                {
                    foreach (var colNameValue in pumpRecord)
                    {
                        var colName = colNameValue.Key;
                        var value = colNameValue.Value;
                        string type = value.GetType().Name;

                        if (colName == "@"+primaryKey)
                        {
                            value = maxValuePrimaryKey++;
                            if(k>=numofIntactsPerRecord)
                                deletedPrimaryKeys.Add((long)value);
                        }
                        else if (type == "Int64" && manipulationArgsLong.ContainsKey(colName))
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
            foreach (long key in deletedPrimaryKeys)
            {
                _cmd.CommandText = $@"delete from {tableName} where {primaryKey} = {key}";
                _cmd.ExecuteNonQuery();
            }
        }

        private long GetMaxValuePrimaryKey(string tableName, string primaryKey)
        {
            _cmd.CommandText = $@"SELECT MAX({primaryKey}) FROM {tableName}";

            var dataTable = new DataTable();
            SQLiteDataReader rdrMaxValue = _cmd.ExecuteReader();
            dataTable.Load(rdrMaxValue);
            var maxValue = dataTable.Rows[0].ItemArray[0];
            return (long)maxValue;
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