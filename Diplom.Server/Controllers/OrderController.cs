using System.Linq;
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
        public async Task<IActionResult> MakeOrder([FromBody] Order data)
        {
            data.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); //найти id пользователя по токену

            var order = await _db.Orders.AddAsync(new Order
            {
                UserId = data.UserId,
                OrderTime = data.OrderTime,
                LeadTime = data.LeadTime,
                TotalPrice = data.TotalPrice,
                Comment = data.Comment,
                TypePayment = data.TypePayment,
                Status = data.Status,
            }); // добавить запись и выбрать Id это записи

            await _db.SaveChangesAsync(); // сохранить запись
            var orderId = order.Entity.OrderId; //получить Id  сохраненой записи

            //Привязать продукты из корзины к текущему заказу
            var basketUpdate = await _db.Baskets.Where(x => x.UserId == data.UserId && x.OrderId == -1).ToArrayAsync();
            foreach(var menu in basketUpdate)
            {
                menu.OrderId = orderId;
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
        public IActionResult GetUserOrders()
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
        public async Task<IActionResult> DeleteOrder(Order del)
        {
            var order = _db.Orders.FirstOrDefault(x => x.OrderId == del.OrderId);
            _db.Orders.RemoveRange(order);
            await _db.SaveChangesAsync(); // сохранить запись
            return Ok();
        }

        [HttpGet("orderOneGet")]
        public IActionResult Get(int orderOne)
        {
            var order = _db.Orders.FirstOrDefault(x => x.OrderId == orderOne);
            return Ok(order);
        }

        [HttpPost("orderUpdate")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder([FromBody] Order data)
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