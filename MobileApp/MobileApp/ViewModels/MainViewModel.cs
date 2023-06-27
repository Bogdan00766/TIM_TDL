using MobileApp.Dtos.User;
using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TIM_TDL.MobileApp.Dtos.Job;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private List<ReadUpdateJobDto> jobs;
        private readonly ApiServices _ApiServices;
        private UserTokenInfoDto userTokenInfo;

        public List<ReadUpdateJobDto> Jobs
        {
            get { return jobs; }
            set
            {
                jobs = value;
                NotifyPropertyChanged();
            }
        }
       // public MainViewModel()
        //{
            // Konstruktor domyślny, może pozostać pusty
        //}
        public MainViewModel()
        {
            _ApiServices = new ApiServices();
            AddJobCommand = new Command<CreateJobDto>(AddJobAsync);
            
            //GetJobs();
        }

        public CreateJobDto NewJob { get; set; } = new CreateJobDto
        {
            Description = "Description",
            DueDate = DateTime.Now,
            Name = "Name",
            Status = TIM_TDL.Domain.Utils.DataTypes.JobStatus.InProgress
        };


        private async void AddJobAsync(CreateJobDto dto)
        {
            try
            {
                var newJob = await _ApiServices.AddJobAsync(dto);
                Jobs.Add(newJob);
                NotifyPropertyChanged();
            }
            catch (Exception)
            {

            }
        }

        public ICommand AddJobCommand { get; }

        public async void GetJobs()
        {
            try
            {
                Jobs = await _ApiServices.GetJobsAsync();
            }
            catch (Exception ex)
            {
                // Handle the error appropriately
                throw;
            }
        }

        // Implement the INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
