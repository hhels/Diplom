using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Diplom.Mobile.Models;
using Diplom.Mobile.Views;

namespace Diplom.Mobile.ViewModels
{
    public class MasterDetailPage1MasterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<DetailPageMenuItem> MenuItems { get; }

        public MasterDetailPage1MasterViewModel()
        {
            MenuItems = new ObservableCollection<DetailPageMenuItem>(new[]
            {
                new DetailPageMenuItem { Id = 0, Title = "Новости", TargetType = typeof(News) },

                // new DetailPageMenuItem { Id = 1, Title = "Page 2" },
                // new DetailPageMenuItem { Id = 2, Title = "Page 3" },
                // new DetailPageMenuItem { Id = 3, Title = "Page 4" },
                // new DetailPageMenuItem { Id = 4, Title = "Page 5" },
            });
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}