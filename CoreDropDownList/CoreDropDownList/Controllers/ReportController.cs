using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalPaymentManagement.Data;
using DigitalPaymentManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using DigitalPaymentManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Claims;
using DigitalPaymentManagement.SecurityOptions;

namespace DigitalPaymentManagement.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ReportController(ApplicationDbContext context,
                                    RoleManager<IdentityRole> roleManager,
                                    SignInManager<AppUser> signInManager,
                                    UserManager<AppUser> userManager)
        {
            this._context = context;
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
 
        // GET: Report Daily Installment
        public IActionResult ViewDailyInstallment(DateTime startDate, string month = "", string year = "")
        {
            if (startDate == null && string.IsNullOrEmpty(month) && string.IsNullOrEmpty(year))
            {
                startDate = DateTime.Now.Date;
                month = DateTime.Now.ToString("MMMM");
                year = DateTime.Now.ToString("yyyy");
            }


            var collection = _context.Collections
                 .Where(x => x.CollectionDate.Equals(startDate) && x.CollectionDate.Month.Equals(month))
                 .Where(x => x.CollectionDate.Year.Equals(year))
                 .GroupBy(x => x.CollectionDate.Day)
                 .Select(a => new
                 {
                     Day = a.Key,
                     installmentAmount = a.Sum(r => r.InstallmentAmount),
                     installmentDeposit = a.Sum(r => r.InstallmentDeposit),
                     securityAmount = a.Sum(r => r.SecurityAmount),
                     serviceAmount = a.Sum(r => r.ServiceAmount),

                 });
            List<ReportDailyViewModel> data = collection.Select(x => new ReportDailyViewModel()
            {
                DayId = x.Day,
                InstallmentAmount = x.installmentAmount,
                InstallmentDeposit = x.installmentDeposit,
                SecurityAmount = x.securityAmount,
                ServiceAmount = x.serviceAmount,
            }).OrderBy(x => x.DayId).ToList();


            ViewBag.Month = startDate;
            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(data);
        }

        // GET: Report Daily Installment Members
        public IActionResult ViewDailyInstallmentMembers(DateTime startDate, string month = "", string year = "")
        {
            if (startDate == null && string.IsNullOrEmpty(month) && string.IsNullOrEmpty(year))
            {
                startDate = DateTime.Now.Date;
                month = DateTime.Now.ToString("MMMM");
                year = DateTime.Now.ToString("yyyy");
            }

            var collection = _context.Collections
                 .Where(x => x.CollectionDate.Equals(startDate) && x.CollectionDate.Month.Equals(month) && x.CollectionDate.Year.Equals(year))
                 .GroupBy(x => x.CollectionDate.Day).ToList();


            ViewBag.Month = startDate;
            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(collection);
        }



        // GET: Report Weekly Installment
        public IActionResult ViewWeeklyInstallment(DateTime startDate, DateTime endDate, string month = "", string year = "")
        {
            if (startDate == null && endDate == null && string.IsNullOrEmpty(month) && string.IsNullOrEmpty(year))
            {
                startDate = DateTime.Now.Date;
                endDate = DateTime.Now.Date;
                month = DateTime.Now.ToString("MMMM");
                year = DateTime.Now.ToString("yyyy");
            }

            //var q = (from temp in _context.Collections where (temp.CollectionDate == StartDate && temp.CollectionDate == EndDate && temp.CollectionDate.ToString("MMMM") == month && temp.CollectionDate.ToString("yyyy") == year) select temp);
            //List<Collection> collectList =(q.ToList());

            var collection = _context.Collections
                                 .Where(x => x.CollectionDate >= startDate && x.CollectionDate <= endDate)
                                 .Where(x => x.CollectionDate.Month.Equals(month) && x.CollectionDate.Year.Equals(year))
                                 .GroupBy(x => x.CollectionDate.Day)
                                 .Select(a => new
                                 {
                                     week = a.Key,
                                     installmentAmount = a.Sum(r => r.InstallmentAmount),
                                     installmentDeposit = a.Sum(r => r.InstallmentDeposit),
                                     securityAmount = a.Sum(r => r.SecurityAmount),
                                     serviceAmount = a.Sum(r => r.ServiceAmount),

                                 });
            List<ReportWeeklyViewModel> data = collection.Select(x => new ReportWeeklyViewModel()
                    {
                        WeekId = x.week,
                        InstallmentAmount = x.installmentAmount,
                        InstallmentDeposit = x.installmentDeposit,
                        SecurityAmount = x.securityAmount,
                        ServiceAmount = x.serviceAmount,
                    }).OrderBy(x => x.WeekId).ToList();


            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate; 
            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(data); 
        }

        // GET: Report Weekly Installment Members
        public IActionResult ViewWeeklyInstallmentMembers(DateTime startDate, DateTime endDate, string month = "", string year = "")
        {
            if (startDate==null && endDate == null && string.IsNullOrEmpty(month) && string.IsNullOrEmpty(year))
            {
                startDate = DateTime.Now.Date;
                endDate = DateTime.Now.Date;

                month = DateTime.Now.ToString("MMMM");
                year = DateTime.Now.ToString("yyyy");
            }

            var collection = _context.Collections
                                 .Where(x => x.CollectionDate >= startDate && x.CollectionDate <= endDate)
                                 .Where(x => x.CollectionDate.Month.Equals(month) && x.CollectionDate.Year.Equals(year))
                                 .GroupBy(x => x.CollectionDate.Day).ToList();


            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(collection);
        }



        // GET: Report Monthly Installment
        public IActionResult ViewMonthlyInstallment(string month = "", string year = "")
        {
            if (string.IsNullOrEmpty(month) && string.IsNullOrEmpty(year))
            {
                month = DateTime.Now.ToString("MMMM");
                year = DateTime.Now.ToString("yyyy");
            }

            var collection = _context.Collections
                            .Where(x => x.CollectionDate.Year.Equals(year))
                            .Where(x => x.CollectionDate.Month.Equals(month))
                            .GroupBy(x => x.CollectionDate.Month)
                            .Select(a => new
                            {
                                week = a.Key, 
                                installmentAmount = a.Sum(r => r.InstallmentAmount),
                                installmentDeposit = a.Sum(r => r.InstallmentDeposit),
                                securityAmount = a.Sum(r => r.SecurityAmount),
                                serviceAmount = a.Sum(r => r.ServiceAmount),

                            });



                        List<ReportMonthlyViewModel> data= collection.Select(x => new ReportMonthlyViewModel()
                        {
                            WeekId = x.week, 
                            InstallmentAmount = x.installmentAmount,
                            InstallmentDeposit = x.installmentDeposit,
                            SecurityAmount = x.securityAmount,
                            ServiceAmount = x.serviceAmount,
                        }).OrderBy(x => x.WeekId).ToList();



            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(data); 
        }

        // GET: Report Monthly Installment Members
        public IActionResult ViewMonthlyInstallmentMembers(string month = "", string year = "")
        {
            if (string.IsNullOrEmpty(month) && string.IsNullOrEmpty(year))
            {
                month = DateTime.Now.ToString("MMMM");
                year = DateTime.Now.ToString("yyyy");
            }

            var collection = _context.Collections
                            .Where(x => x.CollectionDate.Year.Equals(year))
                            .Where(x => x.CollectionDate.Month.Equals(month))
                            .GroupBy(x => x.CollectionDate.Month).ToList();



            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(collection);
        }




        // GET: Report Yearly Installment 
        public IActionResult ViewYearlyInstallment(string year) 
        {
            if (string.IsNullOrEmpty(year)) 
            {
                year = DateTime.Now.ToString("yyyy");
            }
            var collection = _context.Collections
                .Where(x => x.CollectionDate.Year.Equals(year))
                .GroupBy(x => x.CollectionDate.Month)
                .Select(a => new
                {
                    month = a.Key,
                    installmentAmount = a.Sum(r => r.InstallmentAmount),
                    installmentDeposit = a.Sum(r => r.InstallmentDeposit),
                    securityAmount = a.Sum(r => r.SecurityAmount),
                    serviceAmount = a.Sum(r => r.ServiceAmount),

                });


            List<ReportYearlyViewModel> data = collection.Select(x => new ReportYearlyViewModel()
            {
                MonthId = x.month,
                InstallmentAmount = x.installmentAmount,
                InstallmentDeposit = x.installmentDeposit,
                SecurityAmount = x.securityAmount,
                ServiceAmount = x.serviceAmount,
            }).OrderBy(x => x.MonthId).ToList();

            ViewBag.Year = year;

            return View(data);
        }

        // GET: Report Yearly Installment Members
        public IActionResult ViewYearlyInstallmentMembers(int? year)
        {
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            var collection = _context.Collections
                .Where(x => x.CollectionDate.Year.Equals(year))
                .GroupBy(x => x.CollectionDate.Month).ToList();

            ViewBag.Year = year;

            return View(collection);
        }



    }
}
