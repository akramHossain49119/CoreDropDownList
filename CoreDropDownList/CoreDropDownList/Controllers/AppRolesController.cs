using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CoreDropDownList.Data;
using CoreDropDownList.Models;
using CoreDropDownList.ViewModel;


namespace CoreDropDownList.Controllers
{

    [Authorize(Policy = "SrAccountantPolicy")]
    public class AppRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        //private readonly ILogger<RegisterController> _logger;
        private static IWebHostEnvironment _hostingEnvironment;
        //private IPasswordHasher<AppUser> passwordHasher;
        //private  readonly ClaimsIdentity _identity ;


        public AppRolesController(ApplicationDbContext context,
                                    RoleManager<IdentityRole> roleManager,
                                    SignInManager<AppUser> signInManager,
                                    //IPasswordHasher<AppUser> passwordHash,
                                    IWebHostEnvironment hostingEnvironment,
                                    //ClaimsIdentity identity,
                                    UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
            //_identity = identity;
        }

        // GET: AppRoles
        public IActionResult ListRoles()
        {
            var user = _roleManager.Roles;
            return View(user);
        }

        private bool UsersExists(string name)
        {
            return _userManager.Users.Any(e => e.UserName == name);
        }

        // GET: AppRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRole = await _roleManager.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appRole == null)
            {
                return NotFound();
            }

            return View(appRole);
        }



        // GET: AppRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.Name
                };
                IdentityResult result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return RedirectToAction(nameof(ListRoles));
            }
            return View(model);
        }




        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            //var users = await _userManager.FindByIdAsync(id); 

            if (role == null)
            {
                ViewBag.ErrorMessege = $"This User {roleId} is could not found!!!";
                return View("NotFound");
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            // var rolesUser = await _userManager.GetUserAsync(role); 

            var model = new EditRoleViewModel
            {
                RoleId = role.Id.ToString(),
                RoleName = role.Name,
                Claims = roleClaims.Select(c => c.Value).ToList()

            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model, string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);


            if (role == null)
            {
                ViewBag.ErrorMessege = $"This User {roleId} is could not found!!!";
                return View("NotFound");
            }
            else
            {
                role.Id = model.RoleId;
                role.Name = model.RoleName;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var roles = await _roleManager.FindByIdAsync(roleId);
            if (roles == null)
            {
                ViewBag.ErrorMessege = $"This User {roleId} is could not found!!!";
                return View("Not Found");
            }

            var model = new List<EditUserInRole>();


            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new EditUserInRole
                {
                    UserId = (user.Id).ToString(),
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, roles.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else if (await _userManager.IsInRoleAsync(user, roles.Name))
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<EditUserInRole> model, string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessege = $"This User {@roleId} is could not found!!!";
                //  return Json(true);
                return View();
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < model.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { roleId = roleId });
                    }

                }

            }

            return RedirectToAction("EditRole", new { roleId = roleId });

        }


        [HttpGet]
        public async Task<IActionResult> RoleClaimManage(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.errMessage = "$User with Id=(id) can not be found.";
                return View("Not Found");
            }

            var existingUserClaims = await _roleManager.GetClaimsAsync(role);

            var model = new RoleClaimViewModel
            {
                RoleId = roleId
            };
            foreach (Claim claim in RolesClaimsStore.AllClaims)
            {
                RoleClaims roleclaims = new RoleClaims()
                {
                    RoleClaimTypes = claim.Type
                };
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    roleclaims.IsSelected = true;
                }
                model.RoleClaim.Add(roleclaims);
            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> RoleClaimManage(RoleClaimViewModel model, string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.errMessage = "$User with Id={model.UderId} can not be found.";
                return View("Not Found");
            }

            var clms = await _roleManager.GetClaimsAsync(role);
            var result = await _roleManager.RemoveClaimAsync(role, (Claim)clms);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can not remove user exit claims !!!");
                return View(model);
            }

            result = await _roleManager.AddClaimAsync(role, (Claim)model.RoleClaim.Where(c => c.IsSelected).Select(c => new Claim(c.RoleClaimTypes, c.RoleClaimTypes)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can not selected user claims to user !!!");
                return View(model);
            }

            return RedirectToAction("EditRole" +
                "", new { Id = model.UserId });
            //return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRole = await _roleManager.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appRole == null)
            {
                return NotFound();
            }
            await _roleManager.DeleteAsync(appRole);
            await _context.SaveChangesAsync();
            return View(appRole);
        }

        // POST: AppRoles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[Authorize(Policy= "Delete")]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var appRole = await _roleManager.FindByIdAsync(id);
            if (appRole == null)
            {
                ViewBag.errMessage = "$Role with Id=(id) can not be found.";
                return View("Not Found");
            }
            else
            {
                await _roleManager.DeleteAsync(appRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

        }

        private bool AppRoleExists(string id)
        {
            return _roleManager.Roles.Any(e => e.Id == id);
        }

    }
}
