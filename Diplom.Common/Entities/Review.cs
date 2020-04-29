﻿using System;

namespace Diplom.Common.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }
}