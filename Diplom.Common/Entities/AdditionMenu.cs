namespace Diplom.Common.Entities
{
    public class AdditionMenu
    {
        public int AdditionMenuId { get; set; }
        public string Grams { get; set; } // количество грам в порции
        public string Calories { get; set; } //калории в порции
        public string Price { get; set; } // цена порции
        public int MenuId { get; set; } // вид продукта
    }
}