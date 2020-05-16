using System.Security.Claims;
using System.Threading.Tasks;
using Diplom.Common.Entities;
using Diplom.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ReviewController(ApplicationDbContext context)
        {
            _db = context;
        }

        //добавить отзыв пользователя
        [HttpPost("reviewAdd")]
        [Authorize]
        public async Task<IActionResult> ReviewAdd([FromBody] Review data)
        {
            data.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); //найти id пользователя по токену
            await _db.Reviews.AddAsync(data); // добавить запись
            await _db.SaveChangesAsync(); // сохранить запись
            return Ok();
        }

        //получить отзывы пользователей
        [HttpGet("reviewGet")]
        public async Task<IActionResult> Get()
        {
            var reviews = await _db.Reviews.ToArrayAsync();
            return Ok(reviews);
        }
    }
}