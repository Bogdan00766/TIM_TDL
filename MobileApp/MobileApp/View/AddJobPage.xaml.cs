using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.MobileApp.Dtos.Job;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddJobPage : ContentPage
    {
        
        public AddJobPage()
        {
            InitializeComponent();
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            await Navigation.PushAsync(new MainPage());
        }
    }
}