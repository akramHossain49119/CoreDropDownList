using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class ChangePasswordViewModel
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        [Column(TypeName = "nvarchar(500)")]
        public string UserName { get; set; }


        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }


        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }


        [Display(Name = "New Password")]
        [DataType(DataType.Password), Required(ErrorMessage = "New Password Required")]
        public string NewPassword { get; set; }


        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New Password  and Conferm Password do not match !!!")]
        public string ConfirmPassword { get; set; } 

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
