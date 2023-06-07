using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Application.Dtos.User;
using TIM_TDL.Domain.Models;

namespace TIM_TDL.Application.IServices
{
    public interface IUserService
    {
        Task<OneOf<UserDataDto, Error, NotFound>> RegisterAsync(LoginUserDto dto);
        OneOf<TokenInfoDto, Error, NotFound> Login(LoginUserDto dto);
    }
}
