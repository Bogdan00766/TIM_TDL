using MobileApp.Utils;
using MobileApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {//task
            //HttpClient client= new HttpClient();

            //var response = await client.PostAsync(Config.Data.ApiUrl + "/api/login/",) //configi do zmiany api w jednym miejscu a nie - nazwać buttony i odpowiadajace metody nazwa buttona - LoginButton
            DisplayAlert("Login", "Successful login", "Ok");
            Navigation.PushAsync(new MainPage());
        }
    }
}
