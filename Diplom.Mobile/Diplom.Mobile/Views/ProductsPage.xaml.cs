using Diplom.Common.Entities;
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
        public Common.Entities.Menu[] Menus { get; set; }
        public string ProductPage = "1";

        public ProductsPage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        protected async override void OnAppearing()
        {
            Menus = await RequestBuilder.Create()
                            .AppendPathSegments("api", "menu", "menuFoodGet") // добавляет к ендпоинт
                            .GetJsonAsync<Common.Entities.Menu[]>();  //  http://192.168.1.12:5002/api/menu/menuGet
            menuList.ItemsSource = Menus;

        }

        public async void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = picker.SelectedIndex;
            var x = "menuDrinkGet";

            if(selectedIndex == 0)
            {
                x = "menuFoodGet";
            }
            else if(selectedIndex == 1)
            {
                x = "menuDrinkGet";
            }
            Menus = await RequestBuilder.Create()
                            .AppendPathSegments("api", "menu", $"{x}") // добавляет к ендпоинт
                            .GetJsonAsync<Common.Entities.Menu[]>();  //  http://192.168.1.12:5002/api/menu/menuGet

            menuList.ItemsSource = Menus;
        }

        private async void MenuList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedProduct = e.Item as Common.Entities.Menu;
            if(selectedProduct != null)
            {
                ProductPage = "2";
            }
            await Navigation.PushAsync(new AdditionProductsPage(selectedProduct));
        }
    }
}