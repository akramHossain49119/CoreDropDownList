using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public class Collection 
    {
        public Collection()
        {

            //this.Centerss = new List<string>();
            //this.Branchss = new List<string>();

        }

        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CollectionDate { get; set; }
        public string HeadquartersName { get; set; }

        [Required]
        public string UserCollectionName { get; set; }


        [Required]
        [ForeignKey("Center")]
        public int CenterId { get; set; }

        [Required]
        [ForeignKey("Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Employee Registration No")]
        public string MembRegNo { get; set; }
        [Required]
        [ForeignKey("Member")]
        public int MemberId { get; set; }

        [Display(Name = "Contact No")]
        public string  ContactNo { get; set; } 

        [Display(Name = "Alternative Contact No")]
        public string AlternativeContactNo { get; set; }

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
                return ((LoanAmount / 100) * 25);
            }
        }

        [Display(Name = "Security Charge")]
        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double SecurityAmount { get; set; }

        public int GetSecurityAmount
        {
            get
            {
                return (Convert.ToInt32((LoanAmount / 100) * 10));
            }
        }

        [Display(Name = "Installment Times")]
        public int InstallmentTimes { get; set; }

        [Display(Name = "Installment Amount")]
        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double InstallmentAmount { get; set; }

        public int GetInstallmentAmount
        {
            get
            {
                return (Convert.ToInt32((LoanAmount + GetServiceAmount) / InstallmentTimes));
            }
        }

        [Display(Name = "Installment Amount")]
        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double InstallmentDeposit { get; set; }

        [Display(Name = "Installment Day")]
        public string InstallmentDay { get; set; }


        [Display(Name = "Installment Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InstallmentDate { get; set; }


        [DataType(DataType.Currency, ErrorMessage = "Amount must be numeric")]
        public double TotalInstallmentAmount { get; set; }
        public int InstallmentSerialNumber { get; set; }
        public double BalancedAmount { get; set; }
        public double GetTotalInstallmentAmount
        {
            get
            {
                return (GetInstallmentAmount + InstallmentDeposit);
            }
        }

        public double GetPaidTotalInstallment
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
                return (BalancedAmount - InstallmentAmount);
            }
        }


    }
}
