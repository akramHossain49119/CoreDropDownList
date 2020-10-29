using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public class Branch
    {
        public Branch()
        {

        }

        [Key]
        public int Id { get; set; }



        [Remote(action: "BranceNameExists", controller: "ProposeBorrowers")]
        [Required(ErrorMessage = "Please Provide Center Name !!!")]
        [Display(Name = "Center Name")]
        public string BranchName { get; set; }


        [Display(Name = "Center Code")]
        [Required(ErrorMessage = "Please Provide Center Code !!!")]
        [Remote(action: "CenterCode", controller: "LoanBorrower")]
        public string BranchCode { get; set; }


        //[Required]
         public int CenterId { get; set; }
        //public Center Center { get; set; }

        //public List<Member> Members { get; set; }
        //public List<Collection> Collections { get; set; }
        //public List<Approved> Approveds { get; set; }

    }
}
