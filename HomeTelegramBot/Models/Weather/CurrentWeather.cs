using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace HomeTelegramBot.Models.Weather
{
    public class CurrentWeather : WeatherData
    {
        public string Visibility { get; set; }

        public void Deserialize(XElement element)
        {
            Sunrise = DateTime.Parse(element.Element("city").Element("sun").Attribute("rise").Value, null, System.Globalization.DateTimeStyles.RoundtripKind);
            Sunset = DateTime.Parse(element.Element("city").Element("sun").Attribute("set").Value, null, System.Globalization.DateTimeStyles.RoundtripKind);

            if (element.Element("precipitation").Attribute("mode").Value != "no")
            {
                var rainPrecipitation = element.Elements().SingleOrDefault(x => x.Name == "precipitation" && x.Attribute("mode").Value == "rain")?.Attribute("value").Value;
                RainPrecipitation = double.Parse(rainPrecipitation != null ? rainPrecipitation : "0");
                var snowPrecipitation = element.Elements().SingleOrDefault(x => x.Name == "precipitation" && x.Attribute("mode").Value == "snow")?.Attribute("value").Value;
                SnowPrecipitation = double.Parse(snowPrecipitation != null ? snowPrecipitation : "0");
            }
            else
            {
                RainPrecipitation = SnowPrecipitation = 0;
            }
            
            WindAzimuth = double.Parse(element.Element("wind").Element("speed").Attribute("value").Value);
            WindDirectionCode = element.Element("wind").Element("direction").Attribute("code").Value;
            WindSpeed = double.Parse(element.Element("wind").Element("speed").Attribute("value").Value);
            Temperature = double.Parse(element.Element("temperature").Attribute("value").Value);
            MaxTemperature = double.Parse(element.Element("temperature").Attribute("max").Value);
            MinTemperature = double.Parse(element.Element("temperature").Attribute("min").Value);
            Pressure = double.Parse(element.Element("pressure").Attribute("value").Value);
            Humidity = int.Parse(element.Element("humidity").Attribute("value").Value);
            Clouds = int.Parse(element.Element("clouds").Attribute("value").Value);
            WeatherCode = element.Element("weather").Attribute("number").Value;
            Visibility = element.Element("visibility").Attribute("value").Value;
        }
    }
}