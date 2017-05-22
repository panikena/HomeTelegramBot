using HomeTelegramBot.Services.Interfaces;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HomeTelegramBot.Controllers
{
    public class WeatherController : ApiController
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        
        public HttpResponseMessage GetWeather(string commandMessage)
        {
            //http://stackoverflow.com/questions/11581697/is-there-a-way-to-force-asp-net-web-api-to-return-plain-text
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(_weatherService.GetWeatherMessage(commandMessage), System.Text.Encoding.UTF8, "text/plain");
            return response;
        }

    }
}
