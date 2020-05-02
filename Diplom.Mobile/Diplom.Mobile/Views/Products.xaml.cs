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
	public partial class Products : ContentPage
	{
        public Common.Entities.Menu[] Menus { get; set; }
        public string ProductPage = "1";

        public Products ()
		{
			InitializeComponent ();
            //var picker = Picker.SelectedIndexProperty;
            this.BindingContext = this;
		}

        protected async override void OnAppearing()
        {
            //var token = MySettings.Token;
            Menus = await Constants.Endpoint
                            .AppendPathSegments("api", "menu", "menuFoodGet") // добавляет к ендпоинт
                            .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
                            .GetJsonAsync<Common.Entities.Menu[]>();  //  http://192.168.1.12:5002/api/menu/menuGet
            //var EndpointImage = Constants.EndpointImage;
            menuList.ItemsSource = Menus;

        }
        
        //if (picker = 0)
        public async  void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            var x = "menuDrinkGet";

            if (selectedIndex == 0)
            {
                x = "menuFoodGet";
            }
            else if (selectedIndex == 1)
            {
                x = "menuDrinkGet";
            }
            Menus = await Constants.Endpoint
                            .AppendPathSegments("api", "menu", $"{x}") // добавляет к ендпоинт
                            .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
                            .GetJsonAsync<Common.Entities.Menu[]>();  //  http://192.168.1.12:5002/api/menu/menuGet
                                                                      //var EndpointImage = Constants.EndpointImage;
            menuList.ItemsSource = Menus;
        }

        private async void MenuList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Common.Entities.Menu selectedProduct = e.Item as Common.Entities.Menu;
            if (selectedProduct != null)
                ProductPage = "2";
            await Navigation.PushAsync(new ProductsPage(selectedProduct));
            //await DisplayAlert("Выбранная модель", $"{selectedProduct.MenuId} --- {ProductsPage}", "OK");

        }
    }
}