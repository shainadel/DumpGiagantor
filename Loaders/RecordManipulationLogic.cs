using System;
using System.Collections.Generic;
using System.Text;

namespace Loaders
{
    public class RecordManipulationLogic
    {
        public string TableName;
        public Dictionary<string, Func<string, string>> ManipulationArgs;

        public RecordManipulationLogic(string tableName)
        {
            TableName = tableName;
            ManipulationArgs = new Dictionary<string, Func<string, string>>();
        }

        public void AddManipulationArg(string columnNamne, Func<string,string> ManipulationFunc) //where T: class
        {
            ManipulationArgs[columnNamne] = ManipulationFunc;
        }

        //public void AddManipulationArg<T>(string columnNamne, Func<object, object> ManipulationFunc) //where T: class
        //{
        //    ManipulationArgs[columnNamne] = ManipulationFunc;
        //}
    }
}