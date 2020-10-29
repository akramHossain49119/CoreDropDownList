using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public class InfoAllMember
    {


        [Display(Name = "Employee Registration No")]
        public string MemRegNo { get; set; }


        [Display(Name = "Headquarter's Name")]

        public string HeadquartersName { get; set; }


        [Display(Name = "Creation Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }


        [Display(Name = "Name")]
        public string MemberName { get; set; }


        [Display(Name = "Image")]
        public byte[] Image { get; set; }


        [Display(Name = "Borrower NID")]
        public string MbrNID { get; set; } 

        [Display(Name = "Profession")]
        public string MbrProfession { get; set; }

        [Display(Name = "Nominee's Name")]
        public string MbrNominee { get; set; }

        [Display(Name = "Nominee's Address")]
        public string MbrNomineeAddress { get; set; }

        [Display(Name = "Fathers' Name")]
        public string MbrFName { get; set; }


        [Display(Name = "Mothers Name")]
        public string MbrMName { get; set; }


        [Display(Name = "Email Address")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Please provide valid email")]
        public string MbrEmail { get; set; }


        [Display(Name = "Gender")]
        public string MbrGender { get; set; }

        [Display(Name = "Contact No")]
        public string MbrContactNo { get; set; }


        [Display(Name = "Alternative Contact No")]
        public string MbrAlterContactNo { get; set; }


        [Display(Name = "Borrower's present address")]
        [DataType(DataType.MultilineText)]
        public string MbrPresentAddress { get; set; }
    }
}
