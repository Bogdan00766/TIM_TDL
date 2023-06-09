using AutoMapper;
using Microsoft.AspNetCore.Http;
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
            return _Mapper.Map<NewJobDto>(job);
        }
        public List<ReadJobDto> ReadJob(HttpContext context)
        {
            var jobsList = _JobRepository.GetAllUserJobs(TokenUtilities.GetUserIdFromClaims(context));

            return _Mapper.Map<List<ReadJobDto>>(jobsList);
           
        }
    }
}
