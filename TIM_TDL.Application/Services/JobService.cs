using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Application.Dtos.Job;
using TIM_TDL.Application.Dtos.User;
using TIM_TDL.Application.IServices;
using TIM_TDL.Application.Utilities;
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
        public async Task<NewJobDto> CreateJobAsync(CreateJobDto dto, HttpContext context)
        {
            var user = await _UserRepository.FindByIdAsync(TokenUtilities.GetUserIdFromClaims(context));

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
        public List<ReadUpdateJobDto> ReadJob(HttpContext context)
        {
            var jobsList = _JobRepository.GetAllUserJobs(TokenUtilities.GetUserIdFromClaims(context));

            return _Mapper.Map<List<ReadUpdateJobDto>>(jobsList);
           
        }
        public async Task<OneOf<ReadUpdateJobDto,No, Error>> UpdateJobAsync(ReadUpdateJobDto dto, HttpContext context)
        {
            var user = await _UserRepository.FindByIdAsync(TokenUtilities.GetUserIdFromClaims(context));
            var job = await _JobRepository.FindByIdAsync(dto.Id);
            if (job is null) return new Error();
            //check whether user is editing its tasks
            if (job.Owner.Id != user.Id)
            {
                // Użytkownik nie ma uprawnień do edycji tego zadania
                return new No();
            }
            job.Name = dto.Name;
            job.Description = dto.Description;
            job.DueDate = dto.DueDate;
            job.Status = dto.Status;

            await _JobRepository.SaveAsync();
            return _Mapper.Map<ReadUpdateJobDto>(job);

        }

        public async Task<OneOf<Success, No, Error>> DeleteJobAsync(DeleteJobDto dto, HttpContext context)
        {
            var user = await _UserRepository.FindByIdAsync(TokenUtilities.GetUserIdFromClaims(context));
            var job = await _JobRepository.FindByIdAsync(dto.Id);
            if (job is null) return new Error();
            if (job.Owner.Id != user.Id)
            {
                // Użytkownik nie ma uprawnień do edycji tego zadania
                return new No();
            }

             _JobRepository.Delete(job);
            await _JobRepository.SaveAsync();
            return new Success();
        }
    }
}
