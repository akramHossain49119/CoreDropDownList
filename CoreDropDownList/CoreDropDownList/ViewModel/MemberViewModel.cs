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
    public class MemberViewModel
    {
        public MemberViewModel()
        {
             
        }


        [Key]
        public int Id { get; set; }
        public IEnumerable<SelectListItem> Centersh { get; set; }
        public IEnumerable<SelectListItem> Branchsh { get; set; } 

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
        public int CenterId { get; set; }
        public int BranchId { get; set; }
        public int MemberId { get; set; }
        public string UserId { get; set; }
        public string MemberCreatorName { get; set; }
        public double ProposeLoanAmount { get; set; }

        public List<Member> Members { get; set; }
        public List<Center> Centers { get; set; }
        public List<Branch> Branchs { get; set; }

        public List<SelectListItem> Memberss { get; set; }
        public List<SelectListItem> Centerss { get; set; }
        public List<SelectListItem> Branchss { get; set; }


        //public IEnumerable<Member> Members { get; set; }
        //public IEnumerable<Center> Centers { get; set; }
        //public IEnumerable<SelectListItem> Centers { get; set; }

        //public IEnumerable<Branch> Branchs { get; set; }
        //public IEnumerable<SelectListItem> Branchs { get; set; }  

    }
}
