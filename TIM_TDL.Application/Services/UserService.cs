using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using OneOf.Types;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Application.Dtos.User;
using TIM_TDL.Application.IServices;
using TIM_TDL.Application.Utilities;
using TIM_TDL.Domain.IRepositories;
using TIM_TDL.Domain.Models;

namespace TIM_TDL.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger _Logger;
        private readonly IMapper _Mapper;
        private readonly IUserRepository _UserRepository;
        private readonly IConfiguration _Configuration;

        public UserService(ILogger logger, IMapper mapper, IUserRepository userRepository, IConfiguration configuration)
        {
            _Logger = logger.ForContext<UserService>();
            _Mapper = mapper;
            _UserRepository = userRepository;
            _Configuration = configuration;
            
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

        public async Task<OneOf<UserDataDto, Error, NotFound>> RegisterAsync(RegisterUserDto dto)
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
                _Logger.Error(ex, "Error during inserting new user to database or email is already register. Ask admin for information");
            }
            return new Error();  
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
        public OneOf<TokenInfoDto, Error, NotFound> Login(LoginUserDto dto)
        {
            var user = _UserRepository.FindByEmail(dto.Email);
            if (user == null)
            {
                return new NotFound();
            }
            var hash = HashPassword(dto.Email, dto.Password);
            if (ifHashesEqual (hash, user.Password)) 
            {
                var result = new TokenInfoDto();
                result.AccessToken = GenerateBerearToken(user);
                result.RefreshToken = GenerateRefreshToken(user);
               
                return result;
                
            }
            return new Error();
        }



        private string GenerateBerearToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_Configuration["Keys:JWT"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiry = DateTimeOffset.Now.AddMinutes(15);
            var userClaims = GetClaimsForUser(user);

            var securityToken = new JwtSecurityToken(
                issuer: _Configuration["Keys:Issuer"],
                audience: _Configuration["Keys:Audience"],
                claims: userClaims,
                notBefore: DateTime.Now,
                expires: expiry.DateTime,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private string GenerateRefreshToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_Configuration["Keys:JWT"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiry = DateTimeOffset.Now.AddDays(7);
            var userClaims = GetClaimsForUser(user);

            var securityToken = new JwtSecurityToken(
                issuer: _Configuration["Keys:Issuer"],
                //audience: _tokenOptions.Audience,
                claims: userClaims,
                notBefore: DateTime.Now,
                expires: expiry.DateTime,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private IEnumerable<Claim> GetClaimsForUser(User user)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));

                return claims;
            }


       


        public async Task<OneOf<Success, Error>> ChangePasswordAsync(ChangePasswordUser dto, HttpContext context)
        {
            int userId = TokenUtilities.GetUserIdFromClaims(context);

            _Logger.Verbose("Change Password Task in User Service called");


            var newPassword = dto.NewPassword;
            var email = TokenUtilities.GetEmailFromClaims(context);

            var hashNewPassword = HashPassword(email, newPassword);
            try
            {
                var user = await _UserRepository.FindByIdAsync(userId);

                user.Password = hashNewPassword;

                await _UserRepository.SaveAsync();

                return new Success();
            }
            catch(Exception ex) {
            _Logger.Error(ex, "Error while connecting to database while trying to change password");
                return new Error();
            }
           
        }


    }
}
