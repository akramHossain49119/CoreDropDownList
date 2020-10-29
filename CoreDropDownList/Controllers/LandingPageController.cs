using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreDropDownList.Controllers
{
    public class LandingPageController : Controller
    {
        public IActionResult Index(string userName)
        {
            ViewBag.userName = userName;
            return View();
        }
    }
}
