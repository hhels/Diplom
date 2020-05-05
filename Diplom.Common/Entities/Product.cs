using Diplom.Common.Models;

namespace Diplom.Common.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Img { get; set; }
        public MenuType Type { get; set; }
    }
}