using Diplom.Mobile.Views;
using Diplom.Mobile.Views.DetailMenu;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Diplom.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if(string.IsNullOrWhiteSpace(MySettings.Token))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new MasterDetailPage1());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
