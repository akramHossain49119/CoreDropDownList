using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDropDownList.Models
{
    public class LoanAccount
    {

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
                return (LoanAmount - GetPaidTotalInstallment);
            }
        }

        [Column(TypeName = "bit")]
        public bool IsDisbursement { get; set; }

        [Column(TypeName = "bit")]
        public bool IsApproved { get; set; }
    }
}
