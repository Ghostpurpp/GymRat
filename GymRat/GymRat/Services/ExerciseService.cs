using Firebase.Database;
using Firebase.Database.Query;
using GymRat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRat.Services
{
    public class ExerciseService
    {
        private readonly FirebaseClient firebase;

        public ExerciseService()
        {
            firebase = new FirebaseClient("https://gymrat-d89b1.firebaseio.com/");
        }

        public async Task AddExerciseAsync(Exercise exercise)
        {
            exercise.ExerciseId = Guid.NewGuid();
            await firebase.Child("Exercises").PostAsync(exercise);
        }
        public async Task<List<Exercise>> GetExercisesAsync(Guid sessionId)
        {
            var exercises = (await firebase
                    .Child("Exercises")
                    .OnceAsync<Exercise>()).Select(item => new Exercise
                    {
                        SessionId = item.Object.SessionId,
                        ExerciseId = item.Object.ExerciseId,
                        Name = item.Object.Name,
                        Reps = item.Object.Reps,
                        Sets = item.Object.Sets,
                        Duration = item.Object.Duration
                    })
                    .Where(x => x.SessionId == sessionId)
                    .ToList();

            if (exercises != null)
            {
                return exercises;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateExerciseAsync(Exercise exercise)
        {
            var updateExercise = (await firebase
                    .Child("Exercises")
                    .OnceAsync<Exercise>())
                    .Where(x => x.Object.SessionId == exercise.SessionId).FirstOrDefault();
            await firebase.Child("Exercises").Child(updateExercise.Key).PutAsync(exercise);

            if (updateExercise.Object != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteExerciseAsync(Guid sessionId)
        {
            var deleteExercise = (await firebase
                    .Child("Exercises")
                    .OnceAsync<Exercise>())
                    .Where(x => x.Object.SessionId == sessionId).FirstOrDefault();
            await firebase.Child("Exercises").Child(deleteExercise.Key).DeleteAsync();

            if (deleteExercise.Object != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
