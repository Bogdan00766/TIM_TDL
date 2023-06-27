using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Dtos.User
{
    public class UserTokenInfoDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
