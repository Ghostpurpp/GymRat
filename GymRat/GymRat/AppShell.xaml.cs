using System;
using System.Collections.Generic;
using Firebase.Auth;
using GymRat.Models;
using GymRat.Services;
using GymRat.Views;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GymRat
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username)); // Notify that there was a change on this property
            }
        }

        private string profilePic;
        public string ProfilePic
        {
            get { return profilePic; }
            set
            {
                profilePic = value;
                OnPropertyChanged(nameof(ProfilePic)); // Notify that there was a change on this property
            }
        }

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SessionsPage), typeof(SessionsPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ExercisePage), typeof(ExercisePage));
            Routing.RegisterRoute(nameof(AddExercisePage), typeof(AddExercisePage));

            BindingContext = this;

            var userString = Preferences.Get("AppUser", string.Empty);

            if (JsonConvert.DeserializeObject<FacebookUser>(userString).Id != null)
            {
                var user = JsonConvert.DeserializeObject<FacebookUser>(userString);

                Username = user.Name;
                ProfilePic = user.Picture.Data.Url;

                Preferences.Set("UserId", user.Id);
            }
            else if (JsonConvert.DeserializeObject<FirebaseAuth>(userString) != null)
            {
                var user = JsonConvert.DeserializeObject<FirebaseAuth>(userString);

                Username = user.User.DisplayName;
                ProfilePic = user.User.PhotoUrl ?? "";
                RefreshUser(userString);

                Preferences.Set("UserId", user.User.LocalId);
            }
        }

        private async void RefreshUser(string userString)
        {
            FirebaseUserService userService = new FirebaseUserService();
            await userService.GetProfileInfoAndRefreshToken(userString);
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("AppUser", null);
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.CurrentItem = new LoginPage();
        }
    }
}
