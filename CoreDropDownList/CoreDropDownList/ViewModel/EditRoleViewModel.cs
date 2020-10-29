using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
            Claims = new List<string>();
            IsSelected = false;
        }

        public string RoleId { get; set; }
        public string UserId { get; set; }

        public string RoleName { get; set; }



        public List<string> Users { get; set; }

        public IList<string> Claims { get; set; }

        public bool IsSelected { get; set; }
    }
}
