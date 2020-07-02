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
            messagesTableManipulatorLogic.AddManipulationArg("@created", GetRandomTimeStamp);
            messagesTableManipulatorLogic.AddManipulationArg("@entry_id", GetRandomSuccessiveId);
            messagesTableManipulatorLogic.AddManipulationArg("@sort_entry_id", GetRandomSuccessiveId);

            return messagesTableManipulatorLogic;
        }
    }
}