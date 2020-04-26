using Diplom.Mobile.Views;
using Diplom.Mobile.Views.User;
using Flurl.Http;
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

             if (string.IsNullOrWhiteSpace(MySettings.Token))
            {
                MainPage = new NavigationPage(new Login());
            }
            else
            {
                MainPage =  new NavigationPage(new MasterDetailPage1());
            }
            new NavigationPage(new Login());
            //MainPage = new NavigationPage(new Account());
            //FlurlHttp.ConfigureClient("https://192.168.1.12:5002", cli =>
            //cli.Settings.HttpClientFactory = new UntrustedCertClientFactory());
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
