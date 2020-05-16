using Diplom.Common.Entities;
using Diplom.Common.Models;
using Flurl.Http;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Mobile.ViewModels
{
    // подключенная  PropertyChanged.Fody и файлик FodyWeavers.xml
    [AddINotifyPropertyChangedInterface]
    class OrderListDetailViewModel
    {
        public ObservableCollection<BasketList> BasketList { get; set; }
        public BasketList SelectedBasket { get; set; }
        public string AllPrice { get; set; }
        public string Deleted { get; set; }

        public Order OrderDetail { get; set; }

        public OrderListDetailViewModel(Order dell)
        {
            OrderDetail = dell;
            using (var db = new ApplicationContext())
            {
                //получение записей выбранного заказа
                var basket = RequestBuilder.Create()
                                              .AppendPathSegments("api", "basket", "basketOneGet") // добавляет к ендпоинт
                                              .SetQueryParam("orderOne", OrderDetail.OrderId)
                                              .GetJsonAsync<BasketList[]>().GetAwaiter().GetResult(); //  https://192.168.1.12:5002/api/basket/basketOneGet

                BasketList = new ObservableCollection<BasketList>(basket);
                NumberPrice();
            }
        }
        //уменьшение кол-ва
        public async Task LowerQuantity(BasketList del)
        {
            var response = await RequestBuilder.Create()
                                              .AppendPathSegments("api", "basket", "basketOneDell") // добавляет к ендпоинт
                                              .PostJsonAsync(del);

            var basket = await RequestBuilder.Create()
                                             .AppendPathSegments("api", "basket", "basketOneGet") // добавляет к ендпоинт
                                             .SetQueryParam("orderOne", OrderDetail.OrderId)
                                             .GetJsonAsync<BasketList[]>(); //  https://192.168.1.12:5002/api/basket/basketOneGet

            BasketList = new ObservableCollection<BasketList>(basket);
            NumberPrice();
        }
        //прибавление кол-ва
        public async Task AddQuantity(BasketList del)
        {
            var response = await RequestBuilder.Create()
                                              .AppendPathSegments("api", "basket", "basketOneAdd") // добавляет к ендпоинт
                                              .PostJsonAsync(del);
            var basket = await RequestBuilder.Create()
                                             .AppendPathSegments("api", "basket", "basketOneGet") // добавляет к ендпоинт
                                             .SetQueryParam("orderOne", OrderDetail.OrderId)
                                             .GetJsonAsync<BasketList[]>(); //  https://192.168.1.12:5002/api/basket/basketOneGet

            BasketList = new ObservableCollection<BasketList>(basket);
            NumberPrice();
        }
        //Удаление целой записи
        public async Task deleteBasket(BasketList del)
        {
            var response = await RequestBuilder.Create()
                                              .AppendPathSegments("api", "basket", "basketOrderDell") // добавляет к ендпоинт
                                              .SetQueryParam("orderId", OrderDetail.OrderId)
                                              .PostJsonAsync(del);
            var ok = await response.Content.ReadAsStringAsync();
                Deleted = ok;
            
            var basket = await RequestBuilder.Create()
                                             .AppendPathSegments("api", "basket", "basketOneGet") // добавляет к ендпоинт
                                             .SetQueryParam("orderOne", OrderDetail.OrderId)
                                             .GetJsonAsync<BasketList[]>(); //  https://192.168.1.12:5002/api/basket/basketOneGet
            
            BasketList = new ObservableCollection<BasketList>(basket);
            NumberPrice();
        }

        public void NumberPrice()
        {
            AllPrice = BasketList.Sum(x => x.OverallPrice).ToString();
        }

    }
}
