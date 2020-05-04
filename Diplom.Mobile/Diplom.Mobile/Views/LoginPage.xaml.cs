﻿using System;
using System.Net.Http;
using Diplom.Common.Bodies;
using Diplom.Common.Models;
using Diplom.Mobile.Views.DetailMenu;
using Flurl;
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
            var body = new AuthBody()
            {
                Login = loginEntry.Text,
                Password = passwordEntry.Text
            };
            var response = await Constants.Endpoint
                .AppendPathSegments("api", "account", "login") // добавляет к ендпоинт
                .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
                .PostJsonAsync(body);  //  https://localhost:5001/api/account/login?login=1&password=1234567

            var result = await response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<AuthResponse>(result);
                MySettings.Token = data.AccessToken;
                MySettings.UserName = data.UserName;
                MySettings.Email = data.Email;
                MySettings.UserId = data.UserId;
                MySettings.Role = data.Role;

                if(MySettings.Role == "user")
                {
                    await Navigation.PushAsync(new MasterDetailPage1());
                }
                else if(MySettings.Role == "worker")
                {
                    await DisplayAlert("a", MySettings.Role, "cancel");
                }
                else if(MySettings.Role == "director")
                {
                    await DisplayAlert("a", MySettings.Role, "cancel");
                }
            }
            else
            {
                await DisplayAlert("ошибка", result, "cancel");
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