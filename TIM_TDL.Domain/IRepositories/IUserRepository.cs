﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Domain.Models;

namespace TIM_TDL.Domain.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByEmail(string email);
    }
}
