using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CoreDropDownList.Data;
using CoreDropDownList.Models;

namespace CoreDropDownList.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public DashboardController(ApplicationDbContext context,
                                UserManager<AppUser> userManager,
                                SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index() 
        {
              string id = ViewBag.id;

             ViewBag.id = id;
              return View();
        }
    }
}
