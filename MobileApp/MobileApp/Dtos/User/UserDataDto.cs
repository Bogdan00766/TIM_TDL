using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Domain.Utils;

namespace TIM_TDL.MobileApp.Dtos.User
{
    public  class UserDataDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DataTypes.Roles Role { get; set; }
    }
}
