using System;

namespace Loaders.Apps.Android
{
    /*
    CREATE TABLE messages(_id INTEGER PRIMARY KEY AUTOINCREMENT
    key_remote_jid TEXT NOT NULL
    key_from_me INTEGER
    key_id TEXT NOT NULL
    status INTEGER
    needs_push INTEGER
    data TEXT
    timestamp INTEGER
    media_url TEXT
    media_mime_type TEXT
    media_wa_type TEXT
    media_size INTEGER
    media_name TEXT
    media_caption TEXT
    media_hash TEXT
    media_duration INTEGER
    origin INTEGER
    latitude REAL
    longitude REAL
    thumb_image TEXT
    remote_resource TEXT
    received_timestamp INTEGER
    send_timestamp INTEGER
    receipt_server_timestamp INTEGER
    receipt_device_timestamp INTEGER
    read_device_timestamp INTEGER
    played_device_timestamp INTEGER
    raw_data BLOB
    recipient_count INTEGER
    participant_hash TEXT
    starred INTEGER
    quoted_row_id INTEGER
    mentioned_jids TEXT
    multicast_id TEXT
    edit_version INTEGER
    media_enc_hash TEXT
    payment_transaction_id TEXT
    forwarded INTEGER
    preview_type INTEGER
    send_count INTEGER)
    */

    public class AndroidWhatsApp : DbLoader
    {
        private static Random random = new Random();
        private const string hexValues = "0123456789ABCDEF";
        private const string engValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        // private const string dbPath = @"\\ptnas1\RnD\New_RnD\Insight\Forensic Research Group\Personal\IshayK\IW3\msgstore.db";
       private const string dbPath = @"Data Source=C:\IW3\dumps\Android\WhatsApp\WhatsApp_2.19.360\db\msgstore.db;";


        private TableRecordManipulationLogic _tableRecorsdManipulationLogic;

        public void Init()
        {
            base.Init(dbPath);
            _tableRecorsdManipulationLogic = GetRecordManipulationLogic();
        }

        public void Parse()
        {
            base.Parse(_tableRecorsdManipulationLogic);
        }

        private TableRecordManipulationLogic GetRecordManipulationLogic()
        {
            var messagesTableManipulatorLogic = new TableRecordManipulationLogic("messages", "_id", intacts: 500, deletedes: 1);

            // string
            messagesTableManipulatorLogic.AddManipulationArg("@key_id", KeyIdManipulatorFunc);
            messagesTableManipulatorLogic.AddManipulationArg("@data", DataManipulatorFunc);
            //messagesTableManipulatorLogic.AddManipulationArg("@name", DataManipulatorFunc);

            // long
            //messagesTableManipulatorLogic.AddManipulationArg("@_id", IdManipulateFunc);
            messagesTableManipulatorLogic.AddManipulationArg("@timestamp", TimeStampManipulatorFunc);
            //messagesTableManipulatorLogic.AddManipulationArg("@id", IdManipulateFunc);
            //messagesTableManipulatorLogic.AddManipulationArg("@price", TimeStampManipulatorFunc);

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

        private string DataManipulatorFunc(object value)
        {
            if (value == null)
                return null;

            string valueString = value.ToString();
            return "Random: " + RandomString(valueString.Length, engValues);
        }

        private long IdManipulateFunc(object value)
        {
            long valueInt = (long) value;
            return valueInt + 620;
        }

        private long TimeStampManipulatorFunc(object value)
        {
            long valueInt = (long)value;
            return valueInt + random.Next(2000,15000);
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