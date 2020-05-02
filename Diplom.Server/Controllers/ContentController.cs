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
    public class ContentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ContentController(ApplicationDbContext context)
        {
            _db = context;
        }

        [HttpGet("contentGet")]
        public async Task<ActionResult<IEnumerable<Content>>> Get()
        {
            var content = await _db.Contents.ToListAsync();
            content.ForEach(x => x.Img = string.Format("http://192.168.1.12:5002/images/{0}", x.Img));
            return content;
        }
        //[HttpGet("content")]
        //public async Task<IActionResult> Content([FromBody] Content data)
        //{
        //return await _db.Contents.ToArrayAsync();
        //}
    }
}