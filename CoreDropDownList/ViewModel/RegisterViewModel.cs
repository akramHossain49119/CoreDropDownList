using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CoreDropDownList.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            Users = new List<string>();
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }
        //RegistrationNo
        public string RegistrationNo { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        [Column(TypeName = "nvarchar(500)")]
        [Remote(action: "UsersExists", controller: "Register")]
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "User Picture")]
        public byte[] Image { get; set; }
        //public IFormFile Image { get; set; }
        /// <summary>
        /// /
        /// </summary>
        [Required(ErrorMessage = "Please provide Date of Birth")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Display(Name = "Joining Date")]
        [Required(ErrorMessage = "Please provide Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }

        [Display(Name = "Department Name")]
        [Required(ErrorMessage = "Please provide Teachers Faculty")]
        public int DepartmentDepId { get; set; }
        public virtual Department Departments { get; set; }


        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
         ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Column(TypeName = "nvarchar(500)")]
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public string Address { get; set; }

        public List<string> Users { get; internal set; }
        public List<string> Claims { get; internal set; }
        public IList<string> Roles { get; internal set; }
    }
}
