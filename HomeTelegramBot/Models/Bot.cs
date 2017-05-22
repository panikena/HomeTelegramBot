using HomeTelegramBot.DataAccess.Interfaces;
using HomeTelegramBot.DataAccess.Repositories;
using HomeTelegramBot.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HomeTelegramBot.Models
{
    public class Bot : TelegramBotClient
    {
        private static Bot _bot;

        private IUserRepository _userRepository;
        private IList<User> _authorizationRequests;

        private string TelegramAPIKey = Configurator.GetTelegramAPIKey();

        private HttpClient _httpClient;

        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            private set
            {
                _isActive = value;
            }
        }

        private Bot(string token) : base(token)
        {
            InitializeEventHandlers();
         
            _userRepository = new UserRepository();
            _authorizationRequests = new List<User>();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Configurator.GetAppSetting("HostAddress"));
        }

        public static Bot Get()
        {
            if (_bot == null)
            {
                _bot = new Bot(Configurator.GetTelegramAPIKey());
            }

            return _bot;
        }

        private void InitializeEventHandlers()
        {
            OnCallbackQuery += BotOnCallbackQueryReceived;
            OnMessage += BotOnMessageReceived;
            OnMessageEdited += BotOnMessageReceived;
            OnInlineQuery += BotOnInlineQueryReceived;
            OnInlineResultChosen += BotOnChosenInlineResultReceived;
            OnReceiveError += BotOnReceiveError;
        }

        #region Bot events

        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            //   _log.Info(receiveErrorEventArgs.ApiRequestException.Message);

            Debugger.Log(0, "ERROR", receiveErrorEventArgs.ApiRequestException.Message);
        }

        private void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            //InlineQueryResult[] results = {
            //    new InlineQueryResultLocation
            //    {
            //        Id = "1",
            //        Latitude = 40.7058316f, // displayed result
            //        Longitude = -74.2581888f,
            //        Title = "New York",
            //        InputMessageContent = new InputLocationMessageContent // message if result is selected
            //        {
            //            Latitude = 40.7058316f,
            //            Longitude = -74.2581888f,
            //        }
            //    },

            //    new InlineQueryResultLocation
            //    {
            //        Id = "2",
            //        Longitude = 52.507629f, // displayed result
            //        Latitude = 13.1449577f,
            //        Title = "Berlin",
            //        InputMessageContent = new InputLocationMessageContent // message if result is selected
            //        {
            //            Longitude = 52.507629f,
            //            Latitude = 13.1449577f
            //        }
            //    }
            //};

            //await _bot.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
        }

        private string ResolveCommandHandler(string command)
        {
            return Configurator.GetKeyByValue(command.ToLower(), Configurator.BotCommands).FirstOrDefault();
        }

        private string RetrieveCommand(string text)
        {
            return text.Split()[0];
        }

        private void HandleCommand(Message message)
        {
            var command = message.Text.Split()[0];

            var handlerName = ResolveCommandHandler(command);

            SendMessageToHandler(message, handlerName);
        }

        private void HandleTextRequest(Message message)
        {
            var someAction = "action";

            SendMessageToHandler(message, someAction);
        }

        private async void SendMessageToHandler(Message message, string handlerName)
        {
            if (handlerName == null)
            {
                //return error message if bot cannot handle input
                await _bot.SendTextMessageAsync(message.Chat.Id, Properties.Resources.ErrorMessage);

                return;
            }

            var messageWrapper = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

            string address = "api/InputHandler/" + handlerName + "/";

            var result  = await _httpClient.PostAsync(address, messageWrapper);

            if (!result.IsSuccessStatusCode)
            {
                await _bot.SendTextMessageAsync(message.Chat.Id, Properties.Resources.ErrorMessage);
            }
        }

        private void HandleMessage(MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message.Text[0] == '/')
            {
                HandleCommand(message);
            }
            else
            {
                HandleTextRequest(message);
            }
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var user = messageEventArgs.Message.From;

            var message = messageEventArgs.Message;

            if (!_userRepository.IsUserAuthorized(user))
            //if(!true)
            {
                string hint = "Please authorize";

                await _bot.SendTextMessageAsync(message.Chat.Id, hint);

                //_authorizationRequests.Add(user);

                return;
            }

            if (message == null || message.Type != MessageType.TextMessage) return;

            HandleMessage(messageEventArgs);
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await _bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }

        #endregion Bot events

    }
}