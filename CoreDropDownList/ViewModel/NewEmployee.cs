using CoreDropDownList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class NewEmployee
    {

        public NewEmployee()
        {
            IsSelected = false;
        }
        public string EmpRegNo { get; set; }
        public string UName { get; set; }
        public string ShortName { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public string Password { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string RoleAdd { get; set; }
        public int DepartmentDepId { get; set; }
        public virtual Department Departments { get; set; }


        public int CenterId { get; set; }
        public virtual Center Center { get; set; }

        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }

        public string DepCode { get; set; }
        public string DepDepartment { get; set; }

        public string Campus { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }

        public string Description { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool IsSelected { get; set; }

    }
}
