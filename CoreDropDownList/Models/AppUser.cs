using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser() : base()
        {
            IsRole = false;
        }
        [Display(Name = "User Name")]
        [DataType(DataType.Text)]
        public string UsrName { get; set; }

        [Display(Name = "Employee Registration No")]
        public string EmpRegNo { get; set; }

        [Display(Name = "Image")]
        public byte[] Image { get; set; }

        public string RoleAdd { get; set; }

        [Display(Name = "Date of Birth")]
        public string DOB { get; set; }

        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        [Display(Name = "Gender")]
        public string UserGender { get; set; }

        [Display(Name = "Branc's Name")]
        public int DepartmentDepId { get; set; } 
        public Department Department { get; set; }



        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        public string UserAddress { get; set; }

        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public bool IsRole { get; set; }



    }
}
