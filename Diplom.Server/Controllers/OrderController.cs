using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Diplom.Common.Entities;
using Diplom.Common.Models;
using Diplom.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        //Оформить заказ
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

            var orderId = _db.Orders.Add(order); // добавить запись и выбрать Id это записи
            
            await _db.SaveChangesAsync(); // сохранить запись
            var Idorder = orderId.Entity.OrderId;//получить Id  сохраненой записи

            //Привязать продукты из корзины к текущему заказу
            var BasketApdate = await _db.Baskets.Where(x => x.UserId == data.UserId && x.OrderId == -1).ToArrayAsync();
            foreach (var menu in BasketApdate)
            {
                menu.OrderId = Idorder;
            }
            await _db.SaveChangesAsync(); // сохранить запись
            return Ok();
            //var orderId = _db.Orders.Add(new Order
            //{
            //    UserId = data.UserId,
            //    OrderTime = data.OrderTime,
            //    LeadTime = data.LeadTime,
            //    TotalPrice = data.TotalPrice,
            //    Comment = data.Comment,
            //    TypePayment = data.TypePayment,
            //    Status = data.Status,
            //}).Entity.OrderId;
        }

        //Получить список заказов пользователя
        [HttpGet("orderGet")]
        [Authorize]
        public async Task<IActionResult> OrderGet()
        {
            //найти id пользователя по токену
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            //все заказы пользователя
            var order = _db.Orders.Where(x => x.UserId == userId);
            return Ok(order);
        }

        //Удалить выбраный заказ
        [HttpPost("orderDell")]
        [Authorize]
        public async Task<IActionResult> OrderDell(Order del)
        {
            var order = _db.Orders.FirstOrDefault(x => x.OrderId == del.OrderId);
            _db.Orders.RemoveRange(order);
            await _db.SaveChangesAsync(); // сохранить запись
            return Ok();
        }

        [HttpGet("orderOneGet")]
        public IActionResult Get(int orderOne)
        {
            var order =  _db.Orders.FirstOrDefault(x => x.OrderId == orderOne);
            return Ok(order);
        }

        [HttpPost("orderUpdate")]
        [Authorize]
        public async Task<IActionResult> OrderUpdate([FromBody] Order data)
        {
            var order = _db.Orders.FirstOrDefault(x => x.OrderId == data.OrderId);
            order.Comment = data.Comment;
            order.LeadTime = data.LeadTime;
            order.Status = data.Status;
            order.TypePayment = data.TypePayment;

            await _db.SaveChangesAsync(); // сохранить изменения

            return Ok();
        }

    }
}