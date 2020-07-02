using System;
using System.Collections.Generic;
using System.Text;

namespace Loaders.Apps.Android
{ 
    /* CREATE TABLE 'MESSAGE_DATA' (
    '_id' INTEGER PRIMARY KEY AUTOINCREMENT 
    'PROFILE_ID' TEXT NOT NULL 
    'CONTACT_ID' TEXT NOT NULL 
    'TYPE' INTEGER NOT NULL 
    'CONTENT' TEXT
    'TIMESTAMP' INTEGER NOT NULL 
    'SENDER' TEXT
    'REQ_ID' INTEGER UNIQUE 
    'MSG_ID' TEXT
    'HISTORY_ID' INTEGER NOT NULL 
    'SERVICE_TYPE' INTEGER NOT NULL 
    'FLAGS' INTEGER NOT NULL 
    'META' INTEGER
    'GROUP_ID' INTEGER
    'PREV_HISTORY_ID' INTEGER
    'SERVER_ACTION_MASK' INTEGER NOT NULL 
    'READS_COUNT' INTEGER NOT NULL 
    'PARTS' TEXT
    'PINNED' INTEGER NOT NULL 
    'UPDATE_PATCH_VERSION' TEXT
    'MENTION_ME' INTEGER NOT NULL 
    'CAPTCHA' INTEGER NOT NULL 
    IS_UNSUPPORTED INTEGER NOT NULL DEFAULT 0
    HIDE_EDIT INTEGER NOT NULL DEFAULT 0)
    */
    public class AndroidICQ: DbLoader
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

            _DBPathInDisk = @"C:\IW3\dumps\Android\ICQ\ICQ_9.4.2(824620)\db\agent-dao";

            base.Init();
            _tableRecorsdManipulationLogic = GetRecordManipulationLogic();
        }

        public void Parse()
        {
            base.Parse(_tableRecorsdManipulationLogic);
        }

        private TableRecordManipulationLogic GetRecordManipulationLogic()
        {
            var messagesTableManipulatorLogic = new TableRecordManipulationLogic("MESSAGE_DATA", "_id", intacts: 1500, deletedes: 1);

            // string
            messagesTableManipulatorLogic.AddManipulationArg("@CONTENT", BodyManipulatorFunc);

            // long
            messagesTableManipulatorLogic.AddManipulationArg("@TIMESTAMP", TimeStampManipulatorFunc);
            messagesTableManipulatorLogic.AddManipulationArg("@REQ_ID", ReqIdManipulatorFunc);
            messagesTableManipulatorLogic.AddManipulationArg("@HISTORY_ID", TimeStampManipulatorFunc);

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
        private long ReqIdManipulatorFunc(object value)
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