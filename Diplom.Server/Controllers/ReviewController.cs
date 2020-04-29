using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diplom.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Diplom.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Diplom.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Diplom.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        //public int ReviewId { get; set; }
        //public string Text { get; set; }
        //public int Rating { get; set; }
        //public DateTime Date { get; set; }
        //public int UserId { get; set; }  

        //[HttpPost("review")]
        //public async Task<IActionResult> review([FromBody] Review data, string AccessToken)
        //{
        //    var tokenOk = await  AuthResponse(AccessToken);
        //    if (AccessToken )
        //    var record = new Review
        //    {
        //        Text = data.Text,
        //        Rating = data.Rating,
        //        Date = data.Date,
        //        UserId = data.UserId,
        //    };
        //    var result = await CreateAsync(record);
        //}

        private readonly ApplicationDbContext _db;
        public ReviewController(ApplicationDbContext context)
        {
            _db = context;
        }

        [HttpPost("reviewAdd")]
        [Authorize]
        public async Task<IActionResult> ReviewAdd([FromBody] Review data)
        {
            //data.UserId =  User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //data.UserId = User.Identity.Name; 
            data.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);//найти id пользователя по токену
            await _db.Reviews.AddAsync(data); // добавить запись
            await _db.SaveChangesAsync(); // сохранить запись
            return Ok();
        }

        [HttpGet("reviewGet")]
        public async Task<ActionResult<IEnumerable<Review>>> Get()
        {
            return await _db.Reviews.ToArrayAsync();
        }
    }
}