using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Application.Dtos.Job;
using TIM_TDL.Application.Dtos.User;
using TIM_TDL.Domain.Models;

namespace TIM_TDL.Application.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDataDto>();
            CreateMap<Job, NewJobDto>();
            CreateMap<Job, ReadJobDto>();
        }
    }
}
