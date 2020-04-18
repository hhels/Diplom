using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApplication1.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<SiteUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            const string directorRoleName = "director";
            const string workerRoleName = "worker";
            const string userRoleName = "user";

            var isDirectorExists = await roleManager.RoleExistsAsync(directorRoleName);
            var isWorkerExists = await roleManager.RoleExistsAsync(workerRoleName);
            var isUserExists = await roleManager.RoleExistsAsync(userRoleName);

            if (!isDirectorExists)
            {
                var directorRole = new IdentityRole(directorRoleName);
                await roleManager.CreateAsync(directorRole);
            }
            if (!isWorkerExists)
            {
                var workerRole = new IdentityRole(workerRoleName);
                await roleManager.CreateAsync(workerRole);
            }
            if (!isUserExists)
            {
                var userRole = new IdentityRole(userRoleName);
                await roleManager.CreateAsync(userRole);
            }

            //if (await roleManager.FindByNameAsync("director") == null)
            //{
            //    User admin = new User { Email = adminEmail, UserName = adminEmail };
            //    IdentityResult result = await userManager.CreateAsync(admin, password);
            //    if (result.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(admin, "admin");
            //    }
            //}

        }


        //    // GET: /<controller>/
        //    public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
