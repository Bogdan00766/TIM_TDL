using MobileApp.Dtos.User;
using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TIM_TDL.MobileApp.Dtos.Job;

namespace MobileApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private List<ReadUpdateJobDto> jobs;
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
        public MainViewModel()
        {
            // Konstruktor domyślny, może pozostać pusty
        }
        public MainViewModel(UserTokenInfoDto tokenInfo)
        {
            userTokenInfo = tokenInfo;
            //GetJobs();
        }

        public async void GetJobs()
        {
            try
            {
                var apiServices = new ApiServices();
                Jobs = await apiServices.GetJobsAsync(userTokenInfo.AccessToken);
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
