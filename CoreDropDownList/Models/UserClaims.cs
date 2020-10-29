using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public class UserClaims
    {
        public UserClaims()
        {
            IsSelected = false;
        }
        public string ClaimTypes { get; set; }
        public bool IsSelected { get; set; }
    }
}
