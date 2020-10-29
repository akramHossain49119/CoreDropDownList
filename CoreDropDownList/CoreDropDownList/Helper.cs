using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalPaymentManagement
{
    public static class Helper
    {
 
        public static List<SelectListItem> GetDepartments()
        {
            List<SelectListItem> departments = new List<SelectListItem>()
            {
                new SelectListItem("Select Department", "0"),
                new SelectListItem("Managing Director General", "01"),
                new SelectListItem("Administrtor", "02"),
                new SelectListItem("Accounce", "03"),
                new SelectListItem("Executive", "04"),
                new SelectListItem("STUFF", "05")

            };
            return departments;
        }
        public static List<SelectListItem> GetDepartmentsCode()
        {
            List<SelectListItem> departments = new List<SelectListItem>()
            {
                new SelectListItem("Select Department Code", "0"),
                new SelectListItem("MD", "1"),
                new SelectListItem("ADMIN", "2"),
                new SelectListItem("ACCOU", "3"),
                new SelectListItem("EXECU", "4"),
                new SelectListItem("STUFF", "5")

            };
            return departments;
        }
        public static List<SelectListItem> GetRoles()
        {
            List<SelectListItem> roles = new List<SelectListItem>()
            {
                new SelectListItem("Select Role", "Select Role"), 
                new SelectListItem("SuperAccountant", "SuperAccountant"),
                new SelectListItem("SrAccountant", "SrAccountant"),
                new SelectListItem("UrAccountant", "UrAccountant")
            };
            return roles;
        }
        public static List<SelectListItem> GetMonths()
        {
            List<SelectListItem> months = new List<SelectListItem>()
            {
                new SelectListItem("Select Month ... ", "0"),
                new SelectListItem("January", "01"),
                new SelectListItem("February", "02"),
                new SelectListItem("March", "03"),
                new SelectListItem("April", "04"),
                new SelectListItem("May", "05"),
                new SelectListItem("June", "06"),
                new SelectListItem("July", "07"),
                new SelectListItem("August", "08"),
                new SelectListItem("September", "09"),
                new SelectListItem("October", "10"),
                new SelectListItem("November", "11"),
                new SelectListItem("December", "12")
            };
            return months;
        }

        public static List<SelectListItem> GetGenders()
        {
            List<SelectListItem> genders = new List<SelectListItem>()
            {
                new SelectListItem("Select Gender ... ", "Select Gender"),
                new SelectListItem("Male", "Male"),
                new SelectListItem("Female", "Female"),
                new SelectListItem("Both", "Both"),
                new SelectListItem("None", "None")

            };
            return genders;
        }

        public static List<SelectListItem> GetDays()
        {
            List<SelectListItem> days = new List<SelectListItem>()
            {
                new SelectListItem("Select Day ... ", "0"),
                new SelectListItem("SaturDay", "1"),
                new SelectListItem("SunDay", "2"),
                new SelectListItem("Monday", "3"),
                new SelectListItem("TuesDay", "4"),
                new SelectListItem("WednesDay", "5"),
                new SelectListItem("ThursDay", "6"),
                new SelectListItem("Friday", "7")
            };
            return days;
        }

     
    }
}
