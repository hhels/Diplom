using System.Linq;
using System.Threading.Tasks;
using Diplom.Common.Models;
using Diplom.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext context)
        {
            _db = context;
        }

        //получение записей меню
        [HttpGet("productGet")]
        public async Task<IActionResult> Get(MenuType type)
        {
            var menus = await _db.Products.Where(x => x.Type == type).ToArrayAsync();
            foreach(var menu in menus)
            {
                menu.Img = string.Format("http://192.168.1.12:5002/images/{0}", menu.Img);
            }

            return Ok(menus);
        }

        //получение всех записей меню
        [HttpGet("productAllGet")]
        public async Task<ActionResult> GetAll()
        {
            var products = await _db.Products.ToArrayAsync();
            foreach(var product in products)
            {
                product.Img = string.Format("http://192.168.1.12:5002/images/{0}", product.Img);
            }

            return Ok(products);
        }

        //получение расширенных данных выбраной записи
        [HttpGet("productAdditionGet")]
        public async Task<ActionResult> Addition(int menuId)
        {
            var additions = await _db.AdditionMenus.Where(x => x.ProductId == menuId).ToArrayAsync();
            return Ok(additions);
        }
    }
}