using Flurl;
using Flurl.Http;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class News : ContentPage
    {
        public Common.Entities.Content[] Contented { get; set; }
        //public sortList { get; set;}
        public News()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            Contented = await Constants.Endpoint
                           .AppendPathSegments("api", "content", "contentGet") // добавляет к ендпоинт
                           .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
                           .GetJsonAsync<Common.Entities.Content[]>();  //  http://192.168.1.12:5002/api/content/contentGet
                                                                        //var EndpointImage = Constants.EndpointImage;

           

            var sortList = Contented.OrderByDescending(x => x.ContentId).ToList();
            newsList.ItemsSource = sortList;

            //newsList.ItemsSource = Contented;

            //var currentList = ...
            //    this.listView.ItemsSource = currentList;
            //await Task.Delay(10 * 1000);
            //var newList = currentList.OrderByDescending(x = > > x.LastTriggered).ToList();
            //this.listView.ItemsSource = newList;

        }
            private  void Button_Clicked(object sender, System.EventArgs e)
        {
            
            var sortList = Contented.OrderByDescending(x => x.ContentId);
            newsList.ItemsSource = sortList;

        }
    }
}