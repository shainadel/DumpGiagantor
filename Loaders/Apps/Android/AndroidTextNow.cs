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
            
            // string
            messagesTableManipulatorLogic.AddManipulationArg("@message_text", GetRandomString);

            // long
            messagesTableManipulatorLogic.AddManipulationArg("@date", GetRandomTimeStamp);
            messagesTableManipulatorLogic.AddManipulationArg("@message_id", GetRandomSuccessiveId);

            return messagesTableManipulatorLogic;
        }
    }
}