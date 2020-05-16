using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diplom.Common.Entities;
using Diplom.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ContentController(ApplicationDbContext context)
        {
            _db = context;
        }

        [HttpGet("contentGet")]
        public async Task<IActionResult> Get()
        {
            var contents = await _db.Contents.ToArrayAsync();
            foreach(var content in contents)
            {
                content.Img = string.Format("http://192.168.1.12:5002/images/{0}", content.Img);
            }

            return Ok(contents);
        }

        [HttpPost("contentTake")]
        public async Task<IActionResult> Take(int skip)
        {
            const int take = 5;
            var contents = await _db.Contents.ToArrayAsync();
            foreach(var content in contents)
            {
                content.Img = string.Format("http://192.168.1.12:5002/images/{0}", content.Img);
            }

            return Ok(contents.Skip(skip).Take(take));
        }
    }
}