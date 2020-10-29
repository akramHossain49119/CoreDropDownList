using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class EditRoleInUser
    {
        public EditRoleInUser()
        {
            Claims = new List<string>();
            IsSelected = false;
        }

        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }

        public IList<string> Claims { get; set; }
        public bool IsSelected { get; set; }
    }
}
