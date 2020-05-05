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
            if(newPassword.Length <= 6)
            {
                await DisplayAlert("ошибка", "Пароль должен быть не меньше 6 символов", "cancel");
                return;
            }
            var response = await RequestBuilder.Create()
                                            .AppendPathSegments("api", "account", "PasswordEdit") // добавляет к ендпоинт
                                            .SetQueryParam("password", passwordEntry.Text) // вводим логин
                                            .SetQueryParam("newPassword", passworAgaindEntry.Text) // вводим пароль
                                            .PostAsync(null);  //  https://192.168.1.12:5002/api/account/PasswordEdit
            var data = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject<AuthResponse>(data);
                await DisplayAlert("ОК", "Пароль успешно обновлен", "cancel");
                await DisplayAlert("ОК", json.AccessToken, "cancel");
            }
            else
            {
                await DisplayAlert("ОК", data, "cancel");
            }
        }
    }
}