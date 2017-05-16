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
        private Bot _bot = Bot.Get();

        [HttpGet]
        public HttpResponseMessage Help()
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.OK);
        }
        
        public void Help([FromBody] Message message)
        {
            _bot.SendTextMessageAsync(message.Chat.Id, Properties.Resources.AvailableCommands);
        }
        
        public void WeatherForecast([FromBody] Message message)
        {
            _bot.SendTextMessageAsync(message.Chat.Id, "The weather will be great tomorrow!");
        }

        public void CurrentWeather([FromBody] Message message)
        {
            _bot.SendTextMessageAsync(message.Chat.Id, "The weather will be great tomorrow!");
        }

        public void Info([FromBody] Message message)
        {
            _bot.SendTextMessageAsync(message.Chat.Id, Properties.Resources.InfoText + Configurator.GetAppSetting("Version"));
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
            _bot.SendTextMessageAsync(message.Chat.Id, Properties.Resources.InfoText + Configurator.GetAppSetting("Version") + "\n" + Properties.Resources.AvailableCommands);
        }

    }
}
