using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CoreDropDownList.Models
{
    public class Center
    {
        [Key] 
        public int Id { get; set; } 

        public Center()
        {

        }


        [Display(Name = "Center Name")]
        [Required(ErrorMessage = "Please Provide Center Name !!!")]
        [Remote(action: "CenterNameExists", controller: "ProposeBorrowers")]
        public string CenterName { get; set; }


        [Display(Name = "Center Code")]
        [Required(ErrorMessage = "Please Provide Center Code !!!")]
        [Remote(action: "CenterCode", controller: "LoanBorrower")]
        public string CenterCode { get; set; }

        //public List<Member> Members { get; set; }
        //public List<Collection> Collections { get; set; }
        //public List<Approved> Approveds { get; set; }

    }
}
