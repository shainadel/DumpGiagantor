﻿namespace Loaders.Apps.Android
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
        private TableRecordManipulationLogic _tableRecorsdManipulationLogic;

        public void Init()
        {
            //_zipPath = @"C:\IW3\dumps\Android\WhatsApp\WhatsApp_2.19.360\WhatsApp_2.19.360_Android_7.0.zip";
            //_DBPathInsideZip = @"Root/data/com.whatsapp/databases/msgstore.db";
            //_DBPathInDisk = PUT YOUR LOCAL PATH HERE IF YOU'RE NOT WORKING WITH ZIP AND COMMENT THE 2 LINES ABOVE
            _DBPathInDisk = @"C:\IW3\dumps\Android\WhatsApp\WhatsApp_2.19.360\db\msgstore.db";
            base.Init();
            _tableRecorsdManipulationLogic = GetRecordManipulationLogic();
        }

        public void Parse()
        {
            base.Parse(_tableRecorsdManipulationLogic);
        }

        private TableRecordManipulationLogic GetRecordManipulationLogic()
        {
            var messagesTableManipulatorLogic =
                new TableRecordManipulationLogic("messages", "_id", intacts: 500, deletedes: 1);

            // string
            messagesTableManipulatorLogic.AddManipulationArg("@key_id", GetRandomHexString);
            messagesTableManipulatorLogic.AddManipulationArg("@data", GetRandomString);

            // long
            messagesTableManipulatorLogic.AddManipulationArg("@timestamp", GetRandomTimeStamp);

            return messagesTableManipulatorLogic;
        }
    }
}