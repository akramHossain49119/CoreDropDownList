using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class UserRoleViewModel
    {
        public UserRoleViewModel()
        {

            Users = new List<string>();
            IsSelected = false;
        }

        [Key]
        public int URId { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
        public bool IsSelected { get; set; }



    }
}
