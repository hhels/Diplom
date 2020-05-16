using System;
using System.Linq;
using Diplom.Common.Entities;
using Flurl.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewsPage : ContentPage
    {
        public ReviewsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            // если нет подключение к интернету
            if(!CrossConnectivity.Current.IsConnected)
            {
                InsertDataFromLocalDb();
                base.OnAppearing();
                return;
            }

            var reviews = await RequestBuilder.Create()
                                              .AppendPathSegments("api", "review", "reviewGet") // добавляет к ендпоинт
                                              .GetAsync(); //  https://192.168.1.12:5002/api/review/reviewGet

            var data = JsonConvert.DeserializeObject<Review[]>(await reviews.Content.ReadAsStringAsync());

            //если ошбка или пришла пустота берем данные из локальной БД
            if(!reviews.IsSuccessStatusCode || !data.Any())
            {
                InsertDataFromLocalDb();

                base.OnAppearing();
                return;
            }

            //TODO: проверять на копии
            //занесение в локальную БД новых данных
            using(var db = new ApplicationContext())
            {
                db.Review.RemoveRange(db.Review);
                await db.Review.AddRangeAsync(data);
                await db.SaveChangesAsync();
            }

            //reviewList.ItemsSource = data;
            using(var db = new ApplicationContext())
            {
                reviewList.ItemsSource = db.Review.ToList();
            }

            void InsertDataFromLocalDb()
            {
                using(var db = new ApplicationContext())
                {
                    reviewList.ItemsSource = db.Review.ToList();
                }
            }
        }

        // отправление отзыва
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var body = new Review
            {
                Text = reviewEntry.Text,
                Rating = picker.SelectedIndex + 1, //Convert To int 32 если делать со слайдером
                Date = DateTime.Now,
            };
            var text = reviewEntry.Text;

            if(string.IsNullOrEmpty(text))
            {
                await DisplayAlert("Ошибка", "Заполнены не все поля", "ок");
                return;
            }

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