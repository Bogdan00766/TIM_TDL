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
    public partial class MainPage : ContentPage
    {
        private string email;
        public MainPage(string email)
        {
            InitializeComponent();
            this.email = email; 
            EmailLabel.Text ="Hello " + email;
        }
    }
}