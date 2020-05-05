using System;
using System.Linq;
using Diplom.Common.Entities;
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

            using(var db = new ApplicationContext())
            {
                // Создаем бд, если она отсутствует
                db.Database.EnsureCreated();
                if(!db.Review.Any())
                {
                    db.Review.Add(new Review
                    {
                        Text = "пывапьыждвьапждфвьапдфвпждапь", Rating = 4, Date = DateTime.Parse("11.11.2001"), UserId = "sssss"
                    });
                    db.Review.Add(new Review
                    {
                        Text = "qweqweqweqweqweqweqweqweqweqwe", Rating = 5, Date = DateTime.Parse("09.09.2009"), UserId = "rrrrr"
                    });
                    db.SaveChanges();
                }
            }

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