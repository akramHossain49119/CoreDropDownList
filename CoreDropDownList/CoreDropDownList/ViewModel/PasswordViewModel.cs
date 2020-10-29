using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class PasswordViewModel
    {

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }


        [Display(Name = "Old Password")]
        [DataType(DataType.Password), Required(ErrorMessage = "Old Password Required")]
        public string CurrentPassword { get; set; }


        [Display(Name = "New Password")]
        [DataType(DataType.Password), Required(ErrorMessage = "New Password Required")]
        public string NewPassword { get; set; }


        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New Password  and Conferm Password do not match !!!")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Remember Me")]
        public string RememberMe { get; set; }

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
