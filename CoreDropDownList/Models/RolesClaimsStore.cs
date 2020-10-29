using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public static class RolesClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("SuperAccountant","SuperAccountant"),
            new Claim("SrAccountant","SrAccountant"),
            new Claim("JrAccountant","JrAccountant"),
            new Claim("SrExamAccountant","SrExamAccountant"),
            new Claim("BengaliAccountant","BengaliAccountant"),
            new Claim("MathAccountant","MathAccountant"),
            new Claim("EnglishAccountant","EnglishAccountant"),
            new Claim("ScienceAccountant","ScienceAccountant"),
            new Claim("CommerceAccountant","CommerceAccountant"),
            new Claim("ArtsAccountant","ArtsAccountant"),
            new Claim("SocialAccountant","SocialAccountant"),
            new Claim("PEAccountant","PEAccountant")
        };
    }
}
