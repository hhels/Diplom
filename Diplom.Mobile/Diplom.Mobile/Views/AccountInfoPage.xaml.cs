using System;
using Diplom.Common.Models;
using Flurl.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountInfoPage : ContentPage
    {
        public AccountInfoPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            // если нет подключение к интернету
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("ошибка", "Отсутствует подключение к интернету", "cancel");
                return;
            }
            var userResponses = await RequestBuilder.Create()
                                                    .AppendPathSegments("api", "account", "userGet") // добавляет к ендпоинт
                                                    .GetJsonAsync<UserResponse>(); //  https://192.168.1.12:5002/api/account/userGet
            var rus = 1;
            if (userResponses.Sex == SexType.Male)
            {
                rus = 0;
            }
            else if (userResponses.Sex == SexType.Female)
            {
                rus = 1;
            }
            loginEntry.Text = userResponses.Login;
            firstNameEntry.Text = userResponses.FirstName;
            lastNameEntry.Text = userResponses.LastName;
            yearsEntry.Text = userResponses.Year.ToString();
            emailEntry.Text = userResponses.Email;
            telefonEntry.Text = userResponses.PhoneNumber;
            picker.SelectedIndex = rus;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // если нет подключение к интернету
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("ошибка", "Отсутствует подключение к интернету", "cancel");
                return;
            }
            var rus = SexType.Male;
            if (picker.SelectedIndex == 0)
            {
                rus = SexType.Male;
            }
            else if (picker.SelectedIndex == 1)
            {
                rus = SexType.Female;
            }
            var body = new UserResponse
            {
                Login = loginEntry.Text,
                Email = emailEntry.Text,
                FirstName = firstNameEntry.Text,
                LastName = lastNameEntry.Text,
                Year = Convert.ToInt32(yearsEntry.Text),
                PhoneNumber = telefonEntry.Text,
                Sex = rus
            };

            var response = await RequestBuilder.Create()
                                               .AppendPathSegments("api", "account", "userEdit") // добавляет к ендпоинт
                                               .PostJsonAsync(body); //  https://192.168.1.12:5002/api/account/userEdit

            if(!response.IsSuccessStatusCode)
            {
                await DisplayAlert("ошибка", "Что-то пошло не так при обновлении", "cancel");
                return;
            }

            //заносим данные в модель
            var data = JsonConvert.DeserializeObject<UserResponse>(await response.Content.ReadAsStringAsync());

            var russ = 1;
            if (data.Sex == SexType.Male)
            {
                russ = 0;
            }
            else if (data.Sex == SexType.Female)
            {
                russ = 1;
            }
            //обновляем данные на странице
            loginEntry.Text = data.Login;
            firstNameEntry.Text = data.FirstName;
            lastNameEntry.Text = data.LastName;
            yearsEntry.Text = data.Year.ToString();
            emailEntry.Text = data.Email;
            telefonEntry.Text = data.PhoneNumber;
            picker.SelectedIndex = russ;
            await DisplayAlert("ОК", "Данные успешно обновлены", "cancel");
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }
    }
}