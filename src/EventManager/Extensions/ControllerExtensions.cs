using Microsoft.AspNetCore.Mvc;
using Shared;

namespace EventManager.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ErrorToActionResult(
            this ControllerBase controllerBase, 
            Error error)
        {
            IActionResult result;
            string message = error.Message;

            switch (error.StatusCode)
            {
                case 400:
                    {
                        result = controllerBase.BadRequest(message);
                        break;
                    }

                case 404:
                    {
                        result = controllerBase.NotFound(message);
                        break;
                    }

                case 500:
                    {
                        result = controllerBase.StatusCode(500, message);
                        break;
                    }

                default:
                    {
                        result = controllerBase.StatusCode(500, message);
                        break;
                    }
            }

            return result;
        }
    }
}
