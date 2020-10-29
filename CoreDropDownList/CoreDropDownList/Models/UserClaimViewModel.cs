using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreDropDownList.ViewModel;


namespace CoreDropDownList.Models
{
    public class UserClaimViewModel
    {
        public UserClaimViewModel()
        {
            Claim = new List<UserClaims>();
            RoleClaim = new List<RoleClaims>();
            IsSelected = false;
        }
        public string UserId { get; set; }
        public List<UserClaims> Claim { get; set; }
        public string RoleId { get; set; }
        public List<RoleClaims> RoleClaim { get; set; }

        public bool IsSelected { get; set; }
    }
}
