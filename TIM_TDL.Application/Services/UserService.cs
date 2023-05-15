using AutoMapper;
using OneOf;
using OneOf.Types;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Application.Dtos.User;
using TIM_TDL.Application.IServices;
using TIM_TDL.Domain.IRepositories;
using TIM_TDL.Domain.Models;

namespace TIM_TDL.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger _Logger;
        private readonly IMapper _Mapper;
        private readonly IUserRepository _UserRepository;

        public UserService(ILogger logger, IMapper mapper, IUserRepository userRepository)
        {
            _Logger = logger.ForContext<UserService>();
            _Mapper = mapper;
            _UserRepository = userRepository;
            
        }

        private byte[] HashPassword(string email, string password)
        {
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(email + password));
            }
            return hash;
        }

        public async Task<OneOf<UserDataDto, Error, NotFound>> RegisterAsync(RegisterLoginUserDto dto)
        {
            _Logger.Verbose("Register Task in User Service called");

            User user = new User
            {
                Email = dto.Email,
                Password = HashPassword(dto.Email, dto.Password),
            };
            try
            {
                _UserRepository.Create(user);
                await _UserRepository.SaveAsync();
                _Logger.Information("User of id: {id} and email: {email} added sucessfully", user.Id, user.Email);
                return _Mapper.Map<UserDataDto>(user);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex, "Error during inserting new user to database");
            }
            return new Error();  
            return new NotFound();
        }
        private bool ifHashesEqual(byte[] hash1, byte[] hash2)
        {
            bool bEqual = false;
            if (hash1.Length == hash2.Length)
            {
                int i = 0;
                while ((i < hash1.Length) && (hash1[i] == hash2[i]))
                {
                    i += 1;
                }
                if (i == hash1.Length)
                {
                    bEqual = true;
                }
            }
            return bEqual;
        }
        public OneOf<UserDataDto, Error, NotFound> Login(RegisterLoginUserDto dto)
        {
            var user = _UserRepository.FindByEmail(dto.Email);
            if (user == null)
            {
                return new NotFound();
            }
            var hash = HashPassword(dto.Email, dto.Password);
            if (ifHashesEqual (hash, user.Password)) 
            {
                return _Mapper.Map<UserDataDto>(user);
            }
            return new Error();
            throw new NotImplementedException();
        }
    }
}
