using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml.Linq;

namespace HomeTelegramBot.Helpers
{
    public static class Configurator
    {
        public static Dictionary<string, string> BotSettings = GetBotSettings();
        public static Dictionary<string, string> BotCommands = GetBotCommands();


        public static string GetTelegramAPIKey()
        {
            return BotSettings["APIKey"];
        }

        public static string GetAppSetting(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }

        #region Bot.config

        private static XElement GetBotConfig()
        {
            var document = XDocument.Load(HostingEnvironment.MapPath(@"/Bot.config"));

            return document.Root.Element("configuration");
        }

        private static Dictionary<string, string> GetBotCommands()
        {
            var config = GetBotConfig();

            var commands = config.Element("commands").Elements().ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("values").Value);

            return commands;
        }

        private static Dictionary<string, string> GetBotSettings()
        {
            var config = GetBotConfig();

            var settings = config.Element("settings").Elements().ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("values").Value);

            return settings;
        }

        #endregion Bot.config

        public static IEnumerable<string> GetKeyByValue(string value, Dictionary<string, string> data)
        {
            value = value.Trim(new char[] { '/' });
            List<string> keysToReturn = new List<string>();

            foreach (var kvp in data)
            {
                var values = kvp.Value.Split();

                if (values.Any(x => x.Contains(value)))
                {
                    keysToReturn.Add(kvp.Key);
                }   
            }

            return keysToReturn;
        }
    }
}