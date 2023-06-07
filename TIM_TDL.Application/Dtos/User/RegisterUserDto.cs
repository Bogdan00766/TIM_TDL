using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Domain.Utils;

namespace TIM_TDL.Application.Dtos.User
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DataTypes.Roles Role { get; set; }
    }
}
