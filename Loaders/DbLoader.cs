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
            //cmd.ExecuteNonQuery();
            cmd.CommandText = @"SELECT * FROM messages_fts_segdir";

            SQLiteDataReader rdr = cmd.ExecuteReader();
            //while (rdr.Read())
            //{
            dataTable.Load(rdr);
            //Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)} {rdr.GetInt32(2)}");
            //break;
            //}

            StringBuilder stringRow = new StringBuilder();
            stringRow.Append($"INSERT INTO {dataTable.TableName} VALUES(");

            bool firstValue = true;
            var columnList = dataTable.Columns;

            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < columnList.Count; i++)
                {
                    var value = row.ItemArray[i];//GetManipulateValue(row.ItemArray[i], columnList[i].ColumnName);

                    if (value.GetType().Name == "String")
                    {
                        value = $"'{value}'";
                    }
                    else if (value.GetType().Name == "Byte[]")
                    {

                    }
                    if (!firstValue)
                    {
                        stringRow.Append(",");
                    }
                    else
                    {
                        firstValue = false;
                    }

                    stringRow.Append($"{value}");
                }

                stringRow.Append(")");
                string z = stringRow.ToString();
            }
            //cmd.ExecuteNonQuery();

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
        public void UpdateRecord(List<string> lastRec,TableRecordManipulationLogic logic)
        {

        }
    }
}