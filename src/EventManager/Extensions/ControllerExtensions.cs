using EventManager.Shared;
using Microsoft.AspNetCore.Mvc;
using StatusCodes = EventManager.Shared.StatusCodes;

namespace EventManager.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ErrorToActionResult(this ControllerBase controllerBase, Error error)
        {
            IActionResult result;
            string message = error.Message;

            switch (error.StatusCode)
            {
                case StatusCodes.NotFound:
                    result = controllerBase.NotFound(message);
                    break;

                case StatusCodes.BadRequest:
                    result = controllerBase.BadRequest(message);
                    break;

                default:
                    result = controllerBase.StatusCode(500, message);
                    break;
            }

            return result;
        }
    }
}
