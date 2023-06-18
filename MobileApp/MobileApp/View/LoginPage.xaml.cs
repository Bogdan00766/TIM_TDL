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
    public partial class LoginPage : ContentPage
    {
        private INavigation navigation;
        public LoginPage()
        {
            InitializeComponent();
            navigation = Navigation;
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            await navigation.PushAsync(new NavigationPage(new RegisterPage()));
        }
    }
}