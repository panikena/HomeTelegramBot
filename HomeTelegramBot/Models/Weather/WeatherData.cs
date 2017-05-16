using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace HomeTelegramBot.Models.Weather
{
    public abstract class WeatherData
    {
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public string WeatherCode { get; set; }
        public string WindDirectionCode { get; set; }
        public double RainPrecipitation { get; set; }
        public double SnowPrecipitation { get; set; }
        public double WindAzimuth { get; set; }
        public double WindSpeed { get; set; }
        public double Temperature { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public double Pressure { get; set; }
        public int Humidity { get; set; }
        public int Clouds { get; set; }
    }
}