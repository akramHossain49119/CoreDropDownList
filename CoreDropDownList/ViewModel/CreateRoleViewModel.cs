using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class CreateRoleViewModel
    {
        [Key]
        public int URId { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
