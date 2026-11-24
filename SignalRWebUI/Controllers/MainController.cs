using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUI.Controllers
{
    [AllowAnonymous]
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
