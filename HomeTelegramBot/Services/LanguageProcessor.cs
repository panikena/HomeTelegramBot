using HomeTelegramBot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Telegram.Bot.Types;

namespace HomeTelegramBot.Services
{
    public class LanguageProcessor
    {
        public static string GetActionFromMessage(Message message)
        {
            var words = message.Text.Split();

            var registeredKeywords = Configurator.Keywords;
            var keywordControllers = new List<KVPCounter>();

            foreach (var kvp in registeredKeywords)
            {
                keywordControllers.Add(new KVPCounter(kvp.Key, kvp.Value, 0));
            }


            Regex pattern = new Regex("[;,.!?]");

            //analyse by keywords
            foreach (var word in words)
            {
                foreach (var controller in keywordControllers)
                {
                    if (controller.Value.Contains(pattern.Replace(word, string.Empty), StringComparison.OrdinalIgnoreCase))
                    {
                        controller.Counter++;
                    }
                }
            }
            //find max keyword counter
            var maxUsedKeyword = new KVPCounter();
            int maxCounter = 0;
            foreach (var item in keywordControllers)
            {
                if (item.Counter > maxCounter)
                {
                    maxCounter = item.Counter;
                    maxUsedKeyword = item;
                }
            }

            //analyze by keyphrases
            var registeredKeyphrases = Configurator.Keyphrases;
            var keyPhraseControllers = new List<KVPCounter>();

            foreach (var kvp in registeredKeyphrases)
            {
                keyPhraseControllers.Add(new KVPCounter(kvp.Key, kvp.Value, 0));
            }

            foreach (var kvp in keyPhraseControllers)
            {
                var phrases = kvp.Value.Split();

                if (phrases.Any(x => message.Text.Contains(x, StringComparison.OrdinalIgnoreCase)))
                {
                    kvp.Counter++;
                }
            }

            var maxUsedKeyphrase = new KVPCounter();
            maxCounter = 0;
            foreach (var item in keyPhraseControllers)
            {
                if (item.Counter > maxCounter)
                {
                    maxCounter = item.Counter;
                    maxUsedKeyphrase = item;
                }
            }


            if (maxUsedKeyword.Counter != 0 || maxUsedKeyphrase.Counter != 0)
            {
                if (maxUsedKeyphrase.Counter > maxUsedKeyword.Counter)
                {
                    return maxUsedKeyphrase.Key;
                }
                else
                {
                    return maxUsedKeyword.Key;
                }
            }

            return null;
        }
    }

    public class KVPCounter
    {
        public KVPCounter()
        {

        }

        public KVPCounter(string key, string value, int counter)
        {
            Key = key;
            Value = value;
            Counter = counter;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public int Counter { get; set; }
    }
}