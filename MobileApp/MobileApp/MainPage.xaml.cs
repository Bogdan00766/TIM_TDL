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

        private void Button_Clicked(object sender, EventArgs e)
        {//task
            //HttpClient client= new HttpClient();

            //var response = await client.PostAsync(Config.Data.ApiUrl + "/api/login/",) //configi do zmiany api w jednym miejscu a nie - nazwać buttony i odpowiadajace metody nazwa buttona - LoginButton
            DisplayAlert("Login", "Successful login", "Ok");
            Navigation.PushAsync(new HomePage(UsernameEntry.Text));
        }
    }
}
