using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIM_TDL.Domain.Utils
{
    public static class DataTypes
    {
        public enum JobStatus { 
            Error = 0,
            Done = 1,
            InProgress = 2,
            Planned = 3,
        }

        public enum Roles
        {
            Error = 0,
            User = 1,
            Admin = 2,
        }
    }
}
