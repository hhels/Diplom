using Diplom.Common.Entities;
using Flurl;
using Flurl.Http;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        public Content[] Contented { get; set; }
        public NewsPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            Contented = await Constants.Endpoint
                           .AppendPathSegments("api", "content", "contentGet") // добавляет к ендпоинт
                           .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
                           .GetJsonAsync<Content[]>();  //  http://192.168.1.12:5002/api/content/contentGet

            var sortList = Contented.OrderByDescending(x => x.ContentId).ToList();
            newsList.ItemsSource = sortList;
        }
        private void Button_Clicked(object sender, System.EventArgs e)
        {
            var sortList = Contented.OrderByDescending(x => x.ContentId);
            newsList.ItemsSource = sortList;
        }
    }
}