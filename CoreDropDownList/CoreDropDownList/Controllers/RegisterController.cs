using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DigitalPaymentManagement.Data;
using DigitalPaymentManagement.Models;
using DigitalPaymentManagement.ViewModel;
using DigitalPaymentManagement.SecurityOptions;

namespace DigitalPaymentManagement.Controllers
{

    //[AllowAnonymous]
    //[Authorize(Policy = "SuperAccountantPolicy")]
    [Authorize(Policy = "SrAccountantPolicy")]
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        //private readonly IWebHostEnvironment _hostingEnvironment;
         private readonly ILogger<AppUser> _logger;
        //private readonly IEmailSender _emailSender;
        public RegisterController(ApplicationDbContext context,
                                    RoleManager<IdentityRole> roleManager,
                                    SignInManager<AppUser> signInManager,
                                    ILogger<AppUser> logger,
                                    //IWebHostEnvironment hostingEnvironment,
                                    //IEmailSender emailSender,
                                    UserManager<AppUser> userManager)
        {
            this._context = context;
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
           //this._emailSender = emailSender;
        }



        [HttpGet] 
        //[Authorize(Policy = " SuperAccountantPolicy")]
        //[AllowAnonymous] 
        public async Task<IActionResult> ListUsers()
        {
            return View(await _userManager.Users.ToListAsync());
        }
  
        public IActionResult RetrieveImage(string id)
        {
            byte[] cover = GetImageFromDataBasee(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public byte[] GetImageFromDataBasee(string id)
        {
             
            var q = from temp in _context.Users where temp.Id == id select temp.Image;

            byte[] cover = q.FirstOrDefault();

            return cover;
        }

        [HttpGet]
        public async Task<IActionResult> EditPasswords(string userId)
        {


            var user = await _userManager.FindByIdAsync(userId);

            var model = new EditPasswordViewModel
            {
                CurrentPassword = encryptNewPass(user.PasswordHash),
                NewPassword="",
                ConfirmPassword=""
            };

            ViewBag.userId = userId;
            return View(model);
        }

        [HttpPost]
         public async Task<IActionResult> EditPasswords(EditPasswordViewModel model, string userId)
        {
            ViewBag.borrowerId = userId;
            if (ModelState.IsValid)
            {
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

                return RedirectToAction("ChangePasswordConfermation","Account");
            }

            return View(model);
        }




        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var tokend = await _userManager.GeneratePasswordResetTokenAsync(user);
          
            if (tokend == null || email == null)
            {
                ModelState.AddModelError("", "Invalid Password reset token!!!");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                 //var user1 = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.CurrentPassword);
                    //var result2 = await _userManager.ChangePasswordAsync(user,  model.Password, model.ConfirmPassword);
                    if (result.Succeeded)
                    {
                        return View(model);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                            ViewBag.errMasseges = error.Description;
                        }
                    }
                    return View(model);

                }
                else
                {
                    ViewBag.ErrorMessege = $"This Email {model.Email} is could not found!!!";
                    return View("Not Found");
                }

            }
            return View(model);
        }

         
        [HttpGet]
        public  IActionResult ForgotPassword() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
            
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordresetlink = Url.Action("ResetPassword", "Register",new {email=model.Email,token= token }, Request.Scheme);

                    _logger.Log(LogLevel.Warning, passwordresetlink);

                    return View("ForgotPasswordConfirmation");

                }
                return View("ForgotPasswordConfirmation");

            }
            return View(model);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult AddUser()
        {
            return View();
        }

        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(NewEmployee model, List<IFormFile> Image)
        {
            
            var r = (_userManager.Users.Any(e => e.UserName == model.UName));

            if (!r)
            {
                foreach (var item in Image)
                {
                    if (item.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await item.CopyToAsync(stream);
                            model.Image = stream.ToArray();
                        }
                    }
                    else
                    {
                        ViewBag.errMasseges = "There are no Image now !";
                    }
                }
                var dnameq = from temp in _context.Departments where temp.DepId == model.DepartmentDepId select temp.DepName;
                string DName = dnameq.FirstOrDefault();

                var dcode = from temp in _context.Departments where temp.DepId == model.DepartmentDepId select temp.DepCode;
                string DCode = dcode.FirstOrDefault();

                var user = new AppUser();

                user.UserName = model.Email;
                //user.UsrName = model.UName;
                user.EmpRegNo = EmpRegNo(model);
                user.Image = model.Image;
                user.DOB = (model.DOB).ToString();
                user.JoiningDate = DateTime.Now.Date;
                user.PhoneNumber = model.ContactNo.ToString();
                user.DepartmentDepId = model.DepartmentDepId;
                user.Description = (model.UName + "-" + DCode + "-" + model.DOB.Date.ToShortDateString() + "-" + EmpRegNo(model) + "-" + DateTime.Now.Year).ToString();

                user.Email = model.Email;
                user.CreationDate = DateTime.Now.Date;
                user.UserGender = model.Gender;
                user.UserAddress = model.Address;
                user.IsRole = model.IsSelected;
                user.PasswordHash = model.Password;
                user.RoleAdd = model.RoleAdd;

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddPasswordAsync(user, model.Password);
                    await _userManager.AddToRoleAsync(user, model.RoleAdd);
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("ListUsers", "Register");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }


                return RedirectToAction("ListUsers");
            }
            else
            {
                ViewBag.errMasseges = "This name is already admitted !!!";
            }


            return RedirectToAction("ListUsers");
        }

        private string EmpRegNo(NewEmployee model)
        {
            //List<NewEmployee> newuserList = new List<NewEmployee>(); 

            string add = "DPM";

            var days = model.DOB.Day.ToString();
            var months = model.DOB.Month.ToString();
            string Dobdat = Convert.ToDateTime(model.DOB).ToString();
            int Ddateln = Dobdat.Length;
            string DYear = Dobdat[Ddateln - 14].ToString() + Dobdat[Ddateln - 13].ToString();


            int joiningDateYear = model.JoiningDate.Year;
            int adUserJoiningDate = Convert.ToInt32(DateTime.Now.Year);

            var dnameq = from temp in _context.Departments where temp.DepId == model.DepartmentDepId select temp.DepName;
            string DName = dnameq.FirstOrDefault();

            var dcode = from temp in _context.Departments where temp.DepId == model.DepartmentDepId select temp.DepCode;
            string DCode = dcode.FirstOrDefault();

            string DepName = DName;
            string DepCo = DCode;

            var adUserdbYear = _context.Users.Select(a => a.JoiningDate.Year).ToList();
            var adUserdbDepart = _context.Departments.Select(a => a.DepName).ToList();


            string serial = null;
            //foreach (var item in adUserdbDepart)
            //{

            int flag;

            if (adUserdbDepart.Contains(DepName))
            {
                flag = 1;
                int sl1 = Convert.ToInt32(adUserdbDepart.Count);
                sl1++;
                serial = String.Format("{0:00000}", sl1);

            }
            else
            {
                flag = 0;
                int sl1 = 1;
                serial = String.Format("{0:00000}", sl1);
            }

            //}

            string EmpRegNo = add + "" + DepCo + "-" + days + "" + months + "" + DYear + "-" + joiningDateYear + "-" + serial;

            return EmpRegNo;
        }

        private DbSet<AppUser> GetUsers()
        {
            return _context.Users;
        }

        public string encryptNewPass(string PasswordHash)
        {
            string EncryptedData = null;
            try
            {
                byte[] EncDatabyte = new byte[PasswordHash.Length];
                EncDatabyte = System.Text.Encoding.UTF8.GetBytes(PasswordHash);
                EncryptedData = Convert.ToBase64String(EncDatabyte);
                return EncryptedData;
            }
            catch (Exception Er)
            {
                throw new Exception("Rerror Code " + Er.Message);
            };
        }

        private bool UsersExists(string name)
        {
            return _userManager.Users.Any(e => e.UserName == name);
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            string rgdepCode = GetDepCode(user.DepartmentDepId);

            var model = new EditUserModel
            {
                UserId = userId,
                Email = user.Email,
                EmpRegNo = user.EmpRegNo,
                UserName = user.UserName,
                UsrName = user.UsrName,
                Password = user.PasswordHash,
                RoleAdd = user.RoleAdd,
                JoiningDate = user.JoiningDate,
                DepartmentDepId = user.DepartmentDepId,

                PhoneNumber = user.PhoneNumber,
                Image = user.Image,
                UserGender = user.UserGender,
                UserAddress = user.UserAddress,

                Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
                Roles = userRoles

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel model, string userId, List<IFormFile> Image)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                foreach (var item in Image)
                {
                    if (item.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await item.CopyToAsync(stream);
                            model.Image = stream.ToArray();
                        }
                    }
                    else
                    {
                        ViewBag.errMasseges = "There are no Image now !";
                    }
                }

                if (model.EmpRegNo == null)
                {
                    user.EmpRegNo = EditEmpRegNo(model);
                }
                else
                {
                    user.EmpRegNo = model.EmpRegNo;
                }
                var dnameq = from temp in _context.Departments where temp.DepId == model.DepartmentDepId select temp.DepName;
                string DName = dnameq.FirstOrDefault();

                var dcode = from temp in _context.Departments where temp.DepId == model.DepartmentDepId select temp.DepCode;
                string DCode = dcode.FirstOrDefault();

                user.UserName = model.Email;
                user.UsrName = model.UsrName;
                user.Image = model.Image;
                user.DOB = (model.DOB).ToString();
                user.JoiningDate = model.JoiningDate;
                user.PhoneNumber = model.PhoneNumber.ToString();
                user.DepartmentDepId = model.DepartmentDepId;
                user.RoleAdd = model.RoleAdd;
                user.Email = model.Email;
                user.PasswordHash = model.Password;
                user.UserGender = model.UserGender;
                user.UserAddress = model.UserAddress;
                user.Description = ( model.UsrName + "-" +DCode+"-"+ model.DOB.Date.ToShortDateString() + "-" + EditEmpRegNo(model) + "-" + DateTime.Now.Year).ToString();
                user.IsRole = model.IsSelected;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.AddPasswordAsync(user, model.Password);
                    await _userManager.AddToRoleAsync(user, model.RoleAdd); 
                    //return RedirectToAction("EditUser", new { userId = userId });
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return RedirectToAction("ListUsers");

            }
        }


        private string EditEmpRegNo(EditUserModel model)
        {
            //List<NewEmployee> newuserList = new List<NewEmployee>();

            string add = "DPM";

            var days = model.DOB.Day.ToString();
            var months = model.DOB.Month.ToString();
            string Dobdat = Convert.ToDateTime(model.DOB).ToString();
            int Ddateln = Dobdat.Length;
            string DYear = Dobdat[Ddateln - 14].ToString() + Dobdat[Ddateln - 13].ToString();


            int joiningDateYear = model.JoiningDate.Year;
            int adUserJoiningDate = Convert.ToInt32(DateTime.Now.Year);

            var dnameq = from temp in _context.Departments where temp.DepId == model.DepartmentDepId select temp.DepName;
            string DName = dnameq.FirstOrDefault();

            var dcode = from temp in _context.Departments where temp.DepId == model.DepartmentDepId select temp.DepCode;
            string DCode = dcode.FirstOrDefault();

            string DepName = DName;
            string DepCo = DCode;

            var adUserdbYear = _userManager.Users.Select(a => a.JoiningDate.Year).ToList();
            var adUserdbDepart = _context.Departments.Select(a => a.DepName).ToList();

            string serial = null;

            int flag;

            if (adUserdbDepart.Contains(DepName))
            {
                flag = 1;
                int sl1 = Convert.ToInt32(adUserdbDepart.Count);
                sl1++;
                serial = String.Format("{0:00000}", sl1++);

            }
            else
            {
                flag = 0;
                int sl1 = 1;
                serial = String.Format("{0:00000}", sl1);
            }


            string EmpRegNo = add + " " + DepCo + "-" + days + "" + months + "" + DYear + "-" + joiningDateYear + "-" + serial;

            return EmpRegNo;
        }

        private string GetDepCode(int departmentDepId)
        {
            string getCode;
            var department = _context.Departments.FirstOrDefault(x => x.DepId == departmentDepId);
            getCode = department.DepCode;
            return getCode;
        }


        [HttpGet]
        public async Task<IActionResult> EditRoleInUser(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessege = $"This User {userId} is could not found!!!";
                return View("Not Found");
            }

            var model = new List<EditRoleInUser>();

            foreach (var role in _roleManager.Roles)
            {
                var userRoleViewModel = new EditRoleInUser
                {
                    RoleId = role.Id.ToString(),
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditRoleInUser(List<EditRoleInUser> model, string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessege = $"This User {userId} is could not found!!!";
                return View(model);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles !!");
                return RedirectToAction("EditUser", new { userId = userId });
            }
            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles in user !!!");
                return RedirectToAction("EditUser", new { userId = userId });
            }
            return RedirectToAction("EditUser", new { userId = userId });
        }



        [HttpGet]
        public async Task<IActionResult> ManageUserClaim(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.errMessage = "$User with Id=(id) can not be found.";
                return View("Not Found");
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimViewModel
            {
                UserId = userId
            };
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaims userclaims = new UserClaims()
                {
                    ClaimTypes = claim.Type
                };
                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    userclaims.IsSelected = true;
                }
                model.Claim.Add(userclaims);
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageUserClaim(UserClaimViewModel model, string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ViewBag.errMessage = "$User with Id={model.UderId} can not be found.";
                return View("Not Found");
            }

            var clms = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, clms);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can not remove user exit claims !!!");
                return View(model);
            }

            result = await _userManager.AddClaimsAsync(user, model.Claim.Select(c => new Claim(c.ClaimTypes, c.IsSelected ? "true" : "false")));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can not selected user claims to user !!!");
                return View(model);
            }

            return RedirectToAction("EditUser", new { userId = userId });
 
        }




        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string userId)
        {
            AppUser user = await _userManager.Users
                .Include(b => b.RoleAdd)
                .Include(b => b.Department)
                .FirstOrDefaultAsync(m => m.Id == userId); 
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("ListUsers");
                else
                {
                    ModelState.AddModelError("", "Can not remove user exit claims !!!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Can not remove user exit claims !!!");
            }
            return View("ListUsers", _userManager.Users);
        }




    }
}
