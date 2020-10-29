using CoreDropDownList.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class EditUserModel
    {

        public EditUserModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
            IsSelected = false;
        }

        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string EmpRegNo { get; set; }

        [Display(Name = "Email Address")]
        public string UserName { get; set; }
        [Display(Name = "User Name")]
        public string UsrName { get; set; }
        public byte[] Image { get; set; }
        public string Password { get; set; }
        public DateTime DOB { get; set; }
        public int DepartmentDepId { get; set; }
        public virtual Department Departments { get; set; }

        public string DepCode { get; set; }
        public string DepDepartment { get; set; }


        public string RoleAdd { get; set; }

        public DateTime JoiningDate { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string UserGender { get; set; }
        public string UserAddress { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
        public bool IsSelected { get; set; }
    }
}
