using System;
using Diplom.Common.Entities;
using Diplom.Common.Models;
using Flurl.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        public Product[] Menus { get; set; }

        public ProductsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            var selectedIndex = picker.SelectedIndex;

            var type = MenuType.Food;
            if(selectedIndex == 0)
            {
                type = MenuType.Food;
            }
            else if(selectedIndex == 1)
            {
                type = MenuType.Drink;
            }

            Menus = await RequestBuilder.Create()
                                        .AppendPathSegments("api", "product", "productGet") // добавляет к ендпоинт
                                        .SetQueryParam("type", type)
                                        .GetJsonAsync<Product[]>(); //  http://192.168.1.12:5002/api/menu/menuGet
        }

        public async void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = picker.SelectedIndex;

            var type = MenuType.Food;
            if(selectedIndex == 0)
            {
                type = MenuType.Food;
            }
            else if(selectedIndex == 1)
            {
                type = MenuType.Drink;
            }

            Menus = await RequestBuilder.Create()
                                        .AppendPathSegments("api", "product", "productGet") // добавляет к ендпоинт
                                        .SetQueryParam("type", type)
                                        .GetJsonAsync<Product[]>(); //  http://192.168.1.12:5002/api/menu/menuGet

            menuList.ItemsSource = Menus;
        }

        private async void MenuList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedProduct = e.Item as Product;
            await Navigation.PushAsync(new AdditionProductsPage(selectedProduct));
        }
    }
}