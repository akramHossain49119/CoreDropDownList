using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreDropDownList.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.ViewModel
{
    public class NewUser
    {

        public NewUser()
        {
            IsSelected = false;
        }
        [Key]
        public int Id { get; set; }
        public string AdmtId { get; set; }
        public string EmpRegNo { get; set; }
        public string UName { get; set; }
        public string ShortName { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public string RoleAdd { get; set; }
        public string YearofStu { get; set; }
        public string Password { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public int UserId { get; set; }

        public virtual AppUser User { get; set; }
        public int DepartmentsDepId { get; set; }
        public virtual Department Departments { get; set; }

        public string DepCode { get; set; }

        public string Campus { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }

        public string Description { get; set; }
        public string AdmitDate { get; set; }

        public bool IsSelected { get; set; }


    }
}
