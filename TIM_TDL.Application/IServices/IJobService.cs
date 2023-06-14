using Microsoft.AspNetCore.Http;
using OneOf.Types;
using OneOf;
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
        public List<ReadUpdateJobDto> ReadJob(HttpContext context);
        public Task<OneOf<ReadUpdateJobDto,No, Error>> UpdateJobAsync(ReadUpdateJobDto dto, HttpContext context);
        public Task<OneOf<Success, No, Error>> DeleteJobAsync(DeleteJobDto dto, HttpContext context);
    }
}
