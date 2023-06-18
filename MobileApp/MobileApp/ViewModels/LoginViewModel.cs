using MobileApp.Services;
using MobileApp.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    internal class LoginViewModel
    {
        ApiServices _apiServices = new ApiServices();
        public string Email { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        private INavigation Navigation;
        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var isSuccess = await _apiServices.LoginAsync(Email, Password);

                    if (isSuccess)
                    {
                        //TODO _Logger.Information("User of id: {id} and email: {email} added sucessfully", Email, Password);
                        await App.Current.MainPage.DisplayAlert("Success", "Login successfully", "OK");
                        await Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Failed", "Failed to login", "OK");
                    }
                });
            }
            set
            {

            }
        }
    }
}
