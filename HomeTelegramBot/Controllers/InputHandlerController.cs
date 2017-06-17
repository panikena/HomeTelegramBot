using HomeTelegramBot.Helpers;
using HomeTelegramBot.Models;
using System.Net.Http;
using System.Linq;
using System.Web.Http;
using Telegram.Bot.Types;

namespace HomeTelegramBot.Controllers
{
    public class  InputHandlerController : ApiController
    {
        private static  Bot _bot = Bot.Get();
        private static HttpClient _httpClient = new HttpClient();

        public InputHandlerController()
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new System.Uri(Configurator.GetAppSetting("HostAddress"));
            }   
        }

        [HttpGet]
        public HttpResponseMessage Help()
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.OK);
        }
        
        public void Help([FromBody] Message message)
        {
            _bot.SendTextMessageAsync(message.Chat.Id, Properties.Resources.AvailableCommands);
        }
        
        public void Weather([FromBody] Message message)
        { 
            HttpResponseMessage result;
            string responseMessage = "";

            result = _httpClient.GetAsync("api/Weather/GetWeather?commandMessage=" + message.Text).Result;

            responseMessage = result.Content.ReadAsStringAsync().Result;
          
            _bot.SendTextMessageAsync(message.Chat.Id, responseMessage);
        }

        public void Info([FromBody] Message message)
        {
            _bot.SendTextMessageAsync(message.Chat.Id, Properties.Resources.InfoText + Configurator.BotVersion);
        }

        public void Switch([FromBody] Message message)
        {
            var words = message.Text.Split();

            if(words.Length < 3)
            {
                _bot.SendTextMessageAsync(message.Chat.Id, Properties.Resources.SwitchDesc);
                return;
            }

            //get all words except last
            var objectWord = string.Join(" ", words.Skip(1).Take(words.Length - 1));
            //get last word
            var action = words[words.Length - 1];

            _bot.SendTextMessageAsync(message.Chat.Id, objectWord + " was turned " + action);
        }

        public void Start([FromBody] Message message)
        {
            _bot.SendTextMessageAsync(message.Chat.Id, string.Format(Properties.Resources.InfoText, message.Chat.FirstName) + Properties.Resources.MyVersionIs + " " + Configurator.BotVersion + "\n" + Properties.Resources.AvailableCommands);
        }

        public void HowAreYou([FromBody] Message message)
        {
            _bot.SendTextMessageAsync(message.Chat.Id, Properties.Resources.IAmFine);
        }
        public void PleaseRepeat([FromBody] Message message)
        {
            _bot.SendTextMessageAsync(message.Chat.Id, Properties.Resources.PleaseRepeat);
        }
        public void Greeting([FromBody] Message message)
        {
            _bot.SendTextMessageAsync(message.Chat.Id, string.Format(Properties.Resources.InfoText, message.Chat.FirstName));
        }


    }
}

