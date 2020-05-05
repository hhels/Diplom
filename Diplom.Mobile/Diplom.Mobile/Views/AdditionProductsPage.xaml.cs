using System;
using Diplom.Common.Entities;
using Flurl.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdditionProductsPage : ContentPage
    {
        public AdditionMenu[] AdditionMenus { get; set; }
        public Product Product { get; set; }

        public AdditionProductsPage(Product product)
        {
            InitializeComponent();
            Product = product;
        }

        protected override async void OnAppearing()
        {
            await DisplayAlert("Выбранная модель", $"{Product.ProductId} --- eeee", "OK");

            image1.Source = ImageSource.FromUri(new Uri(Product.Img));
            nameProd.Text = Product.Name;
            LongDescription.Text = Product.LongDescription;

            AdditionMenus = await RequestBuilder.Create()
                                                .AppendPathSegments("api", "product", "productAdditionGet")
                                                .SetQueryParam("menuId", Product.ProductId)
                                                .GetJsonAsync<AdditionMenu[]>(); //  http://192.168.1.12:5002/api/menu/menuGet
            additionList.ItemsSource = AdditionMenus;
        }
    }
}