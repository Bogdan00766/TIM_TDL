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
        public ReadUpdateJobDto Job { get; set; }
        public bool IsCreateMode { get; set; }
        public bool IsUpdateMode { get; set; }

        public AddJobPage(ReadUpdateJobDto job = null)
        {

            Job = job;
            if(job == null) 
            {
                IsCreateMode = true; 
                IsUpdateMode = false;
            }
            else
            {
                IsCreateMode = false;
                IsUpdateMode = true;
            }
            InitializeComponent();
            JobDate.Date = DateTime.Now.Date;
            if (Job != null)
            {
                JobName.Text = Job.Name;
                JobDesc.Text = Job.Description;
                JobDate.Date = Job.DueDate.Date;
                JobStatus.SelectedItem = Job.Status;
                JobId.Text = Job.Id.ToString();
            }
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            await Navigation.PushAsync(new MainPage());
        }
    }
}