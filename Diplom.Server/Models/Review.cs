using System;

namespace Diplom.Server.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}