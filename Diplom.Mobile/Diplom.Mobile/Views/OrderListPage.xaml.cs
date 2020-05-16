using System;
using Diplom.Common.Entities;
using Diplom.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderListPage : ContentPage
    {
        private readonly OrderListViewModel _orderListViewModel;

        public OrderListPage()
        {
            InitializeComponent();
            _orderListViewModel = new OrderListViewModel();
            BindingContext = _orderListViewModel;
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = (MenuItem) sender;
            var del = mi.CommandParameter as Order;
            await DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
            if(del != null)
            {
                await _orderListViewModel.DeleteOrder(del);
            }
            else
            {
                await DisplayAlert("Ошибочка", "объект не выбран", "OK");
            }
        }

        private async void BasketList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var mi = (MenuItem) sender;
            await DisplayAlert("Delete Context Action", $"{mi}", "OK");
            var del = mi.CommandParameter as Order;
            await DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
            if(del != null)
            {
                await _orderListViewModel.DeleteOrder(del);
            }
            else
            {
                await DisplayAlert("Ошибочка", "объект не выбран", "OK");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var mi = ((Button) sender).BindingContext;
            await DisplayAlert("Delete Context Action", $"{mi}", "OK");
            var del = mi as Order;

            await DisplayAlert("Delete Context Action", $"{del}", "OK");

            // await DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
            if(del != null)
            {
                await Navigation.PushAsync(new OrderDetailPage(del));
            }
            else
            {
                await DisplayAlert("Ошибочка", "объект не выбран", "OK");
            }
        }
    }
}