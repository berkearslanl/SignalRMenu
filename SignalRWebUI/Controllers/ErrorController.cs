using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUI.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public IActionResult Page404()
        {
            return View();
        }
        public IActionResult Page500()
        {
            return View();
        }
    }
}
