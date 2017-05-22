using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public string GetWeatherDescription()
        {
            var message = new StringBuilder();

            //temperature
            message.Append(Properties.Resources.Temperature);
            message.Append(": ");
            message.Append(Temperature.ToString());
            message.AppendLine(" °C");
            //wind
            message.Append(Properties.Resources.Wind);
            message.Append(": ");
            message.Append(WindSpeed);
            message.Append(" ");
            message.Append(Properties.Resources.MetersPerSecond);
            message.Append(", ");
            message.AppendLine(WindDirectionCode.ToString());
            //humidity
            message.Append(Properties.Resources.Humidity);
            message.Append(": ");
            message.Append(Humidity);
            message.AppendLine("%");
            //pressure
            message.Append(Properties.Resources.Pressure);
            message.Append(": ");
            message.Append(Pressure);
            message.Append(" " + Properties.Resources.PressureUnits);

            return message.ToString();
        }
    }
}
