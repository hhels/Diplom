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
        public Common.Entities.AdditionMenu[] Producted { get; set; }
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

            Producted = await Constants.Endpoint
                            .AppendPathSegments("api", "menu", "menuAdditionGet")
                            .SetQueryParam("menuId", Product.MenuId)
                            .AllowAnyHttpStatus()
                            .GetJsonAsync<Common.Entities.AdditionMenu[]>();  //  http://192.168.1.12:5002/api/menu/menuGet
                                                                              //var EndpointImage = Constants.EndpointImage;
            additionList.ItemsSource = Producted;
            //var token = MySettings.Token;
            //var Menus =  Constants.aaa;
        }

    }
}