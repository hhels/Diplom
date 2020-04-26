using System;
using System.Net.Http;
using Diplom.Common.Bodies;
using Diplom.Common.Models;
using Diplom.Mobile.Views.User;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();

        }


        //var handler = new HttpClientHandler();
        //handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        //handler.ServerCertificateCustomValidationCallback =  HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        //handler.ServerCertificateCustomValidationCallback =
        //(httpRequestMessage, cert, cetChain, policyErrors) =>
        //{
        //    return true;
        //};


        //var client = new HttpClient(handler);
        //var response = await client.GetAsync($"/account/login?username={loginEntry.Text}&password={passwordEntry.Text}");
        //var client = new HttpClient(handler)
        //{
        //   // BaseAddress = new Uri(Constants.Endpoint)
        //};
        //var response = await client.GetAsync($"/api/account/login?username={loginEntry.Text}&password={passwordEntry.Text}");



        //var response = await Constants.Endpoint
        ////var response = await Constants.Endpoint
        ////    .AppendPathSegments("account", "login") // добавляет к ендпоинт
        ////    .SetQueryParam("username", loginEntry.Text) // вводим логин
        ////    .SetQueryParam("password", passwordEntry.Text) // вводим пароль
        ////    .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
        ////    .PostAsync(null);  //  https://localhost:5001/api/account/login?login=1&password=1234567
        //await Navigation.PushAsync(new MasterDetailPage1());


        private async void Button_Clicked(object sender, EventArgs e)
        {
            //var token = "s";
            //var client = new HttpClient()
            //{
            //    BaseAddress = new Uri(Constants.Endpoint)
            //};
            //var response = await client.PostAsync($"/api/account/login?username={loginEntry.Text}&password={passwordEntry.Text}", null);
            var body = new AuthBody()
            {
                Login = loginEntry.Text,
                Password = passwordEntry.Text
            };
            var response = await Constants.Endpoint
                .AppendPathSegments("api", "account", "login") // добавляет к ендпоинт
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
                else if (MySettings.Role == "worker")
                {
                    await DisplayAlert("a", MySettings.Role, "cancel");
                }
                else if (MySettings.Role == "director")
                {
                    await DisplayAlert("a", MySettings.Role, "cancel");
                }
            }

            //var data = await response.Content.ReadAsStringAsync();
            //var token = data("AccessToken");
            // await DisplayAlert("data", data, "cancel");
            //await Navigation.PushAsync(new MasterDetailPage1());
            else
            {
                await DisplayAlert("ошибка", "Не верный логин или пароль", "cancel");
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            //await DisplayAlert("a", MySettings.Token, "cancel");
            await Navigation.PushAsync(new Registration()); 

        }

        private async void  Button_Clicked_2(object sender, EventArgs e)
        {
            await DisplayAlert("a", MySettings.Role, "cancel");
        }
    }
}