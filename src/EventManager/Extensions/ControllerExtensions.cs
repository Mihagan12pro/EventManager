using EventsManager.Failures;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventManager.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ErrorToActionResult(this ControllerBase controllerBase, HttpError error)
            => controllerBase.StatusCode((int)error.StatusCode, error.Message);
    }
}
