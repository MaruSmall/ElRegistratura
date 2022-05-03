using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElRegistratura.Data
{
    public class ErrorDB : Controller
    {
        // GET: ErrorDB
        public IActionResult ErrorDBView()
        {
            return View();
        }

       
    }
}
