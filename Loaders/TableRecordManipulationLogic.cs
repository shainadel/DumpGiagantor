using System;
using System.Collections.Generic;

namespace Loaders
{
    public class TableRecordManipulationLogic
    {
        public string TableName;
        public int Intacts;
        public int Deletedes;
        public string PrimaryKey;
        public Dictionary<string, Func<object, string>> ManipulationArgsString;
        public Dictionary<string, Func<object, long>> ManipulationArgsLong;

        public TableRecordManipulationLogic(string tableName,string primaryKey, int intacts, int deletedes)
        {
            TableName = tableName;
            PrimaryKey = primaryKey;
            Intacts = intacts;
            Deletedes = deletedes;
            ManipulationArgsString = new Dictionary<string, Func<object, string>>();
            ManipulationArgsLong = new Dictionary<string, Func<object, long>>();

        }

        public void AddManipulationArg(string columnNamne, Func<object,string> ManipulationFunc)
        {
            ManipulationArgsString[columnNamne] = ManipulationFunc;
        }

        public void AddManipulationArg(string columnNamne, Func<object, long> ManipulationFunc)
        {
            ManipulationArgsLong[columnNamne] = ManipulationFunc;
        }
    }
}