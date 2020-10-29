using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.SecurityOptions
{
    public class ChangePasswordModelView
    {


        [Display(Name = "Current Password")]
        [DataType(DataType.Password), Required(ErrorMessage = "Old Password Required")]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password), Required(ErrorMessage = "Old Password Required")]
        public string NewPassword { get; set; }


        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password), Required(ErrorMessage = "New Password Required")]
        public string ConfirmPassword { get; set; }
    }
}
