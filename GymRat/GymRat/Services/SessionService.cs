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
    public class SessionService
    {
        private readonly FirebaseClient firebase;

        public SessionService()
        {
           firebase = new FirebaseClient("https://gymrat-d89b1.firebaseio.com/");
        }
        public async Task AddSessionAsync(Session session)
        {
            try
            {
                session.SessionId = Guid.NewGuid();

                var existingSession = (await firebase
                    .Child("Sessions")
                    .OnceAsync<Session>()).Select(item => new Session
                    {
                        SessionId = item.Object.SessionId,
                        SessionDate = item.Object.SessionDate,
                        UserId = item.Object.UserId
                    })
                    .Where(x => x.SessionDate.Date == DateTime.Now.Date)
                    .ToList();

                if (existingSession.Count == 0)
                {
                    await firebase.Child("Sessions").PostAsync(session);
                    await App.Current.MainPage.DisplayAlert("Session Created", null, "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Could not create Session", "Session already exists on this date", "OK");
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Session Error", ex.Message, "OK");
            }
        }

        public async Task<List<Session>> GetSessionsAsync(string userId)
        {
            var sessions = (await firebase
                    .Child("Sessions")
                    .OnceAsync<Session>()).Select(item => new Session { 
                        SessionId = item.Object.SessionId,
                        SessionDate = item.Object.SessionDate,
                        UserId = item.Object.UserId
                    })
                    .Where(x => x.UserId == userId)
                    .ToList();

            if(sessions != null)
            {
                return sessions;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteSessionAsync(Guid sessionId)
        {
            var deleteSession = (await firebase
                    .Child("Sessions")
                    .OnceAsync<Session>())
                    .Where(x => x.Object.SessionId == sessionId).FirstOrDefault();
            await firebase.Child("Sessions").Child(deleteSession.Key).DeleteAsync();

            if (deleteSession.Object != null)
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
