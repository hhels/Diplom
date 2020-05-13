using System;
using System.Linq;
using Diplom.Common.Entities;
using Diplom.Common.Models;
using Flurl.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        public Product[] Menuss { get; set; }

        public ProductsPage()
        {
            //productGet
            InitializeComponent();
            BindingContext = this;
        }


        protected override async void OnAppearing()
        {
            picker.SelectedIndex = 0;
            // если нет подключение к интернету
            if (!CrossConnectivity.Current.IsConnected)
            {
                InsertDataFromLocalDb();
                base.OnAppearing();
                return;
            }

            var Menus = await RequestBuilder.Create()
                                    .AppendPathSegments("api", "product", "productAllGet") // добавляет к ендпоинт
                                    .GetAsync(); //  http://192.168.1.12:5002/api/menu/menuGet

            var data = JsonConvert.DeserializeObject<Product[]>(await Menus.Content.ReadAsStringAsync());

            //если ошбка или пришла пустота берем данные из локальной БД
            if (!Menus.IsSuccessStatusCode || !data.Any())
            {
                InsertDataFromLocalDb();
                base.OnAppearing();
                return;
            }

            //занесение в локальную БД новых данных
            using (var db = new ApplicationContext())
            {
                var test = db.Product.ToList();
                 db.Product.RemoveRange(db.Product);
                await db.SaveChangesAsync();
                var testtt = db.Product.ToList();
                await db.Product.AddRangeAsync(data);
                await db.SaveChangesAsync();
                var testt = db.Product.ToList();
                menuList.ItemsSource = db.Product.ToList();
                InsertDataFromLocalDb();
            }

            //если все ок то данные из инета
            //menuList.ItemsSource = data;

            void InsertDataFromLocalDb()
            {
                using (var db = new ApplicationContext())
                {
                    
                    var selectedIndex = picker.SelectedIndex;
                    var type = MenuType.Food;
                    if (selectedIndex == 0)
                    {
                        type = MenuType.Food;
                    }
                    else if (selectedIndex == 1)
                    {
                        type = MenuType.Drink;
                    }
                    var test = db.Product.ToList();
                    menuList.ItemsSource = db.Product.Where(x => x.Type == type).ToList();
                }
            }
            //var selectedIndex = picker.SelectedIndex;

            //var type = MenuType.Food;
            //if (selectedIndex == 0)
            //{
            //    type = MenuType.Food;
            //}
            //else if (selectedIndex == 1)
            //{
            //    type = MenuType.Drink;
            //}

            //Menus = await RequestBuilder.Create()
            //                            .AppendPathSegments("api", "product", "productGet") // добавляет к ендпоинт
            //                            .SetQueryParam("type", type)
            //                            .GetJsonAsync<Product[]>(); //  http://192.168.1.12:5002/api/menu/menuGet
        }

        public async void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var db = new ApplicationContext())
            {
                
                var selectedIndex = picker.SelectedIndex;

                var type = MenuType.Food;
                if (selectedIndex == 0)
                {
                    type = MenuType.Food;
                }
                else if (selectedIndex == 1)
                {
                    type = MenuType.Drink;
                }
                //берем данные из локальной БД
                menuList.ItemsSource = db.Product.Where(x => x.Type == type).ToList();

            }
            //var Menus = await RequestBuilder.Create()
            //                           .AppendPathSegments("api", "product", "productAllGet") // добавляет к ендпоинт
            //                           .GetAsync(); //  http://192.168.1.12:5002/api/menu/menuGet

            //var data = JsonConvert.DeserializeObject<Product[]>(await Menus.Content.ReadAsStringAsync());
            //menuList.ItemsSource = data;


            //Menus = await RequestBuilder.Create()
            //                            .AppendPathSegments("api", "product", "productGet") // добавляет к ендпоинт
            //                            .SetQueryParam("type", type)
            //                            .GetJsonAsync<Product[]>(); //  http://192.168.1.12:5002/api/menu/menuGet
            //menuList.ItemsSource = Menus;
        }

        private async void MenuList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedProduct = e.Item as Product;
            await Navigation.PushAsync(new AdditionProductsPage(selectedProduct));
        }
    }
}