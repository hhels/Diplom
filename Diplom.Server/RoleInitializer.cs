using System.Threading.Tasks;
using Diplom.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace Diplom.Server
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
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
        }
    }
}
