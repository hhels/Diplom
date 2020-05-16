using Diplom.Common.Entities;
using Diplom.Common.Models;
using Flurl.Http;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Diplom.Mobile.ViewModels
{
    // подключенная  PropertyChanged.Fody и файлик FodyWeavers.xml
    [AddINotifyPropertyChangedInterface]
    class NewsViewModel
    {
        public ObservableCollection<Content> ContentList { get; set; }
        public bool IsBusy { get; set; }
        public int Skip { get; set; }
        public int SkipStage { get; set; }

        public   NewsViewModel()
        {
            using (var db = new ApplicationContext())
            {
                SkipStage = 1;
                Skip = SkipStage * 5;
                //получение записей 
                var News =  RequestBuilder.Create()
                                            .AppendPathSegments("api", "content", "contentTake") // добавляет к ендпоинт
                                            .SetQueryParam("skip", Skip)
                                            .GetJsonAsync<Content[]>().GetAwaiter().GetResult(); //  http://192.168.1.12:5002/api/content/contentTake

                var sortList = News.OrderByDescending(x => x.ContentId).ToList();
                ContentList = new ObservableCollection<Content>(sortList);
                SkipStage++;
            }
        }
        public async void LoadMoreEmployerResult(Content itemTypeObject)
        {
            IsBusy = true;
            //получение записей 
            var News = RequestBuilder.Create()
                                        .AppendPathSegments("api", "content", "contentTake") // добавляет к ендпоинт
                                        .SetQueryParam("skip", Skip)
                                        .GetJsonAsync<Content[]>().GetAwaiter().GetResult(); //  http://192.168.1.12:5002/api/content/contentTake
            ContentList.Add(News);
            IsBusy = false;
        }
    }
}
