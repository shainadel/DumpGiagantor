using System;

namespace Loaders.Apps.Android
{
    public class AndroidWhatsApp : DbLoader
    {
        private static Random random = new Random();
        private const string hexValues = "0123456789ABCDEF";
        private const string engValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const string dbPath = @"\\ptnas1\RnD\New_RnD\Insight\Forensic Research Group\Personal\IshayK\IW3\msgstore.db";

        private TableRecordManipulationLogic tableRecorsdManipulationLogic;

        public void Init()
        {
            base.Init(dbPath);
            tableRecorsdManipulationLogic = GetRecordManipulationLogic();
        }

        public void Parse()
        {
            base.Parse(tableRecorsdManipulationLogic);
        }

        private TableRecordManipulationLogic GetRecordManipulationLogic()
        {
            var messagesTableManipulatorLogic = new TableRecordManipulationLogic("messages");

            // string
            messagesTableManipulatorLogic.AddManipulationArg("key_id", KeyIdManipulatorFunc);
            messagesTableManipulatorLogic.AddManipulationArg("data", DataManipulatorFunc);

            // int
            messagesTableManipulatorLogic.AddManipulationArg("_id", IdManipulateFunc);
            messagesTableManipulatorLogic.AddManipulationArg("timestamp", TimeStampManipulatorFunc);
            return messagesTableManipulatorLogic;
        }

        private string KeyIdManipulatorFunc(string value)
        {
            if (value == null)
                return value;

            return RandomString(value.Length, hexValues);
        }

        private string DataManipulatorFunc(string value)
        {
            if (value == null)
                return value;

            return RandomString(value.Length, engValues);
        }

        private int IdManipulateFunc(int value)
        {
            return value + 1000000;
        }

        private int TimeStampManipulatorFunc(int value)
        {
            return value + random.Next(100);
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