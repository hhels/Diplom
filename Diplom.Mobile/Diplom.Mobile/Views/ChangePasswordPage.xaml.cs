using Diplom.Common.Models;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
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
    public partial class ChangePasswordPage : ContentPage
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var password = passwordEntry.Text;
            var newPassword = passworAgaindEntry.Text;
            if(password == newPassword)
            {
                await DisplayAlert("ошибка", "Новый пароль не должен быть равен старому", "cancel");
                return;
            }
            else if(newPassword.Length <= 6)
            {
                await DisplayAlert("ошибка", "Пароль должен быть не меньше 6 символов", "cancel");
                return;
            }
            else
            {
                var response = await Constants.Endpoint
                                              .AppendPathSegments("api", "account", "PasswordEdit") // добавляет к ендпоинт
                                              .SetQueryParam("password", passwordEntry.Text) // вводим логин
                                              .SetQueryParam("newPassword", passworAgaindEntry.Text) // вводим пароль
                                              .WithOAuthBearerToken(MySettings.Token) //передача токена
                                              .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
                                              .PostAsync(null);  //  https://192.168.1.12:5002/api/account/PasswordEdit
                if(response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync());
                    await DisplayAlert("ОК", "Пароль успешно обновлен", "cancel");
                    await DisplayAlert("ОК", data.AccessToken, "cancel");
                }
                else
                {
                    var datta = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("ОК", datta, "cancel");
                }
            }
        }
    }
}