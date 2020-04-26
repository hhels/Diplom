using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplom.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Reviews : ContentPage
    {
        public Reviews()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            MySettings.UserName = "adsdf";

        }


        private async void Button_Clicked_1(object sender, System.EventArgs e)
        {
            await DisplayAlert("a", MySettings.UserName, "cancel");

        }
    }
}