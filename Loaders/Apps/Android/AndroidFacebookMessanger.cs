using System;
using System.Collections.Generic;
using System.Text;

namespace Loaders.Apps.Android
{
    public class AndroidFacebookMessanger : DbLoader
    {
        private static Random random = new Random();
        private const string hexValues = "0123456789ABCDEF";
        private const string engValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private TableRecordManipulationLogic _tableRecorsdManipulationLogic;

        public void Init()
        {
            //_zipPath = @"C:\IW3\dumps\Android\Facebook\Facebook Messenger_265.0.0.24.107\Facebook Messenger_265.0.0.24.107_Android_7.0.zip";
            //_DBPathInsideZip = @"Root/data/com.facebook.orca/databases/threads_db2";
            _DBPathInDisk =
                @"C:\IW3\dumps\Android\Facebook\Facebook Messenger_265.0.0.24.107\Facebook Messenger_265.0.0.24.107_Android_7.0\Root\data\com.facebook.orca\databases\threads_db2";
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
            
            messagesTableManipulatorLogic.AddManipulationArg("@text", GetRandomString);
            //messagesTableManipulatorLogic.AddManipulationArg("@name", DataManipulatorFunc);

            // long
            //messagesTableManipulatorLogic.AddManipulationArg("@_id", IdManipulateFunc);
            messagesTableManipulatorLogic.AddManipulationArg("@timestamp_ms", GetRandomTimeStamp);
            messagesTableManipulatorLogic.AddManipulationArg("@timestamp_sent_ms", GetRandomTimeStamp);
            messagesTableManipulatorLogic.AddManipulationArg("@msg_id", GetRandomFacebookString);

            return messagesTableManipulatorLogic;
        }

        private string GetRandomFacebookString(object value)
        {
            return "mid.$: " + RandomString(29, engValues);
        }
    }
}

