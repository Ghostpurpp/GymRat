using GymRat.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymRat.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SessionsPage : ContentPage
    {
        public SessionsPage()
        {
            InitializeComponent();
        }

        private async void SessionList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var list = (ListView)sender;

            if(e.SelectedItem == null)
            {
                return;
            }

            Session session = e.SelectedItem as Session;
            var model = JsonConvert.SerializeObject(session);
            Preferences.Set("Session", model);
            await Shell.Current.GoToAsync(nameof(ExercisePage));
            list.SelectedItem = null;
        }
    }
}