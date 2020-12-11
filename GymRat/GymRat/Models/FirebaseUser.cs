using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GymRat.Models
{
    public class FirebaseUser 
    {
        public FirebaseAuthLink Refresh { get; set; }
        public string FirebaseEmail { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string DisplayPhoto { get; set; }
    }
}
