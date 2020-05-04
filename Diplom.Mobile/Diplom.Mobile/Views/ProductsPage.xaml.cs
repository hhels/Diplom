using Flurl;
using Flurl.Http;
using System;
using Diplom.Common.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        public AdditionMenu[] AdditionMenus { get; set; }
        public Common.Entities.Menu Product { get; set; }
        public ProductsPage(Common.Entities.Menu product)
        {
            InitializeComponent();
            Product = product;
        }

        protected async override void OnAppearing()
        {
            await DisplayAlert("Выбранная модель", $"{Product.MenuId} --- eeee", "OK");

            image1.Source = ImageSource.FromUri(new Uri(Product.Img));
            nameProd.Text = Product.Name;
            LongDescription.Text = Product.LongDescription;

            AdditionMenus = await Constants.Endpoint
                            .AppendPathSegments("api", "menu", "menuAdditionGet")
                            .SetQueryParam("menuId", Product.MenuId)
                            .AllowAnyHttpStatus()
                            .GetJsonAsync<AdditionMenu[]>();  //  http://192.168.1.12:5002/api/menu/menuGet
            additionList.ItemsSource = AdditionMenus;
        }
    }
}