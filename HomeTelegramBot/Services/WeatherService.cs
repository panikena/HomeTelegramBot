using HomeTelegramBot.Helpers;
using HomeTelegramBot.Models.Weather;
using HomeTelegramBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace HomeTelegramBot.Services
{
    public class WeatherService : IWeatherService
    {
        private static string currentWeatherAddress = "http://api.openweathermap.org/data/2.5/weather?mode=xml&units=metric&appid=" + Configurator.BotSettings["WeatherAPIKey"];
        private static string forecastAddress = "http://api.openweathermap.org/data/2.5/forecast?mode=xml&units=metric&appid=" + Configurator.BotSettings["WeatherAPIKey"];

        public string GetWeatherMessage(string message)
        {
            string responseMessage = string.Empty;
            var words = message.Split();

            if (words.Length < 2)
            {
                return Properties.Resources.WeatherDesc;
            }
            var day = words[1];

            if (day == Properties.Resources.NowDefinition)
            {
                var currentWeather = GetCurrentWeather(null);
                if (currentWeather != null)
                {
                    responseMessage = currentWeather.GetWeatherDescription();
                }
            }
            else if (day == Properties.Resources.TomorrowDefinition)
            {
                var today = DateTime.Now;

                var weatherForecasts = GetWeatherForecasts(null);
                if (weatherForecasts != null && weatherForecasts.Any())
                {
                    var selectedTimeForecast = weatherForecasts.Single(x => x.From.Day == today.AddDays(1).Day && x.From.Hour == 12);
                    responseMessage = selectedTimeForecast.GetWeatherDescription();
                }   
            }

            return responseMessage;
        }

        public CurrentWeather GetCurrentWeather(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                city = Properties.Resources.DefaultWeatherCity;
            }

            var currentWeather = new CurrentWeather();
            
            var xmlDoc = GetXmlFromWeatherApi(currentWeatherAddress + "&q=" + city);

            currentWeather.Deserialize(XElement.Parse(xmlDoc.DocumentElement.OuterXml));

            return  currentWeather;
        }

        public IEnumerable<WeatherForecast> GetWeatherForecasts(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                city = Properties.Resources.DefaultWeatherCity;
            }

            var weatherForecasts = new List<WeatherForecast>();

            var xmlDoc = GetXmlFromWeatherApi(forecastAddress + "&q=" + city);

            var xRootElement = XElement.Parse(xmlDoc.DocumentElement.OuterXml);
            var timeForecastNodes = xRootElement.Element("forecast").Elements("time");

            foreach (var node in timeForecastNodes)
            {
                weatherForecasts.Add(new WeatherForecast().Deserialize(node));
            }

            return weatherForecasts;
        }

        public XmlDocument GetXmlFromWeatherApi(string requestAddress)
        {
            var request = (HttpWebRequest)WebRequest.Create(requestAddress);
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(response.GetResponseStream());
            response.Close();

            return xmlDoc;
        }
    }
}