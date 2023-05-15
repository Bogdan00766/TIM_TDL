using AutoMapper;
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

namespace TIM_TDL.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IUserRepository _UserRepository;
        private readonly IJobRepository _JobRepository;
        private readonly IMapper _Mapper;

        public JobService(IUserRepository UserRepository, IJobRepository JobRepository, IMapper Mapper)
        {
            _UserRepository = UserRepository;
            _JobRepository = JobRepository;
            _Mapper = Mapper;
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

            _JobRepository.Create(job);
            await _JobRepository.SaveAsync();
            return _Mapper.Map<NewJobDto>(job);
        }
    }
}
