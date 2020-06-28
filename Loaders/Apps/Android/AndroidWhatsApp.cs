using System;
using System.Collections.Generic;
using System.Text;

namespace Loaders.Apps.Android
{
    class AndroidWhatsApp : DbLoader
    {
        private static Random random = new Random();
        private const string hexValues = "0123456789ABCDEF";
        private const string engValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const string dbPath = @"\\ptnas1\RnD\New_RnD\Insight\Forensic Research Group\Personal\IshayK\IW3\msgstore.db";


        public AndroidWhatsApp()
        {
            base.Init(dbPath);
        }

        public override void PumpTable(RecordManipulationLogic table)
        {
            var messagesTableManipulatorLogic = new RecordManipulationLogic("messages");
            messagesTableManipulatorLogic.AddManipulationArg("key_id", KeyIdManipulatorFunc);
            messagesTableManipulatorLogic.AddManipulationArg("data", DataManipulatorFunc);
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
