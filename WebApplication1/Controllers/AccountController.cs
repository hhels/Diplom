using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<SiteUser> _signInManager; // манагер для авторизации
        private readonly UserManager<SiteUser> _userManager; // манагер для управления пользователями

        public AccountController(ApplicationDbContext context, SignInManager<SiteUser> signInManager, UserManager<SiteUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Token(string username, string password)
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

                // возвращаем инфу
                var response = new
                {
                    access_token = token,
                    user_name = username,
                    email = user.Email
                };

                return Ok(response);
            }

            // а если не ок, то досвиданья
            return BadRequest("Incorrect login/password");
        }

        [HttpPost("/register")]
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
            return BadRequest(string.Join("\n", result.Errors));
        }
    }
}