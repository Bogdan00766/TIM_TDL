using AutoMapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Application.Dtos.Job;
using TIM_TDL.Application.Dtos.User;
using TIM_TDL.Application.IServices;
using TIM_TDL.Domain.IRepositories;
using TIM_TDL.Domain.Models;
using TIM_TDL.Domain.Utils;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using System.Net.Mail;

namespace TIM_TDL.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IUserRepository _UserRepository;
        private readonly IJobRepository _JobRepository;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public JobService(IUserRepository UserRepository, IJobRepository JobRepository, IMapper Mapper, ILogger Logger)
        {
            _UserRepository = UserRepository;
            _JobRepository = JobRepository;
            _Mapper = Mapper;
            _Logger = Logger.ForContext<JobService>();
        }
        public async Task<NewJobDto> AddJobAsync(CreateJobDto dto)
        {
            var user = await _UserRepository.FindByIdAsync(dto.UserId);

            var job = new Job
            {
                Name = dto.Name,
                Description = dto.Description,
                CreationDate = DateTime.Now,
                DueDate = dto.DueDate,
                Status = dto.Status,
                Owner = user
            };
            //zwracać OneOf DTO i sprawdzanie istniejacego usera - funkcja rejestracji 
            _Logger.Information("User of id: {id} and email: {email} added job of id: {idJob} and name: {name}", user.Id, user.Email, job.Id, job.Name);
            _JobRepository.Create(job);
            await _JobRepository.SaveAsync();

            var sendGridApiKey = "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";
            var client = new SendGridClient(sendGridApiKey);
            var fromEmail = new EmailAddress("studentwyklad@gmail.com", "Todolist");
            var toEmail = new EmailAddress(user.Email, "Recipient Name");
            var subject = "New task has been added";
            var content = $"New task named:  '{job.Name}' has been added. <br> Click <a href=\"http://localhost:4200/dashboard\">here</a> to go to the website.";

            var message = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, content, content);
            var response = await client.SendEmailAsync(message);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                // Obsługa błędu wysyłki wiadomości
                _Logger.Error("Błąd podczas wysyłania wiadomości e-mail: {statusCode}", response.StatusCode);
                // Można tu zaimplementować dodatkową logikę w przypadku niepowodzenia wysyłki wiadomości
            }



            return _Mapper.Map<NewJobDto>(job);
        }
    }
}
