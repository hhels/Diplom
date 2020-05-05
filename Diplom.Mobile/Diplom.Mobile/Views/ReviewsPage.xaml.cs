using Diplom.Common.Entities;
using Flurl;
using Flurl.Http;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewsPage : ContentPage
    {
        public Review[] Reviews { get; set; }

        public ReviewsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected async override void OnAppearing()
        {
            Reviews = await RequestBuilder.Create()
                            .AppendPathSegments("api", "review", "reviewGet") // добавляет к ендпоинт
                            .GetJsonAsync<Review[]>();  //  https://192.168.1.12:5002/api/review/reviewGet
            reviewList.ItemsSource = Reviews;
        }

        // отправление отзыва
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var body = new Review()
            {
                Text = reviewEntry.Text,
                Rating = picker.SelectedIndex + 1, //Convert To int 32 если делать со слайдером
                Date = DateTime.Now,
            };

            var response = await RequestBuilder.Create()
                                          .AppendPathSegments("api", "review", "reviewAdd") // добавляет к ендпоинт
                                          .PostJsonAsync(body);

            if(response.IsSuccessStatusCode)
            {
                await DisplayAlert("опа", "добавилось", "ок");
            }
            else
            {
                await DisplayAlert("опа", "чет пошло не так", "ок");
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedReview = e.Item as Review;
            if(selectedReview != null)
            {
                await DisplayAlert("Выбранная модель", $"{selectedReview.Rating} - {selectedReview.Date}", "OK");
            }
        }
    }
}