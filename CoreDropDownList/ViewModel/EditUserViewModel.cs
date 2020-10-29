using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class EditUserViewModel
    {

        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
            IsSelected = false;
        }

        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        public string UserGender { get; set; }
        public string UserAddress { get; set; }

        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }

        public bool IsSelected { get; set; }
    }
}
