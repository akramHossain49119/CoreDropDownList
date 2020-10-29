using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoreDropDownList.Models
{
    public class Designations
    {
        [Key]
        public int DesigId { get; set; }

        public Designations()
        {

        }
        [Required(ErrorMessage = "Please Provide Designation !!!")]
        [Display(Name = "Name of Designation")]
        public string DesignationName { get; set; }

    }
}