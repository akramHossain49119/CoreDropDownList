using CoreDropDownList.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDropDownList.Data
{
    public static class ModelBuilderExtensions
    {
        public static void seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Center>().HasData(
             new Center
             {
                  Id = 1,
                 CenterCode = "DHA",
                 CenterName = "Dhaka"

             },
             new Center
             {
                  Id = 2,
                 CenterCode = "KHU",
                 CenterName = "Khulna"
             },
             new Center
             {
                  Id = 3,
                 CenterCode = "BOR",
                 CenterName = "Barisal"
             },
             new Center
             {
                  Id = 4,
                 CenterCode = "CTG",
                 CenterName = "Citagon"
             },
             new Center
             {
                  Id = 5,
                 CenterCode = "SHY",
                 CenterName = "Shylhet"
             });

            modelBuilder.Entity<Branch>().HasData(
                 new Branch
                 {
                     Id = 1,
                     CenterId = 1,
                     BranchName = "Shapla",
                     BranchCode = "SPL"

                 },
                 new Branch
                 {
                      Id = 2,
                     CenterId = 2,
                     BranchName = "Shapla",
                     BranchCode = "SPL"
                 },
                 new Branch
                 {
                      Id = 3,
                     CenterId = 3,
                     BranchName = "Shapla",
                     BranchCode = "SPL"
                 },
                 new Branch
                 {
                      Id = 4,
                     CenterId = 4,
                     BranchName = "Shapla",
                     BranchCode = "SPL"
                 },
                 new Branch
                 {
                      Id = 5,
                     CenterId = 5,
                     BranchName = "Shapla",
                     BranchCode = "SPL"
                 });

            modelBuilder.Entity<Department>().HasData(
                   new Department
                   {
                       DepId = 2,
                       DepCode = "ADMN",
                       DepName = "Administrtor",
                       DepFloor = "1st Floor"
                   },
                   new Department
                   {
                       DepId = 1,
                       DepCode = "MDG",
                       DepName = "Managing Director General",
                       DepFloor = "1st Floor"
                   },
                   new Department
                   {
                       DepId = 3,
                       DepCode = "ACCO",
                       DepName = "Accounce",
                       DepFloor = "1st Floor"
                   },
                   new Department
                   {
                       DepId = 4,
                       DepCode = "EXECU",
                       DepName = "Executive",  
                       DepFloor = "1st Floor"
                   },
                   new Department
                   {
                       DepId = 5,
                       DepCode = "STUFF",
                       DepName = "Supporting Stuff",
                       DepFloor = "1st Floor"
                   });


        }
    }
}
