using System.Threading.Tasks;
using Diplom.Server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Diplom.Server.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<SiteUser> _signInManager; // манагер для авторизации
        private readonly UserManager<SiteUser> _userManager; // манагер для управления пользователями
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<SiteUser> signInManager, UserManager<SiteUser> userManager,
                                 RoleManager<IdentityRole> manager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = manager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            //входим по логину и паролю БЕЗ блокировки пользователя при вводе неправильных данных
            var auth = await _signInManager.PasswordSignInAsync(username, password,
                                                                true, false);

            // если всё ок
            if(auth.Succeeded)
            {
                // ищем сущность этого пользователя
                var user = await _userManager.FindByNameAsync(username);
                var token = AuthService.GenerateToken(user); // создаём токен
                var role = await _userManager.GetRolesAsync(user); // узнаем роль пользователя

                // возвращаем инфу
                var response = new
                {
                    access_token = token,
                    user_name = username,
                    email = user.Email,
                    role
                };

                return Ok(response);
            }

            // а если не ок, то досвиданья
            return BadRequest("Incorrect login/password");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string login, string password, string email, string firstName, string lastName, int year)
        {
            var user = new SiteUser
            {
                Email = email,
                UserName = login,
                FirstName = firstName,
                LastName = lastName,
                Year = year
            };

            // создаём юзера
            var result = await _userManager.CreateAsync(user, password);

            //добавление роль по дефолту
            await _userManager.AddToRoleAsync(user, RoleNames.User);

            // если всё ок, то токен создаем и возвращаем
            if(result.Succeeded)
            {
                var token = AuthService.GenerateToken(user);

                var response = new
                {
                    access_token = token,
                    user_name = login,
                    email = user.Email
                };

                return Ok(response);
            }

            // а если нет, то выдаёт ошибки
            return BadRequest(string.Join("\n", result.Errors)); //TODO:
        }
    }
}