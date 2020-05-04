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
	public partial class AccountEdit : ContentPage
	{
		public AccountEdit ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            //var token = MySettings.Token;
            var UserResponses = await Constants.Endpoint
                            .AppendPathSegments("api", "account", "userGet") // добавляет к ендпоинт
                            .WithOAuthBearerToken(MySettings.Token) //передача токена
                            .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
                            .GetJsonAsync<UserResponse>();  //  https://192.168.1.12:5002/api/account/userGet

            loginEntry.Placeholder = UserResponses.Login;
            firstNameEntry.Placeholder = UserResponses.FirstName;
            lastNameEntry.Placeholder = UserResponses.LastName;
            yearsEntry.Text = UserResponses.Year.ToString();
            emailEntry.Text = UserResponses.Email;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            var body = new UserResponse()
            {
                Login = loginEntry.Text,
                Email = emailEntry.Text,
                FirstName = firstNameEntry.Text,
                LastName = lastNameEntry.Text,
                Year = Convert.ToInt32(yearsEntry.Text),
            };

            var response = await Constants.Endpoint
               .AppendPathSegments("api", "account", "userEdit") // добавляет к ендпоинт
               .AllowAnyHttpStatus() // если сервер вернет не положительный ответ, то исключение не выпадет
               .PostJsonAsync(body);  //  https://192.168.1.12:5002/api/account/userEdit

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<UserResponse>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                await DisplayAlert("ошибка", "Что то пошло не так при обновлении", "cancel");
            }
        }
    }
}