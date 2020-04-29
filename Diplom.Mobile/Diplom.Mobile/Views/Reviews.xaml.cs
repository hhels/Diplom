using Diplom.Common.Entities;
using Diplom.Common.Models;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
 
namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Reviews : ContentPage
    {
        public Review[] Reviewss { get; set; }
       
        public Reviews()
        {
            InitializeComponent();
            this.BindingContext = this;
            
        }
 
        protected async override void OnAppearing()
        {
            //var token = MySettings.Token;
            Reviewss = await Constants.Endpoint
                            .AppendPathSegments("api", "review", "reviewGet") // добавляет к ендпоинт
                            .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
                            .GetJsonAsync<Review[]>();  //  https://192.168.1.12:5002/api/review/reviewGet
            reviewList.ItemsSource = Reviewss;

        }

        
        //private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    sliderZnach.Text = slider.Value.ToString();
        //    //if (slider != null)
        //    //    slider.Text = String.Format("Выбрано: {0:F1}", e.NewValue);
        //}
        //private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var pic = picker.Items[picker.SelectedIndex];
        //}

        // отправление отзыва
        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            var body = new Review()
            {

                Text = reviewEntry.Text,
                Rating = picker.SelectedIndex + 1,
                Date = DateTime.Now,
                //UserId = MySettings.UserId,
                
            };
            var response = await Constants.Endpoint
               .AppendPathSegments("api", "review", "reviewAdd") // добавляет к ендпоинт
               .WithOAuthBearerToken(MySettings.Token) // передача токена
               //.WithHeader("Authorization", $"Bearer {MySettings.Token}")
               .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
               .PostJsonAsync(body);  // 

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("опа", "добавилось", "ок");
            }
            else { await DisplayAlert("опа", "чет пошло не так", "ок"); }
        }


        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Review selectedPhone = e.Item as Review;
            if (selectedPhone != null)
                await DisplayAlert("Выбранная модель", $"{selectedPhone.Rating} - {selectedPhone.Date}", "OK");
        }





        //if (response.IsSuccessStatusCode)
        //    {
        //    var data = JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync());
        //    var body = new Review()
        //    {
        //        Text = data.Text,
        //        Rating = data.Rating,
        //        Date = data.Date,
        //        UserId = data.UserId,
        //    };

        //        public int ReviewId { get; set; }
        //public string Text { get; set; }
        //public int Rating { get; set; }
        //public DateTime Date { get; set; }
        //public int UserId { get; set; }


        //public List<Review[]> Review { get; set; }
        //public MainPage()
        //{
        //    InitializeComponent();
        //    Phones = new List<Phone>
        //    {
        //        new Phone {Title="Galaxy S8", Company="Samsung", Price=48000 },
        //        new Phone {Title="Huawei P10", Company="Huawei", Price=35000 },
        //        new Phone {Title="HTC U Ultra", Company="HTC", Price=42000 },
        //        new Phone {Title="iPhone 7", Company="Apple", Price=52000 }
        //    };
        //    this.BindingContext = this;
        //}


        //private async void Button_Clicked_1(object sender, System.EventArgs e)
        //{
        //    await DisplayAlert("a", MySettings.UserName, "cancel");

        //}
    }
}