using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CoreDropDownList.Models
{
    public class Department
    {
        //: IComparable
        [Key]
        public int DepId { get; set; }

        public Department()
        {

        }

        //[Remote(action: "DepartmentExists", controller: "Department")]
        [Required(ErrorMessage = "Please Provide Department Name !!!")]
        [Display(Name = "Department Name")]
        public string DepName { get; set; }


        [Required(ErrorMessage = "Please Provide Department Code !!!")]
        [Display(Name = "Code")]
        public string DepCode { get; set; }

        [Required(ErrorMessage = "Please Provide Department Floor !!!")]
        [Display(Name = "Department Floor")]
        public string DepFloor { get; set; }



    }
}