using System;
using System.Linq;
using Diplom.Common.Entities;
using Flurl.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        public Content[] News { get; set; }

        public NewsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            News = await RequestBuilder.Create()
                                            .AppendPathSegments("api", "content", "contentGet") // добавляет к ендпоинт
                                            .GetJsonAsync<Content[]>(); //  http://192.168.1.12:5002/api/content/contentGet

            var sortList = News.OrderByDescending(x => x.ContentId).ToList();
            newsList.ItemsSource = sortList;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var sortList = News.OrderByDescending(x => x.ContentId);
            newsList.ItemsSource = sortList;
        }
    }
}