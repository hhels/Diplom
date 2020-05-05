using System;
using Diplom.Common;
using Diplom.Common.Bodies;
using Diplom.Common.Models;
using Diplom.Mobile.Views.DetailMenu;
using Flurl.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var body = new AuthBody
            {
                Login = loginEntry.Text,
                Password = passwordEntry.Text
            };
            var response = await RequestBuilder.Create()
                                               .AppendPathSegments("api", "account", "login") // добавляет к ендпоинт
                                               .PostJsonAsync(body); //  https://localhost:5001/api/account/login?login=1&password=1234567

            var result = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode)
            {
                await DisplayAlert("ошибка", result, "cancel");
                return;
            }

            var data = JsonConvert.DeserializeObject<AuthResponse>(result);
            MySettings.Token = data.AccessToken;
            MySettings.UserName = data.UserName;
            MySettings.Email = data.Email;
            MySettings.UserId = data.UserId;
            MySettings.Role = data.Role;

            if(MySettings.Role == RoleNames.User)
            {
                await Navigation.PushAsync(new MasterDetailPage1());
            }
            else if(MySettings.Role == RoleNames.Worker)
            {
                await DisplayAlert("a", MySettings.Role, "cancel");
            }
            else if(MySettings.Role == RoleNames.Director)
            {
                await DisplayAlert("a", MySettings.Role, "cancel");
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await DisplayAlert("a", MySettings.Role, "cancel");
        }
    }
}