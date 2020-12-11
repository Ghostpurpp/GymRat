using GymRat.Models;
using GymRat.Services;
using GymRat.Views;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Windows.Input;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GymRat.ViewModels
{
    public class LogInViewModel : BaseViewModel
    {
        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public ICommand FacebookLogIn { get; set; }
        public ICommand FirebaseLogIn { get; set; }
        public ICommand RegisterBtn { get; }

        public LogInViewModel()
        {
            FacebookLogIn = new Command(FacebookUserLogIn);
            FirebaseLogIn = new Command(FirebaseUserLogIn);
            RegisterBtn = new Command(RegisterUser);
        }

        private async void RegisterUser()
        {
            await App.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        private async void FirebaseUserLogIn()
        {
            FirebaseUserService userService = new FirebaseUserService();

            var model = new FirebaseUser { FirebaseEmail = Email, Password = Password };
            await userService.LogInAsync(model);
            App.Current.MainPage = new AppShell();
        }

        private async void FacebookUserLogIn()
        {
            try
            {
                string cliendId = string.Empty;
                string redirectUri = string.Empty;
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        redirectUri = Constants.FacebookiOSRedirectUrl;
                        break;

                    case Device.Android:
                        cliendId = Constants.FacebookAndroidClientId;
                        redirectUri = Constants.FacebookAndroidRedirectUrl;
                        break;
                }

                var Authenticator = new OAuth2Authenticator(
                    cliendId,
                    Constants.FacebookScope,
                    new Uri(Constants.FacebookAuthorizeUrl),
                    new Uri(Constants.FacebookAndroidRedirectUrl),
                    null,
                    false);

                var Presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                Presenter.Login(Authenticator);
                Authenticator.Completed += Authenticator_Completed;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            }
        }

        private void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                string accessToken = e.Account.Properties["access_token"];
                FacebookUserProfileAsync(accessToken);
            }
        }

        private void FacebookUserProfileAsync(string accessToken)
        {
            HttpClient client = new HttpClient();
            var response = client.GetStringAsync($"https://graph.facebook.com/me?fields=email,name,picture.width(500).height(500)&access_token={accessToken}").Result;
            var content = JsonConvert.DeserializeObject<FacebookUser>(response);
            content.LogInMethod = "Facebook";
            Preferences.Set("AppUser", JsonConvert.SerializeObject(content));
            App.Current.MainPage = new AppShell();
        }
    }
}
