using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CoreDropDownList.Data;
using CoreDropDownList.Models;
using CoreDropDownList.ViewModel;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection.Metadata.Ecma335;
using System.Net.Sockets;

namespace CoreDropDownList.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(ApplicationDbContext context,
                                UserManager<AppUser> userManager,
                                SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string email,  string returnUrl)
        {


            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if ((result.Succeeded))
                {
                    return RedirectToAction("Index", "Dashboard");
                 }
                ModelState.AddModelError(string.Empty, "Invalid login user Name or Password !!!");
            }
            string userId = _userManager.Users.Single(x => x.Email == model.Email).Id;
            ViewBag.Id = userId;
            return View(model);
        }


        /// <summary>
        /// Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult RetrieveImageLog(string id) 
        {
            var urId = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            string userId = urId.Id;

            byte[] cover = GetImageFromDataBasee(userId);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public byte[] GetImageFromDataBasee(string userId)
        {

            var q = from temp in _context.Users where temp.Id == userId select temp.Image;

            byte[] cover = q.FirstOrDefault();

            return cover;
        }

        //By User Name/Email

        public IActionResult RetrieveImageName(string name)
        {
            byte[] cover = GetImageFromDataBased(name);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
         
        public byte[] GetImageFromDataBased(string name)
        {

            var q = from temp in _context.Users where temp.UserName == name select temp.Image;

            byte[] cover = q.FirstOrDefault();

            return cover;
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
               
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);

                return View("ChangePasswordConfermation");

            }

            return View(model);
        }



        // GET: Account
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "LandingPage");
        }


    }
}
