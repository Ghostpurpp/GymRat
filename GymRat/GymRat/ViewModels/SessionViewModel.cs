using GymRat.Models;
using GymRat.Services;
using GymRat.Views;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GymRat.ViewModels
{
    public class SessionViewModel : BaseViewModel
    {
        private readonly SessionService sessionService;
        
        private List<Session> sessions;
        public List<Session> Sessions
        {
            get => sessions;
            set
            {
                sessions = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddSessionBtn { get; set; }
        public ICommand SyncSessionBtn { get; set; }

        public SessionViewModel()
        {
            sessionService = new SessionService();
            LoadSessions();
            AddSessionBtn = new Command(AddSession);
            SyncSessionBtn = new Command(LoadSessions);
        }

        private async void AddSession()
        {
            Session session = new Session
            {
                UserId = Preferences.Get("UserId", string.Empty),
                SessionDate = DateTime.Now
            };

            await sessionService.AddSessionAsync(session);

            LoadSessions();
        }

        private async void LoadSessions()
        {
            string userId = Preferences.Get("UserId", string.Empty);

            Sessions = await sessionService.GetSessionsAsync(userId);

            if (Sessions == null)
            {
                await App.Current.MainPage.DisplayAlert("No Sessions", "You don't have gym sessions", "OK");
            }
        }
    }
}
