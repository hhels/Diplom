using System;
using System.Linq;
using Diplom.Common.Entities;
using Diplom.Mobile.ViewModels;
using Flurl.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        public Content[] News { get; set; }

        private readonly NewsViewModel _newsViewModel;
        public NewsPage()
        {
            InitializeComponent();
            _newsViewModel = new NewsViewModel();
            BindingContext = _newsViewModel;
        }

        protected override async void OnAppearing()
        {
            //News = await RequestBuilder.Create()
            //                                .AppendPathSegments("api", "content", "contentGet") // добавляет к ендпоинт
            //                                .GetJsonAsync<Content[]>(); //  http://192.168.1.12:5002/api/content/contentGet

            //var sortList = News.OrderByDescending(x => x.ContentId).ToList();
            //newsList.ItemsSource = sortList;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //отсортировать по новизне добавления
            var sortList = News.OrderByDescending(x => x.ContentId);
            newsList.ItemsSource = sortList;
        }

        private void NewsList_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var itemTypeObject = e.Item as Content;
            if (_newsViewModel.ContentList.Last() == itemTypeObject && _newsViewModel.ContentList.Count() != 1)
            {
                if (_newsViewModel.IsBusy == false)
                {
                    _newsViewModel.LoadMoreEmployerResult(itemTypeObject);
                }
            }
        }
    }
}