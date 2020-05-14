﻿using System;
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
    public class BasketController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public BasketController(ApplicationDbContext context)
        {
            _db = context;
        }

        [HttpPost("basketAdd")]
        [Authorize]
        public async Task<IActionResult> BasketAdd([FromBody] Basket data)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //найти id пользователя по токену
            //проверяет есть ли уже эта запись в корзине
            var existedUser = _db.Baskets.Any(x => x.UserId == userId && x.OrderId == -1 && x.AdditionMenuId == data.AdditionMenuId);
            if (existedUser)
            {
                return BadRequest("Запись уже добавлена в корзину");
            }

            data.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); //найти id пользователя по токену
            var basket = new Basket
            {
                UserId = data.UserId,
                AdditionMenuId = data.AdditionMenuId,
                Quantity = data.Quantity,
                OrderId = data.OrderId
                
            };

            await _db.Baskets.AddAsync(basket); // добавить запись
            await _db.SaveChangesAsync(); // сохранить запись
            return Ok();
        }


        //public int BasketListId { get; set; }

        //public string Price { get; set; } // цена порции

        //public string Name { get; set; } // название продукта
        //public string ShortDescription { get; set; } // короткое оприсание
        //public string Img { get; set; } // картинка продукта

        //public int Quantity { get; set; } // колличество
        //public int BasketId { get; set; } // ид строчки в корзине

        //public int OverallPrice { get; set; } //цена с учетом количества
        //public int AllPrice { get; set; } //общая цена покупки
        [HttpGet("basketGet")]
        [Authorize]
        public async Task<IActionResult> BasketGet()
        {
            var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier); //найти id пользователя по токену

            //все записи пользователя которые должны отобразаться в корзине
            var baskets = await _db.Baskets.Where(x => x.UserId == userId && x.OrderId == -1).ToArrayAsync();
            var result = from b in baskets
                         join aditionMenu in _db.AdditionMenus on b.AdditionMenuId equals aditionMenu.AdditionMenuId //название новая таблица поле из первой таблицы поле из новой таблицы
                         join product in _db.Products on aditionMenu.MenuId equals product.ProductId
                         select new BasketList
                         {
                            Price = aditionMenu.Price,
                            Name = product.Name,
                            ShortDescription = product.ShortDescription,
                            Img = string.Format("http://192.168.1.12:5002/images/{0}", product.Img),
                            Quantity = b.Quantity,
                            BasketId = b.BasketId,
                            OverallPrice = b.Quantity *  Convert.ToInt32(aditionMenu.Price),
                         };
            return Ok(result);
        }

        [HttpPost("basketDell")]
        [Authorize]
        public async Task<IActionResult> BasketDell(BasketList del)
        {
            var basket = _db.Baskets.FirstOrDefault(x => x.BasketId == del.BasketId);
             _db.Baskets.RemoveRange(basket);
            await _db.SaveChangesAsync(); // сохранить запись
            return Ok();
        }
   
        [HttpPost("basketOneAdd")]
        [Authorize]
        public async Task<IActionResult> basketOneAdd(BasketList del)
        {
            var basket = _db.Baskets.FirstOrDefault(x => x.BasketId == del.BasketId).Quantity;
            if (basket >= 10)
            {
                return BadRequest("Максимальное количество");
            }
            else
            {
                _db.Baskets.FirstOrDefault(x => x.BasketId == del.BasketId).Quantity++;
                await _db.SaveChangesAsync(); // сохранить запись
                return Ok();
            }
            
        }

        [HttpPost("basketOneDell")]
        [Authorize]
        public async Task<IActionResult> basketOneDell(BasketList del)
        {
            var basket = _db.Baskets.FirstOrDefault(x => x.BasketId == del.BasketId).Quantity;
            if (basket <= 1)
            {
                return BadRequest("Достигнуто минимальное количество");
            }
            else
            {
                _db.Baskets.FirstOrDefault(x => x.BasketId == del.BasketId).Quantity--;
                await _db.SaveChangesAsync(); // сохранить запись
                return Ok();
            }
            
        }
    }
}