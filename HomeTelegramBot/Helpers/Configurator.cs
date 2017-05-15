using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace HomeTelegramBot.Helpers
{
    public static class Configurator
    {
        public static string GetTelegramAPIKey()
        {
            return GetAppSetting("TelegramAPIKey");
        }

        public static string GetAppSetting(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }

        //public static Configuration GetBotConfiguration()
        //{
        //    var configFile = new FileInfo("Bot.config");
        //    var vdm = new VirtualDirectoryMapping(configFile.DirectoryName, true, configFile.Name);
        //    var wcfm = new WebConfigurationFileMap();
        //    wcfm.VirtualDirectories.Add("/", vdm);
        //    return WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/");
        //}

        public static IEnumerable<string> GetKeyByValue(string value)
        {
            value = value.Trim(new char[] { '/' });
            List<string> keysToReturn = new List<string>();

            foreach (var key in WebConfigurationManager.AppSettings)
            {
                var keyString = key.ToString();

                var values = WebConfigurationManager.AppSettings.GetValues(keyString);

                if (values.Any(x => x.Contains(value)))
                {
                    keysToReturn.Add(keyString);
                }   
            }

            return keysToReturn;
        }


    }
}