using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIM_TDL.MobileApp.Dtos.User
{
    public class TokenInfoDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
