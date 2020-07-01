using System;

namespace Loaders.Apps.Android
{
    public class AndroidTango : DbLoader
    {
        private static Random random = new Random();
        private const string hexValues = "0123456789ABCDEF";
        private const string engValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
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
            var messagesTableManipulatorLogic = new TableRecordManipulationLogic("messages", "msg_id", intacts: 540, deletedes: 1);

            // string
            messagesTableManipulatorLogic.AddManipulationArg("@conv_id", RandomStringOrSame);
            //messagesTableManipulatorLogic.AddManipulationArg("@name", DataManipulatorFunc);

            // long
            messagesTableManipulatorLogic.AddManipulationArg("@type", RandomLong);
            messagesTableManipulatorLogic.AddManipulationArg("@create_time", TimeStampManipulatorFunc);
            //messagesTableManipulatorLogic.AddManipulationArg("@price", TimeStampManipulatorFunc);

            return messagesTableManipulatorLogic;
        }

        private string RandomStringOrSame(object value)
        {
            var i = random.Next(0, 999);
            if (i != 5)
                return value.ToString();
            return MediaIdManipulatorFunc(value);
        }

        private string MediaIdManipulatorFunc(object value)
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
            long valueInt = (long)value;
            return valueInt + 620;
        }

        private long TimeStampManipulatorFunc(object value)
        {
            long valueInt = (long)value;
            return valueInt + random.Next(-15000, 15000);
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

        private long RandomLong(object value)
        {
            return random.Next(100);
        }
    }
}