using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml.Linq;

namespace HomeTelegramBot.Helpers
{
    public static class Configurator
    {
        public static Dictionary<string, string> BotSettings = GetBotConfigKeyValuePairs("settings");
        public static Dictionary<string, string> BotCommands = GetBotConfigKeyValuePairs("commands");
        public static string BotVersion = GetBotConfigNode("version").Attribute("value").Value;

        public static string GetTelegramAPIKey()
        {
            return BotSettings["TelegramAPIKey"];
        }

        public static string GetAppSetting(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }

        #region Bot.config

        private static XElement GetBotConfig()
        {
            var document = XDocument.Load(HostingEnvironment.MapPath(@"/Bot.config"));

            return document.Root;
        }

        private static XElement GetBotConfigNode(string nodeName)
        {
            return GetBotConfig().Element(nodeName);
        }

        private static Dictionary<string, string> GetBotConfigKeyValuePairs(string nodeName)
        {
            var commands = GetBotConfigNode(nodeName).Elements().ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("value").Value);

            return commands;
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