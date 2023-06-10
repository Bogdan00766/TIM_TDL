using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TIM_TDL.Application.Utilities
{
    public static class TokenUtilities
    {
        public static int GetUserIdFromClaims(HttpContext context)
        {
            var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new Exception("Nie można wyciągnąć identyfikatora użytkownika z claima.");
        }
        public static string GetEmailFromClaims(HttpContext context)
        {
            var emailClaim = context.User.FindFirstValue(ClaimTypes.Email);
            if (!string.IsNullOrEmpty(emailClaim))
            {
                return emailClaim;
            }
            throw new Exception("Nie można pobrać adresu e-mail z claima.");
        }
    }
}
