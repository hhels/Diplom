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
    public partial class EditAccountPage : ContentPage
    {
        public EditAccountPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            var UserResponses = await RequestBuilder.Create()
                            .AppendPathSegments("api", "account", "userGet") // добавляет к ендпоинт
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

            var response = await RequestBuilder.Create()
               .AppendPathSegments("api", "account", "userEdit") // добавляет к ендпоинт
               .PostJsonAsync(body);  //  https://192.168.1.12:5002/api/account/userEdit

            if(response.IsSuccessStatusCode)
            {
                //TODO: шо это такое?
                var data = JsonConvert.DeserializeObject<UserResponse>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                await DisplayAlert("ошибка", "Что то пошло не так при обновлении", "cancel");
            }
        }
    }
}