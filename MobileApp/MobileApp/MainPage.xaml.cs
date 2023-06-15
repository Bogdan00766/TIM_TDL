using MobileApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async Task Button_ClickedAsync(object sender, EventArgs e)
        {
            HttpClient client= new HttpClient();

            //var response = await client.PostAsync(Config.Data.ApiUrl + "/api/login/",) //configi do zmiany api w jednym miejscu a nie - nazwać buttony i odpowiadajace metody nazwa buttona - LoginButton
            await DisplayAlert("Login", "Successful login", "Ok");
            await Navigation.PushAsync(new HomePage(UsernameEntry.Text));
        }
    }
}
