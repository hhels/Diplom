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
	public partial class BasketPage : ContentPage
	{
        private readonly BasketViewModel _basketViewModel;
        public BasketPage ()
		{
			InitializeComponent ();
            //BindingContext = new BasketViewModel();
            _basketViewModel = new BasketViewModel();
            BindingContext = _basketViewModel;
        }
        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter.ToString() + " more context action", "OK");
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var del = mi.CommandParameter as BasketList;
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
            if (del != null)
            {
                _basketViewModel.deleteBasket(del);
            }
            else { DisplayAlert("Ошибочка", "объект не выбран", "OK"); }
            
        }

        private void BasketList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Binding BasketViewModel SelectedBasket;
            DisplayAlert("Delete Context Action", " тапед", "OK");
        }

        private void BasketList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DisplayAlert("Delete Context Action", " сеелектед", "OK");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Delete Context Action", " началось удаление", "OK");
            using (var db = new ApplicationContext())
            {
                db.Basket.RemoveRange(db.Basket);
                db.BasketList.RemoveRange(db.BasketList);
                db.SaveChangesAsync();
                DisplayAlert("Delete Context Action", " удалилось из таблиц", "OK");

            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            //var stringInThisCell = (string)((Button)sender).BindingContext;
            //myList.Remove(stringInThisCell);

            var mi = ((Button)sender).BindingContext;
            var del = mi as BasketList;
            if (del != null)
            {
                _basketViewModel.AddQuantity(del);
                DisplayAlert("del", $"{del.Quantity}", "OK");
                var asd = _basketViewModel.BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity;
                DisplayAlert("Ошибочка", $"{asd}", "OK");
                basketList.ItemsSource = _basketViewModel.BasketList;

            }
            else { DisplayAlert("Ошибочка", "объект не выбран", "OK"); }
        }
        public void ApdateBasket()
        {
            
        }

        //private void BasketList_ItemTapped(object sender, ItemTappedEventArgs e)
        //{

        //}

        //protected override async void OnAppearing()
        //{
        //    //using (var db = new ApplicationContext())
        //    //{
        //    //    var test = db.BasketList.ToList();
        //    //    basketList.ItemsSource = db.BasketList.ToList();
        //    //}

        //}


        //private async void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    var znach = e.NewValue;
        //    await DisplayAlert("Выбранная модель", $"{znach} - ", "OK");
        //    //lable.Text = String.Format("Выбрано: {0:F1}", e.NewValue);
        //}

        //private async void BasketList_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    var selectedProduct = e.Item as BasketViewModel;
        //    await DisplayAlert("Выбранная модель", $"{selectedProduct} - ", "OK");

        //}

        //private async void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    var znach = e.NewValue;
        //    await DisplayAlert("Выбранная модель", $"{znach} - ", "OK");
        //    var z = e.OldValue;
        //    await DisplayAlert("Выбранная модель", $"{z} - ", "OK");
        //    //lable.Text = String.Format("Выбрано: {0:F1}", e.NewValue);

        //}
        //вывод меню 
        //удаление из меню
        //смена кол-ва порций
        //оформление заказа
    }
}