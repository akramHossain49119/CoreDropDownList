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
    public class ProposeBorrowerViewModel
    {


        [Display(Name = "Customer Number")]
        public int Id { get; set; }

        [NotMapped]
        public List<Center> Centers { get; set; }
        [NotMapped]
        public List<Branch> Branchs { get; set; }


        [Display(Name = "Branch")]
        public int BranchCbrnId { get; set; }
        public IEnumerable<SelectListItem> Branchss { get; set; }


    
        [Display(Name = "Center")]
        public int CenterCntId { get; set; }
        public IEnumerable<SelectListItem> Centerss { get; set; } 

 
        public string UserId { get; set; }
        public AppUser User { get; set; }


        [Display(Name = "Employee Registration No")]
        public string BwrRegNo { get; set; }


        [Display(Name = "Headquarter's Name")]

        public string HeadquartersName { get; set; }

        public string CenterName { get; set; }

        public string BranchName { get; set; }

        [Display(Name = "Creation Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }


        [Display(Name = "Name")]
        public string BorrowerName { get; set; }


        [Display(Name = "Image")]
        public byte[] Image { get; set; }


        [Display(Name = "Borrower NID")]
        public string BwrNID { get; set; }

        [Display(Name = "Profession")]
        public string BwrProfession { get; set; }

        [Display(Name = "Nominee's Name")]
        public string BwrNominee { get; set; }

        [Display(Name = "Nominee's Address")]
        public string BwrNomineeAddress { get; set; }

        [Display(Name = "Fathers' Name")]
        public string FName { get; set; }


        [Display(Name = "Mothers Name")]
        public string MName { get; set; }


        [Display(Name = "Email Address")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Please provide valid email")]
        public string Email { get; set; }


        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }


        [Display(Name = "Alternative Contact No")]
        public string AlternativeContactNo { get; set; }


        [Display(Name = "Borrower's present address")]
        [DataType(DataType.MultilineText)]
        public string BwrPresentAddress { get; set; }

        [Display(Name = "Loan Amount")]
        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double LoanAmount { get; set; }


        [Display(Name = "Service Charge")]
        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double ServiceAmount { get; set; }
        public double GetServiceAmount
        {
            get
            {
                return (((LoanAmount / 100) * 25));
            }
        }

        [Display(Name = "Admission Charge")]
        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double SecurityAmount { get; set; }

        public double GetSecurityAmount
        {
            get
            {
                return (Convert.ToInt32((LoanAmount / 100) * 10));
            }
        }

        [Display(Name = "Installment Amount")]
        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double InstallmentAmount { get; set; }

        public double GetInstallmentAmount
        {
            get
            {
                return ((LoanAmount + GetServiceAmount) /(InstallmentTimes));
            }
        }

        [Display(Name = "Installment Times")]

        public int InstallmentTimes { get; set; }


        [Display(Name = "Installment Amount")]
        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double InstallmentDeposit { get; set; }


        [Display(Name = "Installment Day")]
        public string InstallmentDay { get; set; }


        [Display(Name = "Installment Date")]

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InstallmentDate { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double GetInstallmentDeposit { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double TotalInstallmentAmount { get; set; }

        public double GetTotalInstallmentAmount
        {
            get
            {
                return (GetInstallmentAmount + GetInstallmentDeposit);
            }
        }

        [Column(TypeName = "bit")]
        public bool IsDisbursement { get; set; }

        [Column(TypeName = "bit")]
        public bool IsApproved { get; set; }
    }
}
