using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTelegramBot.Services.Interfaces
{
    public interface IWeatherService
    {
        string GetWeatherMessage(string message);
    }
}
