using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diplom.Common.Entities;
using Diplom.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public MenuController(ApplicationDbContext context)
        {
            _db = context;
        }
        [HttpGet("menuFoodGet")]
        public async Task<ActionResult<IEnumerable<Menu>>> Get()
        {
       
            var menus = await _db.Menus.Where(x => x.Type == 1).ToListAsync();
            menus.ForEach(x => x.Img = string.Format("http://192.168.1.12:5002/images/{0}", x.Img));
            return menus;
        }

        [HttpGet("menuDrinkGet")]
        public async Task<ActionResult<IEnumerable<Menu>>> GetDrink()
        {

            var menus = await _db.Menus.Where(x => x.Type == 0).ToListAsync();
            menus.ForEach(x => x.Img = string.Format("http://192.168.1.12:5002/images/{0}", x.Img));
            return menus;
        }
        [HttpGet("menuAdditionGet")]
        //public async Task<ActionResult <IEnumerable<Menu>>> GetAddition()
        public async Task<ActionResult<IEnumerable<AdditionMenu>>> Addition(int menuId)
        {
            var Addition = await _db.AdditionMenus.Where(x => x.MenuId == menuId).ToListAsync();
            return Ok(Addition);
        }

    }
}