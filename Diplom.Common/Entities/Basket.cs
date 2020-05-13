using System;
using System.Collections.Generic;
using System.Text;

namespace Diplom.Common.Entities
{
    public class Basket
    {
        public int BasketId { get; set; }
        public string UserId { get; set; } // ид клиента
        public int AdditionMenuId { get; set; } // ид выбронного блюда
        public int Quantity { get; set; } // колличество
        public int OrderId { get; set; } // ид заказа

    }
}
