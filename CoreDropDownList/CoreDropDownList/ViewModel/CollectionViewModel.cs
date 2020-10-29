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
    public class CollectionViewModel
    {
        public CollectionViewModel()
        {
            this.Members = new List<Member>();
            this.Centers = new List<Center>();
            this.Branchs = new List<Branch>();

            //this.Memberss = new List<SelectListItem>();
            //this.Centerss = new List<SelectListItem>();
            //this.Branchss = new List<SelectListItem>();

            this.Centersh = new List<string>();
            this.Branchsh = new List<string>();
        }

        [NotMapped]
        public List<string> Centersh { get; set; }
        [NotMapped]
        public List<string> Branchsh { get; set; }

        //public List<SelectListItem> Memberss { get; set; }
        //public List<SelectListItem> Centerss { get; set; }
        //public List<SelectListItem> Branchss { get; set; }
        //public IEnumerable<Collection> Collections { get; set; }
        //public IEnumerable<SelectListItem> Centerss { get; set; }
        //public IEnumerable<SelectListItem> Branchss { get; set; }
        //public IEnumerable<SelectListItem> Memberss { get; set; }

        public List<Member> Members { get; set; }
        public List<Center> Centers { get; set; }
        public List<Branch> Branchs { get; set; }


        public int Id { get; set; }
        public string UserCollectionName { get; set; }
        public DateTime CollectionDate { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public string HeadquartersName { get; set; }
        public int MemberId { get; set; }
        public int CenterId { get; set; }
        public int BranchId { get; set; }
        public string MembRegNo { get; set; }
        public string MbrNID { get; set; }
        //MbrNominee
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
 
        public string InstallmentDay { get; set; }
        public DateTime InstallmentDate { get; set; }
        public double GetBalanced { get; set; }
        public double GetTotalInstallmentAmount
        {
            get
            {
                return (InstallmentAmount+ InstallmentDeposit);
            }

        }

        public double GetTotalInstallment 
        {
            get
            {
                return InstallmentAmount++;
            }
        }
        public double LastBalanced
        {
            get
            {
                return (LoanAmount- GetTotalInstallment);
            }
        }
        public byte[] Image { get; set; }
        public bool IsDisbursement { get; set; }
        public bool IsApproved { get; set; }



    }
}
