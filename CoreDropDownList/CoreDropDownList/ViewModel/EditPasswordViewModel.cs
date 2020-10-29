using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class EditPasswordViewModel
    {

        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }


        [Display(Name = "New Password")]
        [DataType(DataType.Password), Required(ErrorMessage = "New Password Required")]
        public string NewPassword { get; set; }


        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New Password  and Conferm Password do not match !!!")]
        public string ConfirmPassword { get; set; }
    }
}
