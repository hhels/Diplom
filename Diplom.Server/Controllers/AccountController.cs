using System.Linq;
using System.Threading.Tasks;
using Diplom.Common.Bodies;
using Diplom.Common.Models;
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
        public async Task<IActionResult> Login([FromBody] AuthBody data)
        {
            //входим по логину и паролю БЕЗ блокировки пользователя при вводе неправильных данных
            var auth = await _signInManager.PasswordSignInAsync(data.Login, data.Password,
                                                                true, false);

            // если всё ок
            if(auth.Succeeded)
            {
                // ищем сущность этого пользователя
                var user = await _userManager.FindByNameAsync(data.Login);
                var token = AuthService.GenerateToken(user); // создаём токен
                var role = await _userManager.GetRolesAsync(user); // узнаем роль пользователя

                // возвращаем инфу
                var response = new AuthResponse
                {
                    AccessToken = token,
                    UserName = data.Login,
                    Email = user.Email,
                    UserId = user.Id,
                    Role = role.First()
                };

                return Ok(response);
            }

            // а если не ок, то досвиданья
            return BadRequest("Incorrect login/password");
        }

        [HttpPost("register")]
        //public async Task<IActionResult> Register(string login, string password, string email, string firstName, string lastName, int year)
        
        public async Task<IActionResult> Register([FromBody] RegisterBody data)
        {


            var existedUser = await _userManager.FindByNameAsync(data.Login);
            if (existedUser != null)
            {
                return BadRequest("Пользователь с таким логином уже существует");
            }

            var user = new SiteUser
            {
                Email = data.Email,
                UserName = data.Login,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Year = data.Year
            };

            // создаём юзера
            var result = await _userManager.CreateAsync(user, data.Password);

            //добавление роль по дефолту
            await _userManager.AddToRoleAsync(user, RoleNames.User);

            // если всё ок, то токен создаем и возвращаем
            if (result.Succeeded)
            {
                var token = AuthService.GenerateToken(user);

                var response = new AuthResponse
                {
                    AccessToken = token,
                    UserName = data.Login,
                    Email = user.Email,
                    UserId = user.Id,
                    Role = RoleNames.User
                };

                return Ok(response);
            }

            // а если нет, то выдаёт ошибки
            return BadRequest("Произошла ошибка во время создания пользователя"); //TODO:
        }
    }
}