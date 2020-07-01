using System;

namespace Loaders.Apps.Android
{
    
    /*
    CREATE TABLE messages (
    _id integer primary key autoincrement,
    message_id integer unique,
    contact_value text not null,
    contact_type integer,
    contact_name text not null,
    message_direction integer,
    message_type integer,
    message_text text not null,
    read boolean,
    date numeric not null,
    state integer default 0,
    attach text default '',
    message_source integer default 0,
    all_emoji boolean )
     */
    public class AndroidTextNow : DbLoader
    {
        private static Random random = new Random();
        private const string hexValues = "0123456789ABCDEF";
        private const string engValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private TableRecordManipulationLogic _tableRecorsdManipulationLogic;

        public void Init()
        {
            //_zipPath = @"C:\IW3\dumps\Android\Text Now\Text Now_20.21.0.1_Android_7.0.zip";
            //_DBPathInsideZip = @"Root/data/com.enflick.android.TextNow/databases/textnow_data.db";
            _DBPathInDisk =
                @"C:\IW3\dumps\Android\Text Now\Text Now_20.21.0.1_Android_7.0\Root\data\com.enflick.android.TextNow\databases\textnow_data.db";
            base.Init();
            _tableRecorsdManipulationLogic = GetRecordManipulationLogic();

        }
        public void Parse()
        {
            base.Parse(_tableRecorsdManipulationLogic);
        }

        private TableRecordManipulationLogic GetRecordManipulationLogic()
        {
            var messagesTableManipulatorLogic = new TableRecordManipulationLogic("messages", "_id", intacts: 500, deletedes: 1);
            
            messagesTableManipulatorLogic.AddManipulationArg("@message_text", DataManipulatorFunc);
            //messagesTableManipulatorLogic.AddManipulationArg("@name", DataManipulatorFunc);

            // long
            //messagesTableManipulatorLogic.AddManipulationArg("@_id", IdManipulateFunc);
            messagesTableManipulatorLogic.AddManipulationArg("@date", TimeStampManipulatorFunc);
            messagesTableManipulatorLogic.AddManipulationArg("@message_id", IdManipulatorFunc);

            return messagesTableManipulatorLogic;
        }
        
        private string DataManipulatorFunc(object value)
        {
            if (value == null)
                return null;

            string valueString = value.ToString();
            return "Random: " + RandomString(valueString.Length, engValues);
        }

        private long TimeStampManipulatorFunc(object value)
        {
            long valueInt = (long)value;
            return valueInt + random.Next(2000,15000);
        }

        private long IdManipulatorFunc(object value)
        {
            long valueInt = (long)value;
            return valueInt + random.Next(1000,100000);
        }

        private static string RandomString(int length, string chars)
        {
            var stringChars = new char[length];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
