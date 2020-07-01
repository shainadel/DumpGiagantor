using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using System.Text;
namespace Loaders
{
    public abstract class DbLoader
    {
        SQLiteConnection _connection;
        private SQLiteConnection _memoryConnection;
        private SQLiteCommand _cmd;
        protected string _DBPathInsideZip;
        protected string _zipPath;
        private string _DBName;
        protected string _DBPathInDisk;
        private FileStream _fileStream;
        public void Init()
        {
            if (_zipPath != null)
            {
                using (ZipArchive archive = ZipFile.Open(_zipPath, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = archive.GetEntry(_DBPathInsideZip);
                    _DBName = entry.Name;
                    _DBPathInDisk = $"C:\\{_DBName}";
                    var stream = entry.Open();
                    using (_fileStream = File.Create(_DBPathInDisk))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(_fileStream);
                    }

                    stream.Close();
                }
            }

            _memoryConnection = new SQLiteConnection("Data Source=:memory:");
            _connection = new SQLiteConnection($"Data Source={_DBPathInDisk};");
            _memoryConnection.Open();
            _connection.Open();
            _connection.BackupDatabase(_memoryConnection, "main", "main", -1, null, 0);
            _connection.Close();
            //_cmd = new SQLiteCommand(_connection);
            _cmd = new SQLiteCommand(_memoryConnection);
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
            if(_zipPath != null)
                using (ZipArchive archive = ZipFile.Open(_zipPath, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = archive.GetEntry(_DBPathInsideZip);
                    entry.Delete();
                    try
                    {
                        archive.CreateEntryFromFile(_DBPathInDisk, _DBPathInsideZip, CompressionLevel.NoCompression);
                        File.Delete(_DBPathInDisk);
                    }
                    catch (Exception e)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
        }
        public void PumpTable(TableRecordManipulationLogic tableRecorsdManipulationLogic)
        {
            int numofIntactsPerRecord = tableRecorsdManipulationLogic.Intacts;
            int numofDeletedesPerRecord = tableRecorsdManipulationLogic.Deletedes;
            string tableName = tableRecorsdManipulationLogic.TableName;
            string primaryKey = tableRecorsdManipulationLogic.PrimaryKey;
            var manipulationArgsLong = tableRecorsdManipulationLogic.ManipulationArgsLong;
            var manipulationArgsString = tableRecorsdManipulationLogic.ManipulationArgsString;
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
                    try
                    {
                        _cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            foreach (long key in deletedPrimaryKeys)
            {
                _cmd.CommandText = $@"delete from {tableName} where {primaryKey} = {key}";
                _cmd.ExecuteNonQuery();
            }
            _cmd.Dispose();
            //_connection.Close();            
            _connection.Open();
            // save memory db to file
            _memoryConnection.BackupDatabase(_connection, "main", "main", -1, null, 0);
            _memoryConnection.Close();
            _connection.Close();
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