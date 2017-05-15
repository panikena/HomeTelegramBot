using HomeTelegramBot.Helpers;
using HomeTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeTelegramBot.App_Start
{
    public class BotStarter
    {
        public static void StartBot()
        {
            Bot.Get().StartReceiving();
        }
    }
}