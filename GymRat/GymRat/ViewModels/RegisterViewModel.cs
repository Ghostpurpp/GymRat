using GymRat.Models;
using GymRat.Services;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace GymRat.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string pictureFile;
        public string PictureFile
        {
            get => pictureFile;
            set
            {
                pictureFile = value;
                OnPropertyChanged();
            }
        }

        private string displayName;
        public string DisplayName
        {
            get => displayName;
            set
            {
                displayName = value;
                OnPropertyChanged();
            }
        }

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


        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterBtn { get; set; }
        public ICommand LoginBtn { get; }
        public ICommand GetImageFile { get; set; }

        public RegisterViewModel()
        {
            RegisterBtn = new Command(RegisterFirebaseUser);
            LoginBtn = new Command(LoginUser);
            GetImageFile = new Command(GetImage);
        }

        private async void LoginUser()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private async void RegisterFirebaseUser()
        {
            FirebaseUserService userService = new FirebaseUserService();
            if(Password == ConfirmPassword)
            {
                var model = new FirebaseUser { DisplayName = DisplayName, FirebaseEmail = Email, Password = Password, DisplayPhoto = PictureFile };
                await userService.SignUpAsync(model);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Registration Error", "Password does not Match", "OK");
            }
        }

        private async void GetImage()
        {
            var result = await FilePicker.PickAsync(new PickOptions { FileTypes = FilePickerFileType.Images, PickerTitle = "Pick an Image"});
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                var imageStream = ImageSource.FromStream(() => stream);
                var str = JsonConvert.SerializeObject(imageStream);
                PictureFile = str;
            }
        }
    }
}
