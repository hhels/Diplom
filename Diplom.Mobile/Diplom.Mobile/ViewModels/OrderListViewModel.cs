using Diplom.Common.Entities;
using Diplom.Common.Models;
using Flurl.Http;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Mobile.ViewModels
{
    // подключенная  PropertyChanged.Fody и файлик FodyWeavers.xml
    [AddINotifyPropertyChangedInterface]
    class OrderListViewModel
    {
        public ObservableCollection<Order> OrderList { get; set; }
        public BasketList SelectedBasket { get; set; }

        public ConverterPaymentType TypePaymentt { get; set; }

        public OrderListViewModel()
        {
                var basket = RequestBuilder.Create()
                                              .AppendPathSegments("api", "order", "orderGet") // добавляет к ендпоинт
                                              .GetJsonAsync<Order[]>().GetAwaiter().GetResult(); //  https://192.168.1.12:5002/api/order/orderGet
            //Payment(basket);
            //    Status(basket);
                OrderList = new ObservableCollection<Order>(basket);
        }


        //Удаление целой записи
        public async Task deleteOrder(Order del)
        {
            var response = await RequestBuilder.Create()
                                              .AppendPathSegments("api", "order", "orderDell") // добавляет к ендпоинт
                                              .PostJsonAsync(del);
            var basket = await RequestBuilder.Create()
                                             .AppendPathSegments("api", "order", "orderGet") // добавляет к ендпоинт
                                             .GetJsonAsync<Order[]>(); //  https://192.168.1.12:5002/api/order/orderGet

            OrderList = new ObservableCollection<Order>(basket);
        }
    }
}
