using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class ApprovedBorrowerViewModel
    {

        public ApprovedBorrowerViewModel()
        {
            IsDisbursement = false;
            IsApproved = false;
        }

        public string BwrRegNo { get; set; }
        public string HeadquartersName { get; set; }
        public int CenterId { get; set; }
        public int BranchId { get; set; }
        public int MemberId { get; set; } 
        public DateTime CreationDate { get; set; }
        public string BorrowerName { get; set; }
        public byte[] Image { get; set; }
        public string BwrNID { get; set; }
        public string BwrProfession { get; set; }
        public string BwrNominee { get; set; }
        public string BwrNomineeAddress { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string AlternativeContactNo { get; set; }
        public string BwrPresentAddress { get; set; }
        public double InstallmentDeposit { get; set; }
        public bool IsDisbursement { get; set; }
        public bool IsApproved { get; set; }
        public double LoanAmount { get; set; }
        public double ServiceAmount { get; set; }
        public double GetServiceAmount
        {
            get

            {
                return ((LoanAmount / 100) * 25);
            }
        }
        public double SecurityAmount { get; set; }
        public double GetSecurityAmount
        {
            get

            {
                return ((LoanAmount / 100) * 10);
            }
        }
        public double InstallmentAmount { get; set; }
        public double GetInstallmentAmount
        {
            get

            {
                return (Convert.ToInt32((LoanAmount + GetServiceAmount) / InstallmentTimes));
            }
        }
        public int InstallmentTimes { get; set; }
        public string InstallmentDay { get; set; }
        public DateTime InstallmentDate { get; set; } 

    }
}
