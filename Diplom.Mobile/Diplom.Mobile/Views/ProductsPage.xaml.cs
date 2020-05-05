using Diplom.Common.Entities;
using Diplom.Common.Models;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        public Common.Entities.Product[] Menus { get; set; }
        public string ProductPage = "1";

        public ProductsPage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        protected async override void OnAppearing()
        {
            //Menus = await RequestBuilder.Create()
            //                .AppendPathSegments("api", "menu", "menuGet") // добавляет к ендпоинт
            //                .GetJsonAsync<Common.Entities.Menu[]>();  //  http://192.168.1.12:5002/api/menu/menuGet
            //menuList.ItemsSource = Menus;

            var selectedIndex = picker.SelectedIndex;
           // var x = "0";
            MenuType type = MenuType.Food;
            if (selectedIndex == 0)
            {
                type = MenuType.Food;
            }
            else if (selectedIndex == 1)
            {
                type = MenuType.Drink;
            }
            Menus = await RequestBuilder.Create()
                            .AppendPathSegments("api", "product", "productGet") // добавляет к ендпоинт
                            .SetQueryParam("type", type)
                            .GetJsonAsync<Common.Entities.Product[]>();  //  http://192.168.1.12:5002/api/menu/menuGet
        }

        public async void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = picker.SelectedIndex;
            //var x = "menuDrinkGet";

            MenuType type = MenuType.Food;
            if (selectedIndex == 0)
            {
                type = MenuType.Food;
            }
            else if (selectedIndex == 1)
            {
                type = MenuType.Drink;
            }
            Menus = await RequestBuilder.Create()
                            .AppendPathSegments("api", "product", "productGet") // добавляет к ендпоинт
                            .SetQueryParam("type", type)
                            .GetJsonAsync<Common.Entities.Product[]>();  //  http://192.168.1.12:5002/api/menu/menuGet

            menuList.ItemsSource = Menus;
        }

        private async void MenuList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedProduct = e.Item as Common.Entities.Product;
            if(selectedProduct != null)
            {
                ProductPage = "2";
            }
            await Navigation.PushAsync(new AdditionProductsPage(selectedProduct));
        }
    }
}