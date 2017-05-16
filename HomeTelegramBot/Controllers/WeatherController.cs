using HomeTelegramBot.Helpers;
using HomeTelegramBot.Models.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace HomeTelegramBot.Controllers
{
    public class WeatherController : ApiController
    {
        private string currentWeatherAddress = "http://api.openweathermap.org/data/2.5/weather?q=Kharkiv,ua&mode=xml&units=metric&appid=" + Configurator.BotSettings["WeatherAPIKey"];
        private string forecastAddress = "http://api.openweathermap.org/data/2.5/forecast?q=Kharkiv,ua&mode=xml&units=metric&appid=" + Configurator.BotSettings["WeatherAPIKey"];

        public HttpResponseMessage GetCurrentWeather(int chatId)
        {
            var currentWeather = new CurrentWeather();

            var request = WebRequest.Create(currentWeatherAddress);
            var response = request.GetResponse();
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(response.GetResponseStream());

            return Request.CreateResponse(HttpStatusCode.OK, currentWeather);
        }

        public HttpResponseMessage GetWeatherForecast(int chatId)
        {
            var weatherForecast = new List<WeatherForecast>();

            var request = WebRequest.Create(forecastAddress);
            var response = request.GetResponse();
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(response.GetResponseStream());

            

            return Request.CreateResponse(HttpStatusCode.OK, weatherForecast);
        }

    }
}
