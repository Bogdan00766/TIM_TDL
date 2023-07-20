using MobileApp.Dtos.User;
using MobileApp.Services;
using MobileApp.Utils;
using MobileApp.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TIM_TDL.MobileApp.Dtos.User;
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
                    var response = await _apiServices.LoginAsync(Email, Password);// ToDo napraw to bo to debug xDD
                    //var response = await _apiServices.LoginAsync("string", "string"); // ToDo No weź powyżej patrz xDD
                    if (response.IsSuccessStatusCode)
                    {
                        //TODO _Logger.Information("User of id: {id} and email: {email} added sucessfully", Email, Password);
                        await App.Current.MainPage.DisplayAlert("Success", "Login successfully", "OK");
                        var content = await response.Content.ReadAsStringAsync();
                        var userTokenInfo = JsonConvert.DeserializeObject<UserTokenInfoDto>(content);

                        CurrentUser.AccessToken = userTokenInfo.AccessToken;
                        CurrentUser.RefreshToken = userTokenInfo.RefreshToken;
                        CurrentUser.Email = Email;
                        //await Shell.Current.GoToAsync("//MainPage");
                        await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                        //Application.Current.MainPage = new AppShell();
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
