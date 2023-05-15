using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Domain.IRepositories;
using TIM_TDL.Domain.Models;

namespace TIM_TDL.Infrastructure.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(TDLDbContext dbContext) : base(dbContext)
        {
        }
    }
}
