using HomeTelegramBot.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HomeTelegramBot.Controllers
{
    public class SettingsController : ApiController
    {
       // ILog _logger = LogManager.GetLogger(typeof(SettingsController));

        public SettingsController()
        {

        }


        public HttpResponseMessage StopBot()
        {
            try
            {
                Bot.Get().StopReceiving();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
         //       _logger.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        public HttpResponseMessage StartBot()
        {
            try
            {
                Bot.Get().StopReceiving();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
           //     _logger.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

    }
}
