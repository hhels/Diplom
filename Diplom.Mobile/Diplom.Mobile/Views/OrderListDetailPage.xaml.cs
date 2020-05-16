using Diplom.Common.Entities;
using Diplom.Common.Models;
using Diplom.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderListDetailPage : ContentPage
	{
        private readonly OrderListDetailViewModel _basketViewModel;
        public Order OrderDetail { get; set; }
        public OrderListDetailPage (Order dell)
		{
			InitializeComponent ();
            OrderDetail = dell;

            _basketViewModel = new OrderListDetailViewModel(OrderDetail);
            BindingContext = _basketViewModel;
        }
        //удаление продукта
        public async void OnDelete(object sender, EventArgs e)
        {
            //проверяем готов ли заказа
            if (OrderDetail.Status == StatusType.Completed)
            {
                await DisplayAlert("Внимание", $"Ваш заказ уже готов", "OK");
                return;
            }
            var mi = ((MenuItem)sender);
            var del = mi.CommandParameter as BasketList;
            await DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
            if (del != null)
            {
                await _basketViewModel.deleteBasket(del);
                basketList.ItemsSource = _basketViewModel.BasketList;
                if(!string.IsNullOrEmpty(_basketViewModel.Deleted))
                {
                    await DisplayAlert("Ошибочка", $"{_basketViewModel.Deleted}", "OK");
                    await Navigation.PushAsync(new NewsPage());
                }
            }
            else { await DisplayAlert("Ошибочка", "объект не выбран", "OK"); }

        }
        //прибавление количества порций
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            //проверяем готов ли заказа
            if (OrderDetail.Status == StatusType.Completed)
            {
                await DisplayAlert("Внимание", $"Ваш заказ уже готов", "OK");
                return;
            }
            var mi = ((Button)sender).BindingContext;
            var del = mi as BasketList;
            if (del != null)
            {
                await _basketViewModel.AddQuantity(del);
                await DisplayAlert("del", $"{del.Quantity}", "OK");
                var asd = _basketViewModel.BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity;
                await DisplayAlert("Ошибочка", $"{asd}", "OK");

                basketList.ItemsSource = _basketViewModel.BasketList;

            }
            else { await DisplayAlert("Ошибочка", "объект не выбран", "OK"); }
        }
        // уменьшение количества порций
        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            //проверяем готов ли заказа
            if (OrderDetail.Status == StatusType.Completed)
            {
                await DisplayAlert("Внимание", $"Ваш заказ уже готов", "OK");
                return;
            }
            var mi = ((Button)sender).BindingContext;
            var del = mi as BasketList;
            if (del != null)
            {
                await _basketViewModel.LowerQuantity(del);
                await DisplayAlert("del", $"{del.Quantity}", "OK");
                var asd = _basketViewModel.BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity;
                await DisplayAlert("Ошибочка", $"{asd}", "OK");

                basketList.ItemsSource = _basketViewModel.BasketList;

            }
            else { await DisplayAlert("Ошибочка", "объект не выбран", "OK"); }
        }
    }
}