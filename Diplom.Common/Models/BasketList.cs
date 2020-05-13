using System;
using System.Collections.Generic;
using System.Text;

namespace Diplom.Common.Models
{
    public class BasketList
    {
        public int BasketListId { get; set; }

        public string Price { get; set; } // цена порции

        public string Name { get; set; } // название продукта
        public string ShortDescription { get; set; } // короткое оприсание
        public string Img { get; set; } // картинка продукта

        public int Quantity { get; set; } // колличество
        public int BasketId { get; set; } // ид строчки в корзине

    }
}
