﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage { 

        public RegisterPage()
        {
            InitializeComponent();
        }
        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());


        }
    }
}