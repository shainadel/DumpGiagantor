using System;
using System.Collections.Generic;
using System.Text;

namespace Loaders.Apps.Android
{
    /* CREATE TABLE conversation_entries (
    _id INTEGER PRIMARY KEY
    entry_id INTEGER UNIQUE NOT NULL
    sort_entry_id INTEGER UNIQUE NOT NULL
    conversation_id TEXT /*NULLABLE* /
    user_id INTEGER
    created INTEGER
    entry_type INTEGER
    data BLOB /*NULLABLE* /
    request_id TEXT /*NULLABLE* /
    linked_entry_id INTEGER )
    */
    public class AndroidTwitter : DbLoader
    {

        private static Random random = new Random();
        private const string hexValues = "0123456789ABCDEF";
        private const string engValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private TableRecordManipulationLogic _tableRecorsdManipulationLogic;

        public void Init()
        {
            //_zipPath = @"C:\IW3\dumps\Android\WhatsApp\WhatsApp_2.19.360\WhatsApp_2.19.360_Android_7.0.zip";
            //_DBPathInsideZip = @"Root/data/com.whatsapp/databases/msgstore.db";
            //_DBPathInDisk = PUT YOUR LOCAL PATH HERE IF YOU'RE NOT WORKING WITH ZIP AND COMMENT THE 2 LINES ABOVE

            _DBPathInDisk = @"C:\IW3\dumps\Android\Twitter\Twitter_8.48.1-release.01\db\1216268147101184005-60.db";

            base.Init();
            _tableRecorsdManipulationLogic = GetRecordManipulationLogic();
        }

        public void Parse()
        {
            base.Parse(_tableRecorsdManipulationLogic);
        }

        private TableRecordManipulationLogic GetRecordManipulationLogic()
        {
            var messagesTableManipulatorLogic = new TableRecordManipulationLogic("conversation_entries", "_id", intacts: 1850, deletedes: 1);

            // long
            messagesTableManipulatorLogic.AddManipulationArg("@created", TimeStampManipulatorFunc);
            messagesTableManipulatorLogic.AddManipulationArg("@entry_id", UniquManipulatorFunc);
            messagesTableManipulatorLogic.AddManipulationArg("@sort_entry_id", UniquManipulatorFunc);

            return messagesTableManipulatorLogic;
        }

        private string KeyIdManipulatorFunc(object value)
        {
            if (value == null)
                return null;

            string valueString = value.ToString();
            int length = valueString.Length > 16 ? valueString.Length : 16;
            return RandomString(length, hexValues);
        }

        private string BodyManipulatorFunc(object value)
        {
            if (value == null)
                return null;

            string valueString = value.ToString();
            return "Random: " + RandomString(valueString.Length, engValues);
        }

        //private long IdManipulateFunc(object value)
        //{
        //    long valueInt = (long) value;
        //    return valueInt + 620;
        //}

        private long TimeStampManipulatorFunc(object value)
        {
            long valueInt = (long)value;
            return valueInt + random.Next(-15000, 15000);
        }
        private long UniquManipulatorFunc(object value)
        {
            long valueInt = (long)value;
            return valueInt + random.Next(200000, 1500000);
        }

        private static string RandomString(int length, string chars)
        {
            var stringChars = new char[length];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }
    }
}