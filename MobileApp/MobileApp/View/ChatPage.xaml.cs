﻿using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        ChatViewModel viewModel;
        public ChatPage()
        {
            InitializeComponent();
            viewModel = new ChatViewModel();
            BindingContext = viewModel;
        
        }
    }
}