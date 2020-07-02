using System;

namespace Loaders.Apps.Android
{
    public class AndroidWeChat : DbLoader
    {
        private TableRecordManipulationLogic _tableRecorsdManipulationLogic;
        public void Init()
        {
            //_zipPath = @"C:\IW3\dumps\Android\WhatsApp\WhatsApp_2.19.360\WhatsApp_2.19.360_Android_7.0.zip";
            //_DBPathInsideZip = @"Root/data/com.whatsapp/databases/msgstore.db";
            _DBPathInDisk = @"C:\IW3\dumps\Android\WeChat_7.0.7\original\EnMicroMsg.db.decrypted";
            base.Init();
            _tableRecorsdManipulationLogic = GetRecordManipulationLogic();
        }

        public void Parse()
        {
            base.Parse(_tableRecorsdManipulationLogic);
        }

        private TableRecordManipulationLogic GetRecordManipulationLogic()
        {
            var messagesTableManipulatorLogic = new TableRecordManipulationLogic("message", "msgId", intacts: 858, deletedes: 2);

            // string
            messagesTableManipulatorLogic.AddManipulationArg("@content", GetRandomStringOrSame);
            messagesTableManipulatorLogic.AddManipulationArg("@talker", GetRandomStringOrSame);
            messagesTableManipulatorLogic.AddManipulationArg("@imgPath", GetRandomStringOrSame);

            // long
            messagesTableManipulatorLogic.AddManipulationArg("@type", GetRandomLong);
            messagesTableManipulatorLogic.AddManipulationArg("@createTime", GetRandomTimeStamp);
            //messagesTableManipulatorLogic.AddManipulationArg("@price", TimeStampManipulatorFunc);

            return messagesTableManipulatorLogic;
        }
    }
}