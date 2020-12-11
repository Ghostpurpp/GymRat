using Firebase.Auth;
using GymRat.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GymRat.Services
{
    public class FirebaseUserService
    {
        private readonly string WebApiKey = "AIzaSyDZUCssfOazm6OC8XSY5TY2OSWm3KQ_E5E";

        public async Task SignUpAsync(FirebaseUser user)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            await authProvider.CreateUserWithEmailAndPasswordAsync(user.FirebaseEmail, user.Password, user.DisplayName);
            await App.Current.MainPage.Navigation.PopAsync();
        }

        public async Task LogInAsync(FirebaseUser user)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));

            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(user.FirebaseEmail, user.Password);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("AppUser", serializedContent);
                App.Current.MainPage = new AppShell();
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Log In Error", ex.Message, "OK");
            }
        }

        public async Task GetProfileInfoAndRefreshToken(string userString)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));

            try
            {
                var savedFirebaseAuth = JsonConvert.DeserializeObject<FirebaseAuth>(userString);
                var refreshContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                Preferences.Set("AppUser", JsonConvert.SerializeObject(savedFirebaseAuth));
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Refresh Error", ex.Message, "OK");
            }
        }
    }
}
