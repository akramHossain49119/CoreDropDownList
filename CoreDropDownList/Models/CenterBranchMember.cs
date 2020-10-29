using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public class CenterBranchMember
    {
        public CenterBranchMember()
        {
            IsDisbursement = false;
            IsApproved = false;
  
        }
        [Key]
        public int CBMId { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int BranchBranchId { get; set; }
        public Branch Branch { get; set; }
        public int CenterCenterId { get; set; }
        public Center Center { get; set; }

        public int MemberMemberId { get; set; }
        public Member Member { get; set; }

        public double LoanAmount { get; set; }
        public double ServiceAmount { get; set; }
        public double GetServiceAmount
        {
            get
            {
                return (((LoanAmount / 100) * 25));
            }
        }
        public double SecurityAmount { get; set; }
        public int GetSecurityAmount
        {
            get
            {
                return (Convert.ToInt32((LoanAmount / 100) * 10));
            }
        }
        public int InstallmentTimes { get; set; }
        public double InstallmentAmount { get; set; }
        public int GetInstallmentAmount
        {
            get
            {
                return (Convert.ToInt32((LoanAmount + GetServiceAmount) / InstallmentTimes));
            }
        }
        public double InstallmentDeposit { get; set; }
        public string InstallmentDay { get; set; }
        public DateTime InstallmentDate { get; set; }
        public double TotalInstallmentAmount { get; set; }

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
        public double BalancedAmount { get; set; }
        public double LastBalanced
        {
            get
            {
                return (LoanAmount - GetPaidTotalInstallment);
            }
        }

        public double ProposeLoanAmount { get; set; }
        public bool IsDisbursement { get; set; }
        public bool IsApproved { get; set; }
    }
}
