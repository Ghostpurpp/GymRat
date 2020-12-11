using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GymRat.Views;
using Xamarin.Essentials;

namespace GymRat
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(Preferences.Get("AppUser", string.Empty)))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new AppShell();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
