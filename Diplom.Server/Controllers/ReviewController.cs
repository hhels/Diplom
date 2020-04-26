using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diplom.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diplom.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        //public int ReviewId { get; set; }
        //public string Text { get; set; }
        //public int Rating { get; set; }
        //public DateTime Date { get; set; }
        //public int UserId { get; set; }  
        [HttpPost("review")]
        public async Task<IActionResult> review([FromBody] Review data)
        {
            var record = new Review
            {
                Text = data.Text,
                Rating = data.Rating,
                Date = data.Date,
                UserId = data.UserId,
            };
            var result = await CreateAsync(record);
        }
    }
}