using GymRat.Models;
using GymRat.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GymRat.ViewModels
{
    public class AddExerciseViewModel : BaseViewModel
    {
        Session CurrentSession = JsonConvert.DeserializeObject<Session>(Preferences.Get("Session", string.Empty));
        private readonly ExerciseService exerciseService;

        private string exerciseName;
        public string ExerciseName
        {
            get => exerciseName;
            set
            {
                exerciseName = value;
                OnPropertyChanged();
            }
        }

        private int exerciseReps;
        public int ExerciseReps
        {
            get => exerciseReps;
            set
            {
                exerciseReps = value;
                OnPropertyChanged();
            }
        }

        private int exerciseSets;
        public int ExerciseSets
        {
            get => exerciseSets;
            set
            {
                exerciseSets = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan duration;
        public TimeSpan Duration
        {
            get => duration;
            set
            {
                duration = value;
                OnPropertyChanged();
            }
        }

        private int hours;
        public int Hours
        {
            get => hours;
            set
            {
                hours = value;
                OnPropertyChanged();
            }
        }

        private int minutes;
        public int Minutes
        {
            get => minutes;
            set
            {
                minutes = value;
                OnPropertyChanged();
            }
        }

        private int seconds;
        public int Seconds
        {
            get => seconds;
            set
            {
                seconds = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddNewExercise { get; set; }

        public AddExerciseViewModel()
        {
            exerciseService = new ExerciseService();
            AddNewExercise = new Command(AddExercise);
        }

        private async void AddExercise()
        {
            try
            {
                Exercise model = new Exercise
                {
                    SessionId = CurrentSession.SessionId,
                    Name = ExerciseName,
                    Reps = ExerciseReps,
                    Sets = ExerciseSets,
                    Duration = new TimeSpan(Hours, Minutes, Seconds)
                };

                await exerciseService.AddExerciseAsync(model);
                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error Adding Exercise", ex.Message, "OK");
            }
        }
    }
}
