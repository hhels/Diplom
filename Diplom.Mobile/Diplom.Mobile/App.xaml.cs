using Diplom.Common.Entities;
using Diplom.Mobile.Views;
using Diplom.Mobile.Views.DetailMenu;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Diplom.Mobile
{
    public partial class App : Application
    {
        public const string DBFILENAME = "clientapp.db";
        public App()
        {
            InitializeComponent();

            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            using (var db = new ApplicationContext(dbPath))
            {
                // Создаем бд, если она отсутствует
                db.Database.EnsureCreated();
                if (db.Review.Count() == 0)
                {
                   // string dateInput = "Jan 1, 2001";
                    //string dateInputt = "Jan 5, 2005";
                    db.Review.Add(new Review { Text = "пывапьыждвьапждфвьапдфвпждапь", Rating = 4,  Date = DateTime.Parse("11.11.2001"), UserId = "sssss" });
                    db.Review.Add(new Review { Text = "qweqweqweqweqweqweqweqweqweqwe", Rating = 5, Date = DateTime.Parse("09.09.2009"), UserId = "rrrrr" });
                    db.SaveChanges();
                }
            }

            if (string.IsNullOrWhiteSpace(MySettings.Token))
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
