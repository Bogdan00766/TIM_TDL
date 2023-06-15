using MobileApp.Utils;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    public partial class App : Application
    {
        public App()
        {
            Config.Initialize();
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
