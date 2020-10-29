using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Create","Create"),
            new Claim("Index","Index"),
            new Claim("Edit","Edit"),
            new Claim("Delete","Delete"),
            new Claim("ListUsers","ListUsers"),
            new Claim("ListRoles","ListRoles"),
            new Claim("EditUser","EditUser"),
            new Claim("EditUserInRole","EditUserInRole"),
            new Claim("EditRoleInUser","EditRoleInUser"),
            new Claim("ManageUserClaim","ManageUserClaim"),
            new Claim("ManageRoleClaim","ManageRoleClaim")
        };
    }
}
