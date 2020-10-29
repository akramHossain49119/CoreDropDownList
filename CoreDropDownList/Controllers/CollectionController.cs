using CoreDropDownList.Data;
using CoreDropDownList.Models;
using CoreDropDownList.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDropDownList.Controllers
{
    [Authorize(Policy = "JrAccountantPolicy")]
    public class CollectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public CollectionController(ApplicationDbContext context,
                                    RoleManager<IdentityRole> roleManager,
                                    SignInManager<AppUser> signInManager,
                                    //IWebHostEnvironment hostingEnvironment,
                                    //IEmailSender emailSender,
                                    UserManager<AppUser> userManager)
        {
            this._context = context;
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._signInManager = signInManager;
            //this._emailSender = emailSender;
        }


        // GET: Center
        public async Task<IActionResult> Index()
        {
              return View(await _context.Collections.ToListAsync());
        }

       [HttpGet]
        public IActionResult StoreCollection(int id)
        {
            string usNam = User.Identity.Name;

            ViewBag.memberId = id;
            var memb = _context.Members.Find(id);

            var membersName = (from temp in _context.Members where temp.Id == id select temp.MemberName);
            string memsName = membersName.FirstOrDefault();
            ViewBag.memberName = memsName;

            var centersName = (from temp in _context.Members where temp.Id == id select temp.CenNames);
            string centsName = centersName.FirstOrDefault();
            ViewBag.centerName = centsName;


            //var brnName = (from m in _context.Members join b in _context.Branchs on m.BranchId equals b.BranchId 
            //                                where m.BranchId == memb.BranchId select b.BranchName);
            //string branName = brnName.FirstOrDefault();
            //ViewBag.branchName= branName;


            CollectionViewModel model = new CollectionViewModel();

            model.Centers = GetCenter();
            model.MemberId = memb.Id;
            model.Image = memb.Image;
            model.ContactNo = memb.MbrContactNo;
            model.AlternativeContactNo = memb.MbrAlterContactNo;
            model.CenterId = memb.CenterId;
            model.BranchId = memb.BranchId;
            model.HeadquartersName ="Barisal";
            model.InstallmentSerialNumber = GetInatallmentSerial(model);
            model.CollectionDate = DateTime.Now;
            model.UserCollectionName = usNam;
            model.LoanAmount = memb.ProposeLoanAmount;
            model.InstallmentDeposit = GetInstlDeop(model.LoanAmount);

            model.ServiceAmount = (((model.LoanAmount)/100)*25);
            model.InstallmentAmount = ((model.LoanAmount+ model.ServiceAmount)/46);
            model.TotalInstallmentAmount =(model.InstallmentAmount + model.InstallmentDeposit);
            model.InstallmentDay = (DateTime.Now.Date.DayOfWeek).ToString();
            model.GetBalanced =  model.LastBalanced;
            model.UserCollectionName = usNam;



            //int? centerId = model.CenterId;
            //int? branchId = model.BranchId;
            //int? memberId = model.MemberId;
            //if (centerId.HasValue)
            //{

            //    if (branchId.HasValue)
            //    {
            //        //var centers = (from branch in _context.Branchs
            //        //               where branch.BranchId == branchId
            //        //               select branch).ToList();
            //        //foreach (var branch in branchs)
            //        //{
            //        //    model.Branchs.Add(new SelectListItem { Text = branch.BranchName, Value = branch.BranchId.ToString() });
            //        //}
            //        List<Branch> branchlist = new List<Branch>();
            //        Branch bran = new Branch();
            //         var branch = ( _context.Branchs.ToList());
            //        bran.BranchId= branch.Br
            //        branchlist.Insert(0, new Branch { BranchId = 0, BranchName = "--Select Center--" });

            //        model.Branchs.Add(branchlist);
            //    }
            //    if (memberId.HasValue)
            //    {
            //        var members = (from member in _context.Members
            //                       where member.MemberId == memberId
            //                       select member).ToList();
            //        foreach (var branch in members)
            //        {
            //            model.Members.Add(new SelectListItem { Text = branch.MemberName, Value = branch.MemberId.ToString() });
            //        }
            //    }

            //}



            return View(model);

        }

        private int GetInatallmentSerial(CollectionViewModel model)
        {
            var adCollectionId = _context.Collections.Select(a => a.MemberId).ToList();
          
            int serialNo = 0;
            string serial = null;

            int flag;

            if (adCollectionId.Contains(model.MemberId))
            {
                flag = 1;
                int sl1 = Convert.ToInt32(adCollectionId.Count);
                sl1++;
                serial = String.Format("{0:000}", sl1);

            }
            else
            {
                flag = 0;
                int sl1 = 1;
                serial = String.Format("{0:000}", sl1);
            }
            serialNo=Convert.ToInt32(serial);
            return serialNo;
        }



        [HttpPost]
        public IActionResult StoreCollection(CollectionViewModel model)
        {

            var usName= User.FindFirstValue(ClaimTypes.Name);
            var usId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            

            var collection = new Collection
            {
                CenterId = model.CenterId,
                BranchId = model.BranchId,
                CollectionDate = model.CollectionDate,
                AlternativeContactNo = model.AlternativeContactNo,
                MemberId = model.MemberId,
                MembRegNo = model.MembRegNo,
                HeadquartersName = "Barisal",
                ContactNo = model.ContactNo,

                BalancedAmount = model.LastBalanced,
                InstallmentAmount = model.InstallmentAmount,
                LoanAmount = model.LoanAmount,
                SecurityAmount = model.SecurityAmount,
                ServiceAmount = model.ServiceAmount,
                InstallmentDeposit = model.InstallmentDeposit,
                TotalInstallmentAmount = model.TotalInstallmentAmount,

                UserCollectionName = usName,
                UserId= usId
            };


            _context.Collections.Add(collection);
            _context.SaveChangesAsync();
            return RedirectToAction("Index", "Collection");
            //return View(model);

        }



        /// <summary>
        /// Have change this Code for active
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public IActionResult CreateInstallment()
        {
            ViewBag.Center = GetCenters();

            return View();

        }

        private double GetInstlDeop(double loanAmount)
        {
            double servDep = 0;
            double propsLoadn = loanAmount;

            if (loanAmount == 0)
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInstallment(CollectionViewModel model1, int id, List<IFormFile> Image)
        {
            ViewBag.borrowerId = id;

            foreach (var item in Image)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        model1.Image = stream.ToArray();
                    }
                }
                else
                {
                    ViewBag.errMasseges = "There are no Image now !";
                }
            }

            var members = _context.Members.Find(id);

            if (members != null)
            {
                var collectionAdd = new Collection
                {
                    UserId = model1.UserId,
                    MembRegNo = model1.MembRegNo,
                    MemberId = model1.MemberId,
                    AlternativeContactNo = model1.AlternativeContactNo,
                    InstallmentDay = model1.InstallmentDay,
                    InstallmentDeposit = model1.InstallmentDeposit,
                    InstallmentAmount = model1.InstallmentAmount,
                    InstallmentDate = model1.InstallmentDate,
                    InstallmentTimes = model1.InstallmentTimes,
                    LoanAmount = model1.LoanAmount,
                    CenterId = model1.CenterId,
                    BranchId = model1.BranchId,
                    SecurityAmount = model1.SecurityAmount,
                    ServiceAmount = model1.ServiceAmount,
                    TotalInstallmentAmount = model1.InstallmentDeposit + model1.InstallmentAmount,

                    ContactNo = model1.ContactNo,
                    CollectionDate = model1.CollectionDate,
                    UserCollectionName = model1.UserCollectionName,

                    HeadquartersName = model1.HeadquartersName,

                };

                var s = _context.Collections.Add(collectionAdd);
                if (s != null)
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("IndexApproved", "Members");
                }
                else
                {
                    ViewBag.errMasseges = "Approved Borrower Information is not Updated !!!";
                    return View(model1);
                }

            }
            else
            {
                ViewBag.errMasseges = "This member is not Approved !!!";
            }
            return View(model1);

        }


        [HttpGet]
        public async Task<IActionResult> Approved()
        {
            return View(await _context.Collections.ToListAsync());
        }

        [HttpGet]
        public IActionResult CollectionForm(int MemberId)
        {

            var member = _context.Collections.FirstOrDefault(x=>x.MemberId==MemberId);
  
            if (member == null)
            {
                return NotFound();
            }
            else
            {
                var model = new Collection
                {
                    UserCollectionName= User.Identity.Name,
                    Id= member.Id,
                    MemberId = member.MemberId,
                    CenterId= member.CenterId,
                    BranchId = member.BranchId,
                    UserId = member.UserId,
                    MembRegNo = member.MembRegNo,
                    ContactNo = member.ContactNo,
                    AlternativeContactNo = member.AlternativeContactNo,
                    InstallmentDay = member.InstallmentDay,
                    InstallmentDeposit = member.InstallmentDeposit,
                    InstallmentAmount = member.InstallmentAmount,
                    InstallmentDate = member.InstallmentDate,
                    InstallmentTimes = member.InstallmentTimes,
                    LoanAmount = member.LoanAmount,
                    SecurityAmount = member.SecurityAmount,
                    ServiceAmount = member.ServiceAmount,
                    TotalInstallmentAmount = member.InstallmentDeposit + member.InstallmentAmount,
                    CollectionDate = member.CollectionDate,
                    HeadquartersName = member.HeadquartersName,

                };
                return View(model);
            }


        }

        [HttpPost]
        public async Task<IActionResult> CollectionForm(Collection model1, int id)
        {
            ViewBag.borrowerId = id;


                var model = new Collection
                {
                    UserId = model1.UserId,
                    MembRegNo = model1.MembRegNo,
                    MemberId = model1.MemberId,
                    CenterId = model1.CenterId,
                    BranchId = model1.BranchId,
                    HeadquartersName = model1.HeadquartersName,
                    UserCollectionName = User.Identity.Name,
                    AlternativeContactNo = model1.AlternativeContactNo,
                    ContactNo = model1.ContactNo,
                    CollectionDate = model1.CollectionDate,
                    InstallmentDay = model1.InstallmentDay,
                    //InstallmentDeposit = model1.InstallmentDeposit,
                    //InstallmentAmount = model1.InstallmentAmount,
                    InstallmentDate = model1.InstallmentDate,
                    //InstallmentTimes = model1.InstallmentTimes,
                    //LoanAmount = model1.LoanAmount,
                    //SecurityAmount = model1.SecurityAmount,
                    //ServiceAmount = model1.ServiceAmount,
                    TotalInstallmentAmount = model1.InstallmentDeposit + model1.InstallmentAmount,


                };

                var s = _context.Collections.Add(model);
                if (s != null)
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Collection");
                }
                else
                {
                    ViewBag.errMasseges = "Collection Informations is not Updated !!!";
                    return View(model1);
                }

            //}
            //else
            //{
            //    return View(model1);
            //}

        }

        /// <summary>
        /// No need now  
        /// </summary>
        /// <returns></returns>
        public IActionResult CollectionFormIndex()
        {
            int centerId = ViewBag.centerId;
            int branceId = ViewBag.centerId;

            ViewBag.ApprovedCenter = GetCenter(centerId);
            ViewBag.ApprovedBrance = GetBranch(branceId);

            //ViewBag.ApprovedUserName = GetApprovedUserName();
            return View(_context.Collections.ToList());

        }

        //Get List<Center>
        private List<Center> GetCenter()
        {
            List<Center> centerslist = new List<Center>();
            centerslist = _context.Centers.ToList();
            centerslist.Insert(0, new Center { Id = 0, CenterName = "--Select Center--" });
            return centerslist;
        }

        //Get List<Branch>
        private List<Branch> GetBranch() 
        {
            List<Branch> dep = new List<Branch>();
            dep = _context.Branchs.ToList();
            dep.Insert(0, new Branch { Id = 0, BranchName = "--Select Brance--" });
            return dep;
        }

        //string GetCenter
        private string GetCenter(int centerId)
        {
            string bname = null;
            var q = from temp in _context.Centers where temp.Id == centerId select temp.CenterName;
            bname = q.FirstOrDefault();
            return bname;
        }
        //string GetBranch

        private string GetBranch(int branceId) 
        {
            string bname = null;
            var q = from temp in _context.Branchs where temp.Id == branceId select temp.BranchName;
            bname = q.FirstOrDefault();
            return bname;
        }



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
        public IEnumerable<Branch> GetBranchessByCenterId(int centerId)
        {

            return (_context.Branchs.ToList()
                 .Where(x => x.CenterId == centerId).ToList());
            
        }

        //Get List<Member>
        private List<Member> GetMember()
        {
            List<Member> dep = new List<Member>();
            dep = _context.Members.ToList();
            dep.Insert(0, new Member { Id = 0, MemberName = "--Select Member--" });
            return dep;
        }

        //Get IEnumerable<SelectListItem> GetMembers
        public IEnumerable<SelectListItem> GetMembers() 
        {

            List<SelectListItem> members = _context.Members.AsNoTracking()
                .OrderBy(n => n.MemberName)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.MemberName
                    }).ToList();
            var countrytip = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Member ---"
            };
            members.Insert(0, countrytip);
            return new SelectList(members, "Value", "Text");

        }


        //Get  JsonResult GetBranchesbyCenterId
        [HttpGet]
        public JsonResult  BranchesbyCenterId(int centerId)
        {
            //BranchesbyCenterId
            List<SelectListItem> branchs = _context.Branchs
                .OrderBy(n => n.CenterId)
                .Where(x=>x.CenterId== centerId)
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
            return Json(new SelectList(branchs, "Value", "Text"));


            //branches = _context.Branchs.Where(x => x.CenterId == centerId).ToList();
            //branches.Insert(0, new Branch { BranchId = 0, BranchName = "--Select a Branch--" });
            //return Json(new SelectList(branches, "BranchId", "BranchName"));

        }

        //Get  JsonResult GetMembersbyBranchId
        [HttpGet]
        public JsonResult MembersbyBranchId(int branchId) 
        {
            List<SelectListItem> members = _context.Members
              .OrderBy(n => n.BranchId)
              .Where(x => x.BranchId == branchId)
                  .Select(n =>
                  new SelectListItem
                  {
                      Value = n.Id.ToString(), 
                      Text = n.MemberName
                  }).ToList();
            var membertip = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Branch ---"
            };
            members.Insert(0, membertip);
            return Json(new SelectList(members, "Value", "Text")); 

        }


        //Get  JsonResult GetMembersbyMemberId
        //[HttpGet]
        public JsonResult MembersbyMemberId(int memberId)
        {

            //List<CollectionViewModel> memberslist = new List<CollectionViewModel>();

            CollectionViewModel  membersinfo = new CollectionViewModel();

                var s = _context.Members.Where(x => x.Id == memberId).FirstOrDefault();


                    membersinfo.MemberId = s.Id; 
                    membersinfo.MembRegNo = s.MemRegNo;
                    membersinfo.ContactNo = s.MbrContactNo;

                    membersinfo.InstallmentDate = DateTime.Now.Date.Date;
                    membersinfo.InstallmentDay = (DateTime.Now.Date.DayOfWeek).ToString();

                    membersinfo.InstallmentTimes =46;
                    membersinfo.LoanAmount = s.ProposeLoanAmount;
                    membersinfo.SecurityAmount = GetSecurity(memberId);
                    membersinfo.ServiceAmount = GetService(memberId);
                    membersinfo.InstallmentDeposit = GetInstallDeposit(memberId);
                    membersinfo.InstallmentSerialNumber = GetSerialNumber(memberId);
                    membersinfo.InstallmentAmount = GetInstallment(memberId);
                    membersinfo.TotalInstallmentAmount = (membersinfo.InstallmentDeposit+ membersinfo.InstallmentAmount);
                    membersinfo.GetBalanced = GetBalanced(memberId);

            //_context.Collections.Where(x => x.MemberId == memberId).FirstOrDefault()

            return Json(membersinfo);

       }

        private int GetSerialNumber(int memberId)
        {
            int SerialNo = 0;

            var dnameq = from temp in _context.Collections where temp.MemberId == memberId select temp.MembRegNo;
            var slNo = dnameq.FirstOrDefault();
            var adUserdbDepart = _context.Collections.Select(a => a.MembRegNo).ToList();


            string serial = null;

            int flag;

            if (adUserdbDepart.Contains(slNo))
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
            SerialNo = Convert.ToInt32(serial);

            return SerialNo;
        }

        private double GetSecurity(int memberId) 
        {
            var propoloan = (_context.Members.Where(x => x.Id == memberId).FirstOrDefault());
            double instLoan = propoloan.ProposeLoanAmount;


            double servCrg = 0;
            double approvedLoadn = instLoan;

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
        private double GetService(int memberId)
        {
            var propoloan = (_context.Members.Where(x => x.Id == memberId).FirstOrDefault());
            double instLoan = propoloan.ProposeLoanAmount; 


            double servCrg = 0;
            double approvedLoadn = instLoan;

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
        private double GetInstallDeposit(int memberId)
        {
            var propoloan = (_context.Members.Where(x => x.Id == memberId).FirstOrDefault());
            double instLoan = propoloan.ProposeLoanAmount; 
               
            double propsLoadn = instLoan;

            double servDep = 0;
           

            if (propsLoadn == 0)
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

        private double GetInstallment(int memberId)
        {

            var propoloan = ( _context.Members.Where(x=>x.Id == memberId).FirstOrDefault());
            double instLoan = propoloan.ProposeLoanAmount; 
            double instServ = ((instLoan/100)*25);

            double installm = ((instLoan + instServ) / 46);

            return installm;
        }

        private double GetBalanced(int memberId)
         {
            double servCrg = 0;
            double sTotalinstall = 0;
            double getBal = 0;
            //var allInstall = (from temp in _context.Collections where temp.MemberId == memberId select temp.InstallmentAmount);
            //double inst = allInstall.FirstOrDefault();
            //Collection collect = new Collection();

            var proLoan = (from temp in _context.Members where temp.Id == memberId select temp.ProposeLoanAmount).ToList();
            double proBal = proLoan.FirstOrDefault();

            var lastLoan = (from temp in _context.Collections where temp.Id == memberId select temp.LastBalanced).ToList();
            double lastBal = lastLoan.FirstOrDefault(); 

            var allin =  _context.Collections.Where(x=>x.MemberId == memberId).ToList();
           

            if (memberId == 0)
            {
                servCrg = 0;
            }
            else
            {

                if(lastBal==0)
                {
                    lastBal = proBal;
                }
                else
                {
                    foreach (var item in allin)
                    {
                        sTotalinstall = item.InstallmentAmount;
                        sTotalinstall++;
                    }

                }
                servCrg = (lastBal - sTotalinstall);
            }

            return servCrg;
        }

        public JsonResult GetBranchesbyCenterId(int centerId)
        {
            List<Branch> branches = new List<Branch>();
            branches = _context.Branchs.Where(x => x.CenterId == centerId).ToList();
            branches.Insert(0, new Branch { Id = 0, BranchName = "--Select a Branch--" });
            return Json(new SelectList(branches, "Id", "BranchName"));

        }

        //Get  JsonResult GetMembersbyCenterId
        public JsonResult GetMembersbyCenterId(int centerId)
        {
            List<Member> members = new List<Member>();
            members = _context.Members.Where(x => x.CenterId == centerId).ToList();
            members.Insert(0, new Member { Id = 0, MemberName = "--Select a Member--" });
            return Json(new SelectList(members, "Id", "MemberName"));

        }



        //Get  JsonResult GetMembersbyBranchId
        public JsonResult GetMembersbyBranchId(int branchId)
        {
            List<Member> members = new List<Member>();
            members = _context.Members.Where(x => x.BranchId == branchId).ToList();
            members.Insert(0, new Member { Id = 0, MemberName = "--Select a Member--" });
            return Json(new SelectList(members, "Id", "MemberName"));
        }


        //Get  JsonResult GetMembersbyMemberId
        public JsonResult GetMembersbyMemberId(int memberIdinfo)
        {
            List<CollectionViewModel> memberslist = new List<CollectionViewModel>();

            var mList = _context.Members.Where(x => x.Id == memberIdinfo).ToList();

            CollectionViewModel members = new CollectionViewModel();

            foreach (var item in mList)
            {
                members.AlternativeContactNo=item.MemRegNo;
                members.AlternativeContactNo = item.MbrAlterContactNo ;
                members.ContactNo = item.MbrContactNo ;
                members.LoanAmount = item.ProposeLoanAmount;
                members.SecurityAmount = GetSecurityAmount(members);
                members.ServiceAmount = GetServiceAmount(members);
                members.InstallmentAmount = GetInstallmentAmount(members);
                memberslist.Add(members);
            }

            return Json(memberslist);

        }


        private double GetBalacedAmount(CollectionViewModel members)
        {

            double installm = ((members.LoanAmount- members.LastBalanced));

            return installm;
        }

        private double GetInstallmentAmount(CollectionViewModel members)
        {

            double installm=((members.LoanAmount+ members.ServiceAmount) * 46);

            return installm;
        }


        /// <summary>
        /// Get json Actions
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        /// 

        [HttpGet]
        public IActionResult GetBranchbiCenterId(int centerId)
        {
            if (centerId != 0)
            {
                IEnumerable<SelectListItem> branches = GetBranches(centerId);
                return Json(branches);

            }
            return null;
        }

        private IEnumerable<SelectListItem> GetBranches(int centerId)
        {
            List<SelectListItem> branchs = _context.Branchs.AsNoTracking()
                .OrderBy(n => n.BranchName)
                .Where(n => n.CenterId== centerId)
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
        public IActionResult GetMembersbiCenterId(int centerId)
        {
            if (centerId != 0)
            {
                List<Member> members = new List<Member>();
                members = _context.Members.Where(p => p.CenterId == centerId).ToList();
                members.Insert(0, new Member { Id = 0, MemberName = "--Select a Branch--" });
                return Json(new SelectList(members, "Id", "MemberName"));
            }
            return null;
        }


        [HttpGet]
        public ActionResult GetMembersBranchId(int branchId)
        {
            if (branchId != 0)
            {
                IEnumerable<SelectListItem> members = GetMembers(branchId);
                return Json(members);
            }
            return null;
        }

        private IEnumerable<SelectListItem> GetMembers(int branchId)
        {
              
            List<SelectListItem> members = _context.Members.AsNoTracking()
                .OrderBy(n => n.MemberName)
                .Where(n => n.BranchId== branchId)
                .Select(n =>

                new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.MemberName
                }).ToList();
                var countrytip = new SelectListItem() 
                {
                    Value = null,
                    Text = "--- Select Branch ---"
                };
             members.Insert(0, countrytip);
            return new SelectList(members, "Value", "Text");
        }

        [HttpGet]
        public IActionResult GetMembersbibiMemberId(int memberId)
        {
            if (memberId != 0)
            {
                List<CollectionViewModel> memberslist = new List<CollectionViewModel>();

                var q = _context.Members.Where(x => x.Id == memberId);
                var r = _context.Members.Where(x => x.Id == memberId).Select(c => c.Image);

                byte[] imge = r.FirstOrDefault();

                CollectionViewModel members = new CollectionViewModel();

                members.AlternativeContactNo = q.Select(x => x.MbrAlterContactNo).ToString();
                members.ContactNo = q.Select(x => x.MbrContactNo).ToString();
                members.MembRegNo = q.Select(x => x.MemRegNo).ToString();
                members.Image = imge;
                members.LoanAmount = Convert.ToDouble(q.Select(x => x.ProposeLoanAmount));
                members.SecurityAmount = GetSecurityAmount(members);
                members.ServiceAmount = GetServiceAmount(members);
                members.InstallmentAmount = GetInstallmentAmount(members);
                members.GetBalanced = GetBalacedAmount(members);

                memberslist.Add(members);

                return Json(new SelectList(memberslist, "MemberId", "BranchName"));
            }
            return null;
        }


        private double GetSecurityAmount(CollectionViewModel members)
        {
            double servCrg = 0;
            double approvedLoadn = members.LoanAmount;

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

        private double GetServiceAmount(CollectionViewModel members)
        {
            double servCrg = 0;
            double approvedLoadn = members.LoanAmount;

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

        private double GetInstallmentDeposit(CollectionViewModel members)
        {

            double servDep = members.InstallmentDeposit;
            double propsLoadn = members.LoanAmount;

            if (propsLoadn == 0)
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




    }
}
