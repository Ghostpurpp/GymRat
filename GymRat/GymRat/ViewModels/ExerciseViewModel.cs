using GymRat.Models;
using GymRat.Services;
using GymRat.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GymRat.ViewModels
{
    public class ExerciseViewModel : BaseViewModel
    {
        Session CurrentSession = JsonConvert.DeserializeObject<Session>(Preferences.Get("Session", string.Empty));
        private readonly ExerciseService exerciseService;
        private readonly SessionService sessionService;

        private List<Exercise> exercises;
        public List<Exercise> Exercises
        {
            get => exercises;
            set
            {

                exercises = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddExercise { get; set; }
        public ICommand DeleteSessionBtn { get; set; }
        public ICommand SyncSessionBtn { get; set; }

        public ExerciseViewModel()
        {
            exerciseService = new ExerciseService();
            sessionService = new SessionService();
            LoadExercises(CurrentSession.SessionId);

            AddExercise = new Command(AddNewExercise);
            DeleteSessionBtn = new Command(DeleteSessionAsync);
            SyncSessionBtn = new Command(() => LoadExercises(CurrentSession.SessionId));
        }

        private async void DeleteSessionAsync()
        {
            await App.Current.MainPage.DisplayAlert("Delete Confirmation", "If you delete this Session it will be removed permanently", "Ok", "Cancel");
            await sessionService.DeleteSessionAsync(CurrentSession.SessionId);
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private async void LoadExercises(Guid sessionId)
        {
            Exercises = await exerciseService.GetExercisesAsync(sessionId);
        }

        private async void AddNewExercise()
        {
            await Shell.Current.GoToAsync(nameof(AddExercisePage));
        }
    }
}
