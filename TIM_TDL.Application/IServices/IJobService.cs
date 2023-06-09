using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Application.Dtos.Job;

namespace TIM_TDL.Application.IServices
{
    public interface IJobService
    {
        public Task<NewJobDto> CreateJobAsync(CreateJobDto dto, HttpContext context);
        public List<ReadJobDto> ReadJob(HttpContext context);
    }
}
