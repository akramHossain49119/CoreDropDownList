using CoreDropDownList.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class ReportYearlyViewModel
    {
  
        public int MonthId { get; set; }
        public int DYearId { get; set; } 
        public string UserCollectionName { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserName { get; set; }
        public string HeadquartersName { get; set; }
        public int MemberId { get; set; }
        public int CenterId { get; set; }
        public int BranchId { get; set; }
        public string MembRegNo { get; set; }
        public string MbrNID { get; set; }
        public string MbrNominee { get; set; }
        public string MemberName { get; set; }
        public string ContactNo { get; set; }
        public string AlternativeContactNo { get; set; }
        public int InstallmentSerialNumber { get; set; }
        public double LoanAmount { get; set; }
        public double ServiceAmount { get; set; }
        public double SecurityAmount { get; set; }
        public int InstallmentTimes { get; set; }
        public double InstallmentAmount { get; set; }
        public double InstallmentDeposit { get; set; }
        public double TotalInstallmentAmount { get; set; }

    }
}
