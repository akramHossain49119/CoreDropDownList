using System;
using System.Threading.Tasks;
using CoreDropDownList.Data;
using CoreDropDownList.Models;
using Microsoft.AspNetCore.Identity;

namespace CoreDropDownList
{
    public class TableSeeder
    {

        public static async Task Initialize(ApplicationDbContext context,
                                      UserManager<AppUser>userManager,
                                      RoleManager<IdentityRole>roleManager)
        {
            //context.Database.EnsureCreated();
            string spAccountant = "SuperAccountant";
            string srAccountant = "SrAccountant";
            string urAccountant = "UrAccountant";

            ///End  
            string passwordSp = "P@$$w0rdSp";
            string passwordSr = "P@$$w0rdSr";
            string passwordUr = "P@$$w0rdUR";


            if (await roleManager.FindByNameAsync(spAccountant) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(spAccountant));
            }

            if (await roleManager.FindByNameAsync(srAccountant) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(srAccountant));
            }

            if (await roleManager.FindByNameAsync(urAccountant) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(urAccountant));
            }


            if (await userManager.FindByEmailAsync("akramSuper@gmail.com") == null)
            {
                var user = new AppUser
                {
                    UserName = "akramSuper@gmail.com",
                    UsrName = "Akram Hossain",
                    Email = "akramSuper@gmail.com",
                    Image = null,
                    DepartmentDepId = 1,
                    DOB = (DateTime.Now.Date).ToShortDateString(),
                    JoiningDate = DateTime.Now,
                    UserGender = "Male",
                    UserAddress = "F#3,H#219/1, Godighor Goli, Rayer Bazar, Mohammadpur, Dhaka-1207."
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, passwordSp);
                    await userManager.AddToRoleAsync(user, spAccountant);
                }
            }

            if (await userManager.FindByEmailAsync("akram49119@gmail.com") == null)
            {
                var user = new AppUser
                {
                    UserName = "akram49119@gmail.com",
                    UsrName = "Akram Hossain",
                    Email = "akram49119@gmail.com",
                    Image = null,
                    DepartmentDepId = 1,
                    DOB = (DateTime.Now.Date).ToShortDateString(),
                    JoiningDate = DateTime.Now,
                    UserGender = "Male",
                    UserAddress = "F#3,H#219/1, Godighor Goli, Rayer Bazar, Mohammadpur, Dhaka-1207."
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, passwordSr);
                    await userManager.AddToRoleAsync(user, srAccountant);
                }

            }

            if (await userManager.FindByNameAsync("nawer49119@gmail.com") == null)
            {
                var user = new AppUser
                {
                    UserName = "nawer49119@gmail.com",
                    UsrName = "Nawer Nandini Hossain",
                    Email = "nawer49119@gmail.com",
                    Image = null,
                    DepartmentDepId = 1,
                    DOB = (DateTime.Now.Date).ToShortDateString(),
                    JoiningDate = DateTime.Now,
                    UserGender = "Female",
                    UserAddress = "F#3,H#219/1, Godighor Goli, Rayer Bazar, Mohammadpur, Dhaka-1207."
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, passwordUr);
                    await userManager.AddToRoleAsync(user, urAccountant);
                }

            }

            if (await userManager.FindByNameAsync("user49119@gmail.com") == null)
            {
                var user = new AppUser
                {
                    UserName = "user49119@gmail.com",
                    UsrName = "User User User",
                    Email = "user49119@gmail.com",
                    Image = null,
                    DepartmentDepId = 1,
                    DOB = (DateTime.Now.Date).ToShortDateString(),
                    JoiningDate = DateTime.Now,
                    UserGender = "Male",
                    UserAddress = "F#3,H#219/1, Godighor Goli, Rayer Bazar, Mohammadpur, Dhaka-1207."
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, passwordUr);
                    await userManager.AddToRoleAsync(user, urAccountant);
                }
 
            }

            if (await userManager.FindByNameAsync("malthomas@gmail.com") == null)
            {
                var user = new AppUser
                {
                    UserName = "malthomas@gmail.com",
                    UsrName = "Alfred Malthomas Bhuttabin",
                    Email = "malthomas@gmail.com",
                    Image = null,
                    DepartmentDepId = 1,
                    DOB = (DateTime.Now.Date).ToShortDateString(),
                    JoiningDate = DateTime.Now,
                    UserGender = "Female",
                    UserAddress = "F# 3,H# 219/1, Godighor Goli, Rayer Bazar, Mohammadpur, Dhaka-1207."
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, passwordUr);
                    await userManager.AddToRoleAsync(user, urAccountant);
                }

            }

        }

    }
}
