namespace Loaders.Apps.Android
{
    public class AndroidTango : DbLoader
    {
        private TableRecordManipulationLogic _tableRecorsdManipulationLogic;

        public void Init()
        {
            //_zipPath = @"C:\IW3\dumps\Android\WhatsApp\WhatsApp_2.19.360\WhatsApp_2.19.360_Android_7.0.zip";
            //_DBPathInsideZip = @"Root/data/com.whatsapp/databases/msgstore.db";
            _DBPathInDisk = @"C:\IW3\dumps\Android\Tango_6.11.237500\tc.db";
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
                new TableRecordManipulationLogic("messages", "msg_id", intacts: 540, deletedes: 1);

            // string
            messagesTableManipulatorLogic.AddManipulationArg("@conv_id", GetRandomStringOrSame);

            // long
            messagesTableManipulatorLogic.AddManipulationArg("@type", GetRandomLong);
            messagesTableManipulatorLogic.AddManipulationArg("@create_time", GetRandomTimeStamp);

            return messagesTableManipulatorLogic;
        }
    }
}