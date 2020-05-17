using Diplom.Common.Entities;
using Diplom.Common.Models;
using Flurl.Http;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Diplom.Mobile.ViewModels
{
    // подключенная  PropertyChanged.Fody и файлик FodyWeavers.xml
    [AddINotifyPropertyChangedInterface]
    class NewsViewModel
    {
        public ObservableCollection<Content> ContentList { get; set; }
        public bool IsBusy { get; set; } // последняя запись в БД или нет
        public int Skip { get; set; } // сколько записей пропустить при запросе
        public int SkipStage { get; set; } // кол-во загруженых записей
        public bool IsRefreshing { get; set; } // иконка загрузки

        public ICommand RefreshCommand { get; set; }

        public  NewsViewModel()
        {
            IsBusy = true;
            IsRefreshing = false;
            SkipStage = 0;
            Skip = SkipStage * 5;
            //получение записей 
            var news =  RequestBuilder.Create()
                                        .AppendPathSegments("api", "content", "contentTake") // добавляет к ендпоинт
                                        .SetQueryParam("skip", Skip)
                                        .GetJsonAsync<Content[]>()
                                        .GetAwaiter().GetResult(); //  http://192.168.1.12:5002/api/content/contentTake

            //var sortList = news.OrderByDescending(x => x.ContentId).ToList();
            ContentList = new ObservableCollection<Content>(news);
            SkipStage++;

            RefreshCommand = new Command<Content>(commandParameter =>
            {
                IsBusy = true;
                IsRefreshing = true;
                SkipStage = 0;
                Skip = SkipStage * 5;
                var newss = RequestBuilder.Create()
                                        .AppendPathSegments("api", "content", "contentTake") // добавляет к ендпоинт
                                        .SetQueryParam("skip", Skip)
                                        .GetJsonAsync<Content[]>()
                                        .GetAwaiter().GetResult(); //  http://192.168.1.12:5002/api/content/contentTake

                ContentList = new ObservableCollection<Content>(newss);
                IsRefreshing = false;
            });
        }
        
        public async Task LoadMoreEmployerResult(Content itemTypeObject)
        {
            IsRefreshing = true;
            Skip = (5*SkipStage);
            //получение записей 
            var news =  RequestBuilder.Create()
                                        .AppendPathSegments("api", "content", "contentTake") // добавляет к ендпоинт
                                        .SetQueryParam("skip", Skip)
                                        .GetJsonAsync<Content[]>() 
                                        .GetAwaiter().GetResult(); //  http://192.168.1.12:5002/api/content/contentTake
            if (!news.Any())
            {
                IsBusy = false;
            }
            SkipStage++;
            var xxx = ContentList;
            foreach (var x in news)
            {
                 xxx.Add(x);  
            }
            //var sortList = xxx.OrderByDescending(x => x.ContentId).ToList();
            ContentList = new ObservableCollection<Content>(xxx);
            IsRefreshing = false;
            return;
            //var sortList = news.OrderByDescending(x => x.ContentId).ToList();
            //ContentList = new ObservableCollection<Content>(sortList);
            
        }
    }
}
