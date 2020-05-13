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

        //class Basket
        //{
        //    public int BasketId { get; set; }
        //    public string UserId { get; set; } // ид клиента
        //    public int AdditionMenuId { get; set; } // ид выбронного блюда
        //    public int Quantity { get; set; } // колличество
        //    public int OrderId { get; set; } // ид заказа

        //}


        [HttpPost("basketAdd")]
        [Authorize]
        public async Task<IActionResult> ReviewAdd([FromBody] Basket  data)
        {
            data.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); //найти id пользователя по токену

            //await _db.Reviews.AddAsync(data); // добавить запись
            //await _db.SaveChangesAsync(); // сохранить запись
            return Ok();
        }



    }
}