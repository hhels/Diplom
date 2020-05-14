using Diplom.Common.Entities;
using Diplom.Common.Models;
using Flurl.Http;
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
	public partial class OrderPage : ContentPage
	{
        public string prise { get; set; }
        public OrderPage (string AllPrice)
		{
			InitializeComponent ();
            var prise = AllPrice;

        }
        protected override async void OnAppearing()
        {
            
            datePicker.MinimumDate = DateTime.UtcNow;
            datePicker.MaximumDate = DateTime.UtcNow.AddDays(7);
            timePicker.Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            //DisplayAlert("дата", $"{datePicker.Date} выбрано", "OK");
            //if(datePicker.Date.DayOfWeek != DayOfWeek.Sunday)
            //{
            //    DisplayAlert("дата", $" хорошая дата", "OK");
            //}
            //else
            //{
            //    datePicker.Date = DateTime.Now;
            //}
        }

        private void TimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //DisplayAlert("время", $"{timePicker.Time} выбрано", "OK");
            //var vibrVrem = timePicker.Time;// выбранное время
            //TimeSpan date1 = new TimeSpan(DateTime.Now.Hour); //текущее время
            //DisplayAlert("время", $"{date1} текушее время", "OK");
            //TimeSpan maxVrem = new TimeSpan(18, 00, 00);
            //TimeSpan minVrem = new TimeSpan(08, 00, 00);

            //if (vibrVrem > date1 && vibrVrem < maxVrem && vibrVrem > minVrem)
            //{
            //    DisplayAlert("время", $"выбранное время находится в нужном диапазоне", "OK");
            //    return;
            //}
            //else
            //{
            //    DisplayAlert("время", $"вне  диапазона", "OK");
            //    timePicker.Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            //    return;
            //}


            //TimeSpan timeSpan = endDatePicker.Date - startDatePicker.Date;

            //var date1 = DateTime.Now;
            //var startTime = date1.ToShortTimeString();

            //if (e. <= startTime)
            //{

            //}
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //DisplayAlert("дата", $"{datePicker.Date} выбрано", "OK");
            if (datePicker.Date.DayOfWeek != DayOfWeek.Sunday)
            {
                await DisplayAlert("дата", $" хорошая дата", "OK");
                
            }
            else
            {
                await DisplayAlert("дата", $"Не верная дата", "OK");
                datePicker.Date = DateTime.Now;
                return;
            }


           // DisplayAlert("время", $"{timePicker.Time} выбрано", "OK");
            var vibrVrem = timePicker.Time;// выбранное время
            TimeSpan date1 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0) ; //текущее время
            //DisplayAlert("время", $"{date1} текушее время", "OK");
            TimeSpan maxVrem = new TimeSpan(18, 00, 00);
            TimeSpan minVrem = new TimeSpan(08, 00, 00);

            if (vibrVrem > date1 && vibrVrem < maxVrem && vibrVrem > minVrem)
            {
                await DisplayAlert("время", $"выбранное время находится в нужном диапазоне", "OK");
                
            }
            else
            {
                await DisplayAlert("Внимание", $"Не верное время", "OK");
                timePicker.Time = new TimeSpan(DateTime.Now.AddHours(1).Hour, DateTime.Now.Minute, 0);
                return;
            }

            //    < !--public int OrderId { get; set; }           заказ
            //public string UserId { get; set; } // ид клиента
            //public DateTime OrderTime { get; set; } // дата и время оформления 
            //public DateTime LeadTime { get; set; } //дата и время к которому нужно выполнить заказ +++++++++++
            //public float TotalPrice { get; set; } //общая цена заказа
            //public string Comment { get; set; } //коментарий пользователя  +++++++++++++++
            //public PaymentType TypePayment { get; set; } // тип оплаты +++++++++++++++
            //public StatusType Status { get; set; } // статус заказа-->

            //DisplayAlert("время", $"{datePicker.Date + timePicker.Time}", "OK");
            picker.SelectedIndex = 0;
            
            var selectedIndex = picker.SelectedIndex;
            var type = PaymentType.Cash;
            if (selectedIndex == 0)
            {
                type = PaymentType.Cash;
            }
            else if (selectedIndex == 1)
            {
                type = PaymentType.Card;
            }

            var data = new Order
            {
                UserId = MySettings.UserId,
                OrderTime = DateTime.Now,
                LeadTime = datePicker.Date + timePicker.Time,
                TotalPrice = Convert.ToInt32(prise),
                Comment = reviewEntry.Text,
                TypePayment = type,
                Status = StatusType.Processing,
            };
             using  (var db = new ApplicationContext())
             {
                await db.Order.AddRangeAsync(data);
                await db.SaveChangesAsync();
             }
            var response = await RequestBuilder.Create()
                                           .AppendPathSegments("api", "order", "orderAdd") // добавляет к ендпоинт
                                           .PostJsonAsync(data);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("время", $"Заказ успешно оформлен", "OK");
            }
        }
    }
}