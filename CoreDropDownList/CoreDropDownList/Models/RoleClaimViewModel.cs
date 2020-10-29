using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public class RoleClaimViewModel : ClaimsIdentity
    {
        public RoleClaimViewModel()
        {
            UserClaim = new List<UserClaims>();
            RoleClaim = new List<RoleClaims>();
            IsSelected = false;
        }
        public string UserId { get; set; }
        public List<UserClaims> UserClaim { get; set; }

        public string RoleId { get; set; }
        public List<RoleClaims> RoleClaim { get; set; }

        public bool IsSelected { get; set; }
    }
}
