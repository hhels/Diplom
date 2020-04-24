using System;

namespace Diplom.Common.Entities
{
    public class Content
    {
        public int ContentId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Img { get; set; }
        public DateTime Date { get; set; }
    }
}