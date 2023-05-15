using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Domain.IRepositories;
using TIM_TDL.Domain.Models;

namespace TIM_TDL.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TDLDbContext dbContext) : base(dbContext)
        {
        }

        public User FindByEmail(string email)
        {
            return _dbContext.Users.Where(x => x.Email == email).FirstOrDefault();
        }
    }
}
