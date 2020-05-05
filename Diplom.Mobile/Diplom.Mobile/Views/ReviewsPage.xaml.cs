using Diplom.Common.Entities;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewsPage : ContentPage
    {
        public Review[] Reviews { get; set; }
        string dbPath;

        public ReviewsPage()
        {
            InitializeComponent();
            //dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            //var friend = (Review)BindingContext;
            
            // BindingContext = this;
        }

        protected async override void OnAppearing()
        {
            // если есть подключение к интернету
            if (CrossConnectivity.Current.IsConnected)
            {
                var Reviews = await RequestBuilder.Create()
                              .AppendPathSegments("api", "review", "reviewGet") // добавляет к ендпоинт
                              .GetAsync();  //  https://192.168.1.12:5002/api/review/reviewGet
                //var x = await Reviews.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<Review[]>(await Reviews.Content.ReadAsStringAsync());

                //если ошбка или пришла пустота берем данные из локальной БД
                if (!Reviews.IsSuccessStatusCode || !data.Any())
                {
                    string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
                    using (ApplicationContext db = new ApplicationContext(dbPath))
                    {
                        reviewList.ItemsSource = db.Review.ToList();
                    }
                    base.OnAppearing();
                }

                //берем данные с сервера
                else
                {
                    reviewList.ItemsSource = data;

                    //занесение в локальную БД новых данных
                    using (ApplicationContext db = new ApplicationContext(dbPath))
                    {
                        db.Review.AddRange(data);
                    }
                }
            }
            // если нет подключения к интернету то локальная БД
            else
            {
                string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
                using (ApplicationContext db = new ApplicationContext(dbPath))
                {
                    reviewList.ItemsSource = db.Review.ToList();
                }
                base.OnAppearing();
            }
            
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