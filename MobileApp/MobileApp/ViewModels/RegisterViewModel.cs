using MobileApp.Services;
using MobileApp.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    internal class RegisterViewModel
    {
        ApiServices _apiServices = new ApiServices();
        public string Email { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public ICommand RegisterCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var isSuccess = await _apiServices.RegisterAsync(Email, Password);

                    if (isSuccess)
                    {
                        //TODO _Logger.Information("User of id: {id} and email: {email} added sucessfully", Email, Password);
                        await App.Current.MainPage.DisplayAlert("Success", "Registered successfully", "OK");
                        await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        Message = "Retry later";
                    }
                });
            }
            set
            {

            }
        }
    }
}
