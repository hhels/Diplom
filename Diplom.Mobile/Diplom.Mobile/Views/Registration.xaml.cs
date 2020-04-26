using Diplom.Common.Bodies;
using Diplom.Common.Models;
using Diplom.Mobile.Views.User;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        public Registration()
        {
            InitializeComponent();

        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            if (loginEntry.Text == null || passwordEntry.Text == null || emailEntry.Text == null || firstNameEntry.Text == null || lastNameEntry.Text == null || yearsEntry.Text == null)
            {
                await DisplayAlert("Ошибка", "Заполнены не все поля", "cancel");
                return;
            }

            var body = new RegisterBody()
            {
                
                Login = loginEntry.Text,
                Password = passwordEntry.Text,
                Email = emailEntry.Text,
                FirstName = firstNameEntry.Text,
                LastName = lastNameEntry.Text,
                Year = Convert.ToInt32(yearsEntry.Text),

            };




           // int count = body.Email.Split("@").First().Length;
            if (!body.Email.Contains("@"))
            {
                await DisplayAlert("Ошибка", "Не коректный Email", "cancel");
                return;
            }
            var a = body.Year;
            if (a < 1 || a > 150)
            {
                await DisplayAlert("Ошибка", "Не коректный возраст", "cancel");
                return;
            }
            if (body.Password.Length <= 6)
            {
                await DisplayAlert("Ошибка", "Длина пароля должна быть больше 6", "cancel");
                return;
            }


            var response = await Constants.Endpoint
                .AppendPathSegments("api", "account", "register") // добавляет к ендпоинт
                .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
                .PostJsonAsync(body);  //  https://localhost:5001/api/account/login?login=1&password=1234567

            
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync());
                MySettings.Token = data.AccessToken;
                MySettings.UserName = data.UserName;
                MySettings.Email = data.Email;
                MySettings.UserId = data.UserId;
                MySettings.Role = data.Role;

                if (MySettings.Role == "user")
                {
                    await Navigation.PushAsync(new MasterDetailPage1());
                }
                //else if (MySettings.Role == "worker")
                //{
                //    await DisplayAlert("a", MySettings.Role, "cancel");
                //}
                //else if (MySettings.Role == "director")
                //{
                //    await DisplayAlert("a", MySettings.Role, "cancel");
                //}
            }
            
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("a", error, "cancel");
            }

        }
    }
}