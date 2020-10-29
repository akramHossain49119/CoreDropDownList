using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class EditUserInRole
    {
        public EditUserInRole()
        {
            Claims = new List<string>();
            IsSelected = false;
        }

        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public IList<string> Claims { get; set; }
        public bool IsSelected { get; set; }
    }
}
