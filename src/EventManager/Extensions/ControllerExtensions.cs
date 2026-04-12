using EventsManager.Failures;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventManager.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ErrorToActionResult(
            this ControllerBase controllerBase, 
            HttpError error)
        {
            IActionResult result;
            string message = error.Message;

            switch (error.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    {
                        result = controllerBase.BadRequest(message);
                        break;
                    }

                case HttpStatusCode.NotFound:
                    {
                        result = controllerBase.NotFound(message);
                        break;
                    }

                case HttpStatusCode.InternalServerError:
                    {
                        result = controllerBase.StatusCode(500, message);
                        break;
                    }

                default:
                    {
                        result = controllerBase.StatusCode((int)error.StatusCode, message);
                        break;
                    }
            }

            return result;
        }
    }
}
