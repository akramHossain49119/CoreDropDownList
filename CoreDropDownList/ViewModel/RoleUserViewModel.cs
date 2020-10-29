using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class RoleUserViewModel
    {
        public RoleUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
            IsSelected = false;
        }
        [Key]
        public int RUId { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
        public bool IsSelected { get; set; }

    }
}
