using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public class Member: InfoAllMember
    {
        public Member()
        {
            IsDisbursement = false;
            IsApproved = false;
            CreationDate = DateTime.Now.Date;

            //this.Centers = new List<string>();
            //this.Branchs = new List<string>();
        }
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }  
 
        public string CenNames { get; set; }

        [Column(Order = 2)]
        public string MemCreatorName { get; set; }

        //[NotMapped]
        //public List<string> Centers { get; set; }
        //[NotMapped]
        //public List<string> Branchs { get; set; }

        [Column(Order = 3)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        //[Required]
        public int BranchId { get; set; } 
        public Branch Branch { get; set; }

        public int CenterId { get; set; }
        public Center Center { get; set; } 

        public double ProposeLoanAmount { get; set; }
        public bool IsDisbursement { get; set; }
        public bool IsApproved { get; set; }


    }
}
