using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TIM_TDL.Domain.Utils;
using Xamarin.Forms;

namespace TIM_TDL.MobileApp.Dtos.User
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
       
    }
}
