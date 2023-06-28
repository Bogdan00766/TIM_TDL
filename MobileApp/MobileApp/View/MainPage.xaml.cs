using MobileApp.Dtos.User;
using MobileApp.ViewModels;
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
    public partial class MainPage : ContentPage
    {
        
        private MainViewModel viewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            BindingContext = viewModel;
            viewModel.GetJobs();
            ToolbarItems.Add(new ToolbarItem("💬", "Chat.png", async () =>
            {
                await Navigation.PushAsync(new ChatPage());
            }));
            ToolbarItems.Add(new ToolbarItem("🗑", "Delete.png", async () =>
            {
                if (viewModel.Job == null)
                {
                    await DisplayAlert("Usuwanie", "Najpierw wybierz element", "OK");
                    return;
                }
                var result = await DisplayAlert("Usuwanie", "Czy na pewno chcesz usunać?", "Tak", "Nie");
                if (result)
                {
                    await viewModel.DeleteJobAsync(viewModel.Job);
                }
            }));
            ToolbarItems.Add(new ToolbarItem("✎", "Edit.png", async () =>
            {
                if (viewModel.Job == null)
                {
                    await DisplayAlert("Edycja", "Najpierw wybierz element", "OK");
                    return;
                }
                
                await Navigation.PushAsync(new AddJobPage(viewModel.Job));
            }));
            ToolbarItems.Add(new ToolbarItem("+", "Add.png", async () =>
            {
                await Navigation.PushAsync(new AddJobPage());
            }));
            

            
            //this.userTokenInfo.Email = userTokenInfo.Email;
            //  this.userTokenInfo.AccessToken = userTokenInfo.AccessToken;
            // this.userTokenInfo.RefreshToken = userTokenInfo.RefreshToken;
        }
    }
}