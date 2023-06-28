using MobileApp.Dtos.User;
using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public ReadUpdateJobDto Job { get; set; }
        private List<ReadUpdateJobDto> jobs;
        private readonly ApiServices _ApiServices;
        private UserTokenInfoDto userTokenInfo;
        public ReadUpdateJobDto NewJob { get; set; } = new ReadUpdateJobDto();
        public string SelectedStatus { 
            get 
            {
                return " ";
            }
            set 
            {
                if (value == null) return;
                if (value == "Done")
                    NewJob.Status = TIM_TDL.Domain.Utils.DataTypes.JobStatus.Done;
                if (value == "InProgress")
                    NewJob.Status = TIM_TDL.Domain.Utils.DataTypes.JobStatus.InProgress;
                if (value == "Planned")
                    NewJob.Status = TIM_TDL.Domain.Utils.DataTypes.JobStatus.Planned;
            } 
        }
        public IList<string> Statuses { 
            get
            {
                return new List<string> { "Done", "InProgress", "Planned"};
            }
        }
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
            AddJobCommand = new Command<ReadUpdateJobDto>(AddJobAsync);
            
            //GetJobs();
        }

       


        private async void AddJobAsync(ReadUpdateJobDto dto)
        {
            
            if (dto.Id > 0)
            {
                try
                {
                    var newJob = await _ApiServices.UpdateJobAsync(dto);
                    Jobs.Remove(NewJob);
                    Jobs.Add(newJob);
                    NotifyPropertyChanged();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                
                try
                {
                    CreateJobDto createJobDto = new CreateJobDto
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        DueDate = dto.DueDate,
                        Status = dto.Status,
                    };
                    var newJob = await _ApiServices.AddJobAsync(createJobDto);
                    Jobs.Add(newJob);
                    NotifyPropertyChanged();
                }
                catch (Exception)
                {

                }
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

        internal async Task DeleteJobAsync(ReadUpdateJobDto job)
        {
            try
            {
                await _ApiServices.DeleteJobAsync(job);
                var element = Jobs.Where(x => x.Id == job.Id).FirstOrDefault();
                Jobs.Remove(element);
                await App.Current.MainPage.DisplayAlert("Sukces", "Usunięto pomyslnie", "OK");
                GetJobs();
            }
            catch (Exception ex)
            {
                // Handle the error appropriately
                throw;
            }
        }
    }
}
