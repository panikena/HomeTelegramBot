using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace HomeTelegramBot.Models.Weather
{
    public class CurrentWeather : WeatherData
    {
        public string Visibility { get; set; }

        public CurrentWeather Deserialize(XElement element)
        {
            var currentWeather = new CurrentWeather();

            currentWeather.Sunrise = DateTime.Parse(element.Element("city").Element("sun").Attribute("rise").Value, null, System.Globalization.DateTimeStyles.RoundtripKind);
            currentWeather.Sunset = DateTime.Parse(element.Element("city").Element("sun").Attribute("set").Value, null, System.Globalization.DateTimeStyles.RoundtripKind);

            if (element.Element("precipitation").Attribute("mode").Value != "no")
            {
                var rainPrecipitation = element.Elements().SingleOrDefault(x => x.Name == "precipitation" && x.Attribute("mode").Value == "rain")?.Attribute("value").Value;
                currentWeather.RainPrecipitation = double.Parse(rainPrecipitation != null ? rainPrecipitation : "0");
                var snowPrecipitation = element.Elements().SingleOrDefault(x => x.Name == "precipitation" && x.Attribute("mode").Value == "snow")?.Attribute("value").Value;
                currentWeather.SnowPrecipitation = double.Parse(snowPrecipitation != null ? snowPrecipitation : "0");
            }
            else
            {
                currentWeather.RainPrecipitation = currentWeather.SnowPrecipitation = 0;
            }
            
            currentWeather.WindAzimuth = double.Parse(element.Element("windDirection").Attribute("deg").Value);
            currentWeather.WindDirectionCode = element.Element("wind").Element("direction").Attribute("code").Value;
            currentWeather.WindSpeed = double.Parse(element.Element("wind").Element("speed").Attribute("value").Value);
            currentWeather.Temperature = double.Parse(element.Element("temperature").Attribute("value").Value);
            currentWeather.MaxTemperature = double.Parse(element.Element("temperature").Attribute("max").Value);
            currentWeather.MinTemperature = double.Parse(element.Element("temperature").Attribute("min").Value);
            currentWeather.Pressure = double.Parse(element.Element("pressure").Attribute("value").Value);
            currentWeather.Humidity = int.Parse(element.Element("humidity").Attribute("value").Value);
            currentWeather.Clouds = int.Parse(element.Element("clouds").Attribute("value").Value);
            currentWeather.WeatherCode = element.Element("weather").Attribute("number").Value;
            currentWeather.Visibility = element.Element("visibility").Attribute("value").Value;

            return currentWeather;
        }
    }
}