using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreDropDownList.Data;
using CoreDropDownList.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using CoreDropDownList.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Claims;
using CoreDropDownList.SecurityOptions;

namespace CoreDropDownList.Controllers
{
    [Authorize(Policy = "JrAccountantPolicy")]
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public MembersController(ApplicationDbContext context,
                                    RoleManager<IdentityRole> roleManager,
                                    SignInManager<AppUser> signInManager,
                                    UserManager<AppUser> userManager)
        {
            this._context = context;
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

 
        public async Task<IActionResult> Index()
        {
            var members = _context.Members.ToListAsync();
            return View(await members); 
        }


        [HttpPost]
        public IActionResult IsApproved(int[] ids)
        {

            byte[] img = RetrieveImageColl(  ids);

            try
            {

                var entity = _context.Members.Where(i => ids.Contains(i.Id)); 

                foreach (var e in entity)
                {
                    e.IsApproved = true;
                    _context.Members.Update(e);


                }
                _context.SaveChanges();
                TempData["success"] = "Data approved!";

                return Json(new { success = true, redirecturl = Url.Action("Index", "Members") });
            }
            catch (Exception e)
            {
                TempData["success"] = e.Message;
                return Json(new { success = false, message = e.Message });
            }

        }


        [HttpPost]
        public IActionResult IsDisbursement(int[] ids)
        {


            try
            {

                var entity = _context.Members.Where(i => ids.Contains(i.Id)); 

                foreach (var e in entity)
                {

                    int intims = 46;

                    Collection collect = new Collection();

                    collect.UserId = e.UserId;
                    collect.MemberId = e.Id; 
                    collect.UserCollectionName = User.Identity.Name;
                    collect.LoanAmount = e.ProposeLoanAmount;
                    collect.ServiceAmount = GetServiceAmount(collect);
                    collect.SecurityAmount = GetSecurityAmount(collect);
                    collect.InstallmentTimes = intims;
                    collect.InstallmentDeposit = GetInstallmentDeposit(collect);
                    collect.InstallmentAmount = collect.GetInstallmentAmount;
                    collect.BalancedAmount = e.ProposeLoanAmount;
                    collect.InstallmentDay = "Sunday";
                    collect.InstallmentDate = DateTime.Now.Date;
                    collect.BalancedAmount = e.ProposeLoanAmount;
                    collect.TotalInstallmentAmount = collect.GetTotalInstallmentAmount;
                    collect.UserCollectionName = User.Identity.Name;

                    collect.BranchId = e.BranchId;
                    collect.CenterId = e.CenterId;
                    collect.AlternativeContactNo = e.MbrAlterContactNo;
                    collect.ContactNo = e.MbrContactNo;
                    collect.HeadquartersName = e.HeadquartersName;
                    collect.MembRegNo = e.MemRegNo;
                    _context.Collections.Add(collect);

                    e.IsDisbursement = true;
                    _context.Members.Update(e);

                }
                _context.SaveChanges();
                TempData["success"] = "Data approved!";

                return Json(new { success = true, redirecturl = Url.Action("Index", "Members") });
            }
            catch (Exception e)
            {
                TempData["success"] = e.Message;
                return Json(new { success = false, message = e.Message });
            }

        }


        private byte[] RetrieveImageColl(int[] ids)
        {
            byte[] cover = GetImageFromDataBaseeColl(ids);
            if (cover != null)
            {
                return cover;
            }
            else
            {
                return null;
            }
        }


        public byte[] GetImageFromDataBaseeColl(int[] ids)
        {
            var q = _context.Members.Where(i => ids.Contains(i.Id)).Select(i => i.Image);
            //var q = from temp in _context.Members where temp.Id == ids.Contains(temp.Id) select temp.Image;

            byte[] cover = q.FirstOrDefault();

            return cover; 
        }


        private double GetSecurityAmount(Collection collect)
        {
            double servCrg = 0;
            double approvedLoadn = collect.LoanAmount;

            if (approvedLoadn == 0)
            {
                servCrg = 0;
            }
            else
            {
                servCrg = ((approvedLoadn / 100) * 10);
            }
            return servCrg;
        }

        private double GetServiceAmount(Collection collect)
        {
            double servCrg = 0;
            double approvedLoadn = collect.LoanAmount;

            if (approvedLoadn == 0)
            {
                servCrg = 0;
            }
            else
            {
                servCrg = ((approvedLoadn / 100) * 25);
            }
            return servCrg;
        }

        private double GetInstallmentDeposit(Collection model)
        {

            double servDep = model.InstallmentDeposit;
            double propsLoadn = model.LoanAmount;

            if (model.LoanAmount == 0)
            {

                servDep = 0;

            }
            else
            {
                if (propsLoadn == 20000)
                {
                    servDep = 50;
                }
                else if (propsLoadn == 15000)
                {
                    servDep = 35;
                }
                else if (propsLoadn == 10000)
                {
                    servDep = 30;
                }
                else if (propsLoadn == 5000)
                {
                    servDep = 25;
                }

            }
            return servDep;
        }



        [HttpGet]
        public IActionResult GetCentersName(int centerId)
        {
            string cNme = GetCenName(centerId);
             if(cNme!=null)
            {
                return View(cNme);
            }
            return null;
        }

        public string GetCenName(int centerId)
        {
            var cenName = (from c in _context.Centers join b in _context.Branchs on c.Id equals b.CenterId where b.CenterId == centerId select c.CenterName);
            string cen = cenName.ToString();
            return cen;
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.BranchId)
                .FirstOrDefaultAsync(m => m.Id == id); 
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        private List<Branch> GetAllBranchess()
        {
            List<Branch> dep = new List<Branch>();
            dep = _context.Branchs.ToList();
            return dep;
        }


        //private List<Member> GetMemberUserName() 
        //{
        //    List<Member> dep = new List<Member>();
        //    dep = (from c in _context.Members select c).ToList();
        //    dep.Insert(0, new Member { Id = 0, MemberName = "--Select Member Name--" });
        //    return dep;
        //}


 
        //Center-brance
        [HttpPost]
        public IActionResult ChengeBrances(int id)
        {
            var brancess = _context.Branchs.Find(id);
            return Json(brancess);
        }


        public IActionResult RetrieveImagebwr(int id)
        {
            byte[] cover = GetImageFromDataBaseebwr(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public byte[] GetImageFromDataBaseebwr(int id)
        {

            var q = from temp in _context.Members where temp.Id == id select temp.Image;

            byte[] cover = q.FirstOrDefault();

            return cover; 
        }


        /// <summary>
        /// RetrieveImagebwrApp
        /// </summary>

        public IActionResult RetrieveImagebwrApp(int id)
        {
            byte[] cover = GetImageFromDataBaseebwrApp(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public byte[] GetImageFromDataBaseebwrApp(int id)
        {

            var q = from temp in _context.Members where temp.Id == id select temp.Image;
            byte[] cover = q.FirstOrDefault();

            return cover;
        }
        /// <returns></returns> 
        /// 


        //GetCenter
        private List<Center> GetCenter()
        {
            List<Center> centerslist = new List<Center>(); 
            centerslist = _context.Centers.ToList();
            centerslist.Insert(0, new Center {Id = 0, CenterName = "--Select Center--" });
            return centerslist; 
        }

        //GetBranch
        private List<Branch> GetBranch()
        {
            List<Branch> dep = new List<Branch>();
            dep = _context.Branchs.ToList();
            dep.Insert(0, new Branch { Id = 0, BranchName = "--Select Brance--" });
            return dep;
        }
  
        public JsonResult GetBranchesbyCenterId(int centerId)
        {
            List<Branch> branches = new List<Branch>();
            branches = _context.Branchs.Where(x => x.CenterId == centerId).ToList();
            branches.Insert(0, new Branch { Id = 0, BranchName = "--Select a Branch--" });
            return Json(new SelectList(branches, "BranchId", "BranchName"));

        }

        public JsonResult GetMembersbyBranchId(int branchId)   
        { 
            List<Member> members = new List<Member>();
            members = _context.Members.Where(x => x.BranchId == branchId).ToList();
            members.Insert(0, new Member { Id = 0, MemberName = "--Select a Member--" });
            return Json(new SelectList(members, "Id", "MemberName"));

        }

        public IActionResult GetMembersbCenterId(int centerId)
        {
            List<Member> members = new List<Member>();
            members = _context.Members.Where(x => x.CenterId == centerId).ToList();
            members.Insert(0, new Member { Id = 0, MemberName = "--Select a Member--" });
            return Json(new SelectList(members, "Id", "MemberName"));
        }

        public JsonResult GetMembersinfobyMemberId(int memberId)
        {
            List<Member> memberslist = new List<Member>();
            var q = _context.Members.Where(x => x.Id == memberId); 
            var r = _context.Members.Where(x => x.Id == memberId).Select(c=>c.Image);

            byte[] imge = r.FirstOrDefault();

            Member members = new Member();
            members.MbrNomineeAddress = q.Select(x => x.HeadquartersName).ToString();
            members.MbrAlterContactNo = q.Select(x => x.MbrAlterContactNo).ToString();
            members.MbrContactNo = q.Select(x => x.MbrContactNo).ToString();
            members.MbrEmail = q.Select(x => x.MbrEmail).ToString();
            members.MbrFName = q.Select(x => x.MbrFName).ToString();
            members.MbrMName = q.Select(x => x.MbrMName).ToString();
            members.MbrMName = q.Select(x => x.MbrMName).ToString();
            members.MbrNID = q.Select(x => x.MbrNID).ToString();
            members.MemRegNo = q.Select(x => x.MemRegNo).ToString();
            members.CreationDate = Convert.ToDateTime(q.Select(x => x.CreationDate).ToString());
            members.Image = imge;
            members.MbrNID = q.Select(x => x.MbrNID).ToString();
            members.MbrProfession = q.Select(x => x.MbrProfession).ToString();
            members.MbrNominee = q.Select(x => x.MbrNominee).ToString();
            members.MbrGender = q.Select(x => x.MbrGender).ToString();
            members.MbrContactNo = q.Select(x => x.MbrContactNo).ToString();
            members.MbrPresentAddress = q.Select(x => x.MbrPresentAddress).ToString();

            memberslist.Add(members);


            return Json(new SelectList(memberslist, "MemberId", "BranchName"));

        }


        public IActionResult GetMembersbyCenterId(int branchId)
        {
            if (branchId != 0)
            { 
                List<Member> members = new List<Member>();
                members = _context.Members.Where(p => p.BranchId == branchId).ToList();
                members.Insert(0, new Member { Id = 0, MemberName = "--Select a Branch--" });
                return Json(new SelectList(members, "Id", "MemberName"));
            }
            return null;
        }


        [HttpGet]
        public IActionResult AdmissionMember()
        {
            var usId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string usrId = Convert.ToInt32(usId.FirstOrDefault()).ToString();
          
            ViewBag.Center = GetCenters();
    
            Member x = new Member();

            var modelView = new MemberViewModel
            {
                Centersh = GetCenters(), 

                MbrAlterContactNo = x.MbrAlterContactNo,
                MbrPresentAddress = x.MbrPresentAddress,
                MbrNomineeAddress = x.MbrNomineeAddress,
                MbrNominee = x.MbrNominee,
                MbrNID = x.MbrNID,
                MbrMName = x.MbrMName,
                MbrContactNo = x.MbrContactNo,
                MbrEmail = x.MbrEmail,
                MbrFName = x.MbrFName,
                MbrGender = x.MbrGender,
                CreationDate = x.CreationDate,
                MbrProfession = x.MbrProfession,
                MemberId = x.Id, 
                MemberName = x.MemberName,
                HeadquartersName = x.HeadquartersName,
                MemRegNo = x.MemRegNo,
                BranchId = x.BranchId,
                CenterId = x.CenterId,
                Image = x.Image,
                UserId = usrId
            };

            return View(modelView);
        }

        [HttpPost]
        public async Task<IActionResult> AdmissionMember(MemberViewModel model, List<IFormFile> Image)
        {

            var userNameAdd = User.FindFirstValue(ClaimTypes.Name);
            var userIdAdd = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var q = from temp in _context.Users where temp.Id == userIdAdd select temp.UsrName;
            //string useName = q.FirstOrDefault().ToString();
            string centernamed = (from c in _context.Centers where c.Id == model.CenterId select c.CenterName).FirstOrDefault();


            var r = (_context.Members.Any(e => e.MemberName == model.MemberName));
            var el = (_context.Members.Any(e => e.MbrEmail == model.MbrEmail));

            if (!r||!el)
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

                
                //string centernamed = ceName.ToString();

                //var cenName = from c in _context.Centers where c.Id == model.CenterId select c.CenterName;
                //string centername = cenName.FirstOrDefault().ToString();

                Member newmenber = new Member();
                newmenber.MemberName = model.MemberName;
                newmenber.MbrGender = model.MbrGender;
                newmenber.UserId = userIdAdd;
                newmenber.MemCreatorName = userNameAdd;
                newmenber.Image = model.Image;
                newmenber.MemRegNo = MemberRegNo(model);
                newmenber.CreationDate = (DateTime.Now.Date);
                newmenber.HeadquartersName = "Barishal";
                newmenber.MbrAlterContactNo = model.MbrAlterContactNo;
                newmenber.MbrNomineeAddress = model.MbrNomineeAddress;
                newmenber.BranchId = model.BranchId;
                newmenber.CenterId = model.CenterId;
                newmenber.CenNames = centernamed;
                newmenber.MbrNID = model.MbrNID;
                newmenber.MbrNominee = model.MbrNominee;
                newmenber.MbrPresentAddress = model.MbrPresentAddress;
                newmenber.MbrProfession = model.MbrProfession;
                newmenber.MbrContactNo = model.MbrContactNo;
                newmenber.CreationDate = model.CreationDate;
                newmenber.MbrEmail = model.MbrEmail;
                newmenber.MbrFName = model.MbrFName;
                newmenber.MbrMName = model.MbrMName;
                newmenber.Id = model.MemberId;  
                newmenber.ProposeLoanAmount = model.ProposeLoanAmount;

                var rsl = _context.Members.Add(newmenber);
                if (rsl != null)
                {
                    await _context.SaveChangesAsync();
                   return RedirectToAction("Index", "Members");
                   //return Json(RedirectToAction("Index", "Members"));
                }
                else
                {
                    ViewBag.errMasseges = "This Information is not  saved !!!";
                    return View(model); 
                }

            }

            else
            {
                if(r)
                {
                    ViewBag.errMasseges = "This name is already a member !!!";
                }
                else if(el) 
                {
                    ViewBag.errMasseges = "This Email Address is already a added !!!";
                }

            }

            ViewBag.Center = GetCenter();
            ViewBag.useName = userNameAdd;
            return RedirectToAction("Index", "Members");
        }


        private string MemberRegNo(MemberViewModel model) 
        {

            string add = "DPMIS";

            var days = model.CreationDate.Day.ToString();
            var months = model.CreationDate.Month.ToString();
            string Dobdat = Convert.ToDateTime(model.CreationDate).ToString();
            int Ddateln = Dobdat.Length;
            string DYear = Dobdat[Ddateln - 14].ToString() + Dobdat[Ddateln - 13].ToString();

            int adUserJoiningDate = Convert.ToInt32(DateTime.Now.Year);


            string CenName = (from c in _context.Centers where c.Id == model.CenterId select c.CenterName).FirstOrDefault();
            //string CenName = cnName.FirstOrDefault(); 
            //string DepCenter = CenName;


            string cdoc = (from temp in _context.Centers where temp.Id == model.CenterId select temp.CenterCode).FirstOrDefault();
            //string cdoc = cooddee.FirstOrDefault();

            string bCode = (from temp in _context.Branchs where  temp.Id == model.BranchId select temp.BranchCode).FirstOrDefault();
           // string bCode = bCod.FirstOrDefault();


           // var adMemberCreationYear = _context.Members.Select(a => a.CreationDate.Year).ToList();
            //var addMemberCenter = _context.Centers.Select(a => a.CenterName).ToList();
            var adMemberId = _context.Members
                .Where(a=>a.BranchId==model.BranchId)
                .Select(a => a.Id).ToList();

            string serial = null;

            int flag;

            if (adMemberId.Contains(model.BranchId)) 
            {
                flag = 1;
                int sl1 = Convert.ToInt32(adMemberId.Count);
                sl1++;
                serial = String.Format("{0:00000}", sl1);

            }
            else
            {
                flag = 0;
                int sl1 = 1;
                serial = String.Format("{0:00000}", sl1);
            }


            string EmpRegNo = add + "" + cdoc + "-" + days + "" + months + "" + DYear + "" + bCode + "-" + serial;

            return EmpRegNo;
        }


        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rs = await _context.Members.FindAsync(id);

            if (rs == null)
            { 
                return NotFound();
            }
            MemberViewModel x = new MemberViewModel(); 

            var modelView = new CascadingModel 
            {
                Centerss = GetCenter(),
                //Branchs = x.Branchss,
                MemberId = rs.Id, 
                MbrAlterContactNo = rs.MbrAlterContactNo,
                MbrPresentAddress = rs.MbrPresentAddress,
                MbrNomineeAddress = rs.MbrNomineeAddress,
                MbrNominee = rs.MbrNominee,
                MbrNID = rs.MbrNID,
                MbrMName = rs.MbrMName,
                MbrContactNo = rs.MbrContactNo,
                MbrEmail = rs.MbrEmail,
                MbrFName = rs.MbrFName,
                MbrGender = rs.MbrGender,
                CreationDate = rs.CreationDate,
                MbrProfession = rs.MbrProfession,
                MemberName = rs.MemberName,
                HeadquartersName = rs.HeadquartersName,
                MemRegNo = rs.MemRegNo,
                ProposeLoanAmount = rs.ProposeLoanAmount,
                BranchId = rs.BranchId,
                CenterId = rs.CenterId,
                Image = rs.Image,
                UserId = rs.UserId
            };


            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "BranchName", modelView.BranchId);
            return View( modelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MemberViewModel model, List<IFormFile> Image)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (model.MemRegNo == null)
            {
                model.MemRegNo = MemberReg(model);
            }
            else
            {
                model.MemRegNo = model.MemRegNo;
            }

            if (ModelState.IsValid)
            {
                try
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

                    Member newmenber = new Member();

                    newmenber.MemberName = model.MemberName;
                    newmenber.MbrGender = model.MbrGender;
                    newmenber.ProposeLoanAmount = model.ProposeLoanAmount;
                    newmenber.Image = model.Image;
                    newmenber.UserId = model.UserId;
                    newmenber.MemRegNo = MemberRegNo(model);
                    newmenber.CreationDate = (DateTime.Now.Date);
                    newmenber.HeadquartersName = "Barishal";
                    newmenber.ProposeLoanAmount = model.ProposeLoanAmount;
                    newmenber.MbrAlterContactNo = model.MbrAlterContactNo;
                    newmenber.MbrNomineeAddress = model.MbrNomineeAddress;
                    newmenber.BranchId = model.BranchId;
                    newmenber.CenterId =  model.CenterId ;
                    newmenber.MbrNID = model.MbrNID;
                    newmenber.MbrNominee = model.MbrNominee;
                    newmenber.MbrPresentAddress = model.MbrPresentAddress;
                    newmenber.MbrProfession = model.MbrProfession;
                     newmenber.CenNames = (from temp in _context.Centers where temp.Id == model.CenterId select temp.CenterName).FirstOrDefault();
                    newmenber.MbrContactNo = model.MbrContactNo;
                    //newmenber.CreationDate = model.CreationDate;
                    newmenber.MbrEmail = model.MbrEmail;
                    newmenber.MbrFName = model.MbrFName;
                    newmenber.MbrMName = model.MbrMName;

                    var rsl = _context.Members.Add(newmenber);
                    if (rsl != null)
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Members");
                    }
                    else
                    {
                        ViewBag.errMasseges = "This Information is not  saved !!!";
                    }


                    _context.Members.Update(newmenber);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branchs, "Id", "BranchName", model.BranchId);
            return View(model);
        }

        private string MemberReg(MemberViewModel model) 
        {
            string add = "DPMIS";

            var days = model.CreationDate.Day.ToString();
            var months = model.CreationDate.Month.ToString();
            string Dobdat = Convert.ToDateTime(model.CreationDate).ToString();
            int Ddateln = Dobdat.Length;
            string DYear = Dobdat[Ddateln - 14].ToString() + Dobdat[Ddateln - 13].ToString();

            int adUserJoiningDate = Convert.ToInt32(DateTime.Now.Year);


            var cnName = (from c in _context.Centers join b in _context.Branchs on c.Id equals b.CenterId where b.Id == model.BranchId select c.CenterName);
            string CenName = cnName.FirstOrDefault();
            string DepCenter = CenName;

            var cooddee = (from c in _context.Centers join b in _context.Branchs on c.Id equals b.CenterId where b.Id == model.BranchId select c.CenterCode);

            string cdoc = cooddee.FirstOrDefault();

            var adMemberCreationYear = _context.Members.Select(a => a.CreationDate.Year).ToList();
            var addMemberCenter = _context.Centers.Select(a => a.CenterName).ToList();
            var adMemberId = _context.Members.Select(a => a.Id).ToList();  

            string serial = null;

            int flag;

            if (adMemberId.Contains(model.Id))
            {
                flag = 1;
                int sl1 = Convert.ToInt32(adMemberId.Count);
                sl1++;
                serial = String.Format("{0:00000}", sl1);

            }
            else
            {
                flag = 0;
                int sl1 = 1;
                serial = String.Format("{0:00000}", sl1);
            }


            string EmpRegNo = add + "" + cdoc + "-" + days + "" + months + "" + DYear + "-" + "-" + serial;

            return EmpRegNo;
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.BranchId)
                .FirstOrDefaultAsync(m => m.Id == id); 
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);  
        }



        //CenterName

        private bool CenterNameExists(string centerName)
        {
            return _context.Centers.Any(e => e.CenterName == centerName);
        }
        private bool BranceNameExists(string branceName)
        {
            return _context.Branchs.Any(e => e.BranchName == branceName);
        }



        /// <summary>
        /// Have change this Code for active
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        //IEnumerable<SelectListItem> Centers
        public IEnumerable<SelectListItem> GetCenters()
        {

            List<SelectListItem> centers = _context.Centers.AsNoTracking()
                .OrderBy(n => n.CenterName)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.CenterName
                    }).ToList();
            var countrytip = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Center ---"
            };
            centers.Insert(0, countrytip);
            return new SelectList(centers, "Value", "Text");

        }


        //Get IEnumerable<SelectListItem> GetBranchs
        public IEnumerable<SelectListItem> GetBranchs()
        {

            List<SelectListItem> branchs = _context.Branchs.AsNoTracking()
                .OrderBy(n => n.BranchName)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.BranchName
                    }).ToList();
            var countrytip = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Branch ---"
            };
            branchs.Insert(0, countrytip);
            return new SelectList(branchs, "Value", "Text");

        }


        [HttpGet]
        public IActionResult IndexApproved()
        {
            return View(_context.Collections.ToList());
        }


        public IActionResult RetrieveImagebwApred(int id)
        {
            byte[] cover = GetImageFromDataBaseebwrApred(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public byte[] GetImageFromDataBaseebwrApred(int id) 
        {

            var q = from temp in _context.Members where temp.Id == id && temp.IsApproved==true select temp.Image;

            byte[] cover = q.FirstOrDefault(); 

            return cover;
        }

    }
}