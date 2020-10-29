using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public class RoleClaims
    {
        public RoleClaims()
        {
            IsSelected = false;
        }
        public string RoleClaimTypes { get; set; }
        public bool IsSelected { get; set; }
    }
}
