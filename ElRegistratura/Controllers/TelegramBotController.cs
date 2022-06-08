using Microsoft.AspNetCore.Mvc;

namespace ElRegistratura.Controllers
{
    public class TelegramBotController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
