using CoreDropDownList.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.SecurityOptions
{
    public class CascadingModel
    {
        public CascadingModel()
        {
            this.Members = new List<SelectListItem>();
            this.Centers = new List<SelectListItem>();
            this.Branchs = new List<SelectListItem>();
            //this.Memberss = new List<Member>();
            //this.Centerss = new List<Center>();
            //this.Branchss = new List<Branch>();
        }

        [Key]
        public int Id { get; set; }

        public string MemRegNo { get; set; }
        public string HeadquartersName { get; set; }
        public DateTime CreationDate { get; set; }
        public string MemberName { get; set; }
        public byte[] Image { get; set; }
        public string MbrNID { get; set; }
        public string MbrProfession { get; set; }
        public string MbrNominee { get; set; }
        public string MbrNomineeAddress { get; set; }
        public string MbrFName { get; set; }
        public string MbrMName { get; set; }
        public string MbrEmail { get; set; }
        public string MbrGender { get; set; }
        public string MbrContactNo { get; set; }
        public string MbrAlterContactNo { get; set; }
        public string MbrPresentAddress { get; set; }
        public string UserId { get; set; }
        public string MemberCreatorName { get; set; }
        public double ProposeLoanAmount { get; set; }
        public List<SelectListItem> Members { get; set; }
        public List<SelectListItem> Centers { get; set; }
        public List<SelectListItem> Branchs { get; set; }

        public List<Member> Memberss { get; set; }
        public List<Center> Centerss { get; set; }
        public List<Branch> Branchss { get; set; }

        public int MemberId { get; set; }
        public int CenterId { get; set; }
        public int BranchId { get; set; } 

    }
}
