using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diplom.Common.Entities;
using Diplom.Common.Models;
using Diplom.Server.Models;
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

        [HttpGet("menuGet")]
        public async Task<ActionResult<IEnumerable<Menu>>> Get(MenuType type)
        {
            var menus = await _db.Menus.Where(x => x.Type == type).ToArrayAsync();
            foreach(var menu in menus)
            {
                menu.Img = string.Format("http://192.168.1.12:5002/images/{0}", menu.Img);
            }
            return menus;
        }

        [HttpGet("menuAdditionGet")]
        public async Task<ActionResult> Addition(int menuId)
        {
            var additions = await _db.AdditionMenus.Where(x => x.MenuId == menuId).ToArrayAsync();
            return Ok(additions);
        }
    }
}