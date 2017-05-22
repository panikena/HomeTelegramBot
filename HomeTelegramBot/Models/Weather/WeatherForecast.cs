using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace HomeTelegramBot.Models.Weather
{
    public class WeatherForecast : WeatherData
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public IEnumerable<WeatherForecast> Deserialize(XmlDocument xmlDoc)
        {
            var weatherForecasts = new List<WeatherForecast>();
            var xdoc = XDocument.Load(new XmlNodeReader(xmlDoc));
            xdoc.Element("forecast").Elements().ToList().ForEach(x => weatherForecasts.Add(Deserialize(x)));

            return weatherForecasts;
        }

        public WeatherForecast Deserialize(XElement element)
        {
            var weatherForecast = new WeatherForecast();

            weatherForecast.From = DateTime.Parse(element.Attribute("from").Value, null, System.Globalization.DateTimeStyles.RoundtripKind);
            weatherForecast.To = DateTime.Parse(element.Attribute("to").Value, null, System.Globalization.DateTimeStyles.RoundtripKind);

            //precipitation node can be empty
            var rainPrecipitation = element.Elements().SingleOrDefault(x => x.Name == "precipitation" && x.Attribute("type")?.Value == "rain")?.Attribute("value").Value;
            weatherForecast.RainPrecipitation = double.Parse(rainPrecipitation != null ? rainPrecipitation : "0");

            var snowPrecipitation = element.Elements().SingleOrDefault(x => x.Name == "precipitation" && x.Attribute("type")?.Value == "snow")?.Attribute("value").Value;
            weatherForecast.SnowPrecipitation = double.Parse(snowPrecipitation != null ? snowPrecipitation : "0");

            weatherForecast.WindAzimuth = double.Parse(element.Element("windDirection").Attribute("deg").Value);
            weatherForecast.WindDirectionCode = element.Element("windDirection").Attribute("code").Value;
            weatherForecast.WindSpeed = double.Parse(element.Element("windSpeed").Attribute("mps").Value);
            weatherForecast.Temperature = double.Parse(element.Element("temperature").Attribute("value").Value);
            weatherForecast.MaxTemperature = double.Parse(element.Element("temperature").Attribute("max").Value);
            weatherForecast.MinTemperature = double.Parse(element.Element("temperature").Attribute("min").Value);
            weatherForecast.Pressure = double.Parse(element.Element("pressure").Attribute("value").Value);
            weatherForecast.Humidity = int.Parse(element.Element("humidity").Attribute("value").Value);
            weatherForecast.Clouds = int.Parse(element.Element("clouds").Attribute("all").Value);

            return weatherForecast;
        }
    }
}