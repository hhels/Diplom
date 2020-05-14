using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Diplom.Common.Entities;
using Diplom.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diplom.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext context)
        {
            _db = context;
        }

        [HttpPost("orderAdd")]
        [Authorize]
        public async Task<IActionResult> OrderAdd([FromBody] Order data)
        {
            data.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); //найти id пользователя по токену
            var order = new Order
            {
                UserId = data.UserId,
                OrderTime = data.OrderTime,
                LeadTime = data.LeadTime,
                TotalPrice = data.TotalPrice,
                Comment = data.Comment,
                TypePayment = data.TypePayment,
                Status = data.Status,
            };

            await _db.Orders.AddAsync(order); // добавить запись
            await _db.SaveChangesAsync(); // сохранить запись
            return Ok();
        }



    }
}