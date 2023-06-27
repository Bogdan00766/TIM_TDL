using MobileApp.Dtos.User;
using MobileApp.ViewModels;
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
    public partial class MainPage : ContentPage
    {
        private MainViewModel viewModel;
        public MainPage(UserTokenInfoDto userTokenInfo)
        {
            InitializeComponent();
            viewModel = new MainViewModel(userTokenInfo);
            BindingContext = viewModel;
            viewModel.GetJobs();
            //this.userTokenInfo.Email = userTokenInfo.Email;
            //  this.userTokenInfo.AccessToken = userTokenInfo.AccessToken;
            // this.userTokenInfo.RefreshToken = userTokenInfo.RefreshToken;
        }
    }
}