using System;
using System.Collections.Generic;

namespace Loaders
{
    public class TableRecordManipulationLogic
    {
        public string TableName;
        public int Intacts;
        public int Deletedes;
        public Dictionary<string, Func<string, string>> ManipulationArgsString;
        public Dictionary<string, Func<int, int>> ManipulationArgsInt;

        public TableRecordManipulationLogic(string tableName, int intacts, int deletedes)
        {
            TableName = tableName;
            Intacts = intacts;
            Deletedes = deletedes;
            ManipulationArgsString = new Dictionary<string, Func<string, string>>();
            ManipulationArgsInt = new Dictionary<string, Func<int, int>>();
        }

        public void AddManipulationArg(string columnNamne, Func<string,string> ManipulationFunc)
        {
            ManipulationArgsString[columnNamne] = ManipulationFunc;
        }

        public void AddManipulationArg(string columnNamne, Func<int, int> ManipulationFunc)
        {
            ManipulationArgsInt[columnNamne] = ManipulationFunc;
        }
    }
}