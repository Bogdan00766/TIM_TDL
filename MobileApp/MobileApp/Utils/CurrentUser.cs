using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Utils
{
    public static class CurrentUser
    {
        public static string Id { get; set; }
        public static string Email { get; set; }
        public static string AccessToken { get; set; }
        public static string BearerToken { get; set; }
        //wszystkie properties static id ematil tokeny 2 i cos tam 
        //z zalogowania podawac tutaj wszystko
        //currentuser.token = ...
        //crud korzysta z readjobs -> podawać token
        //wylogowywanie ustawia parametry na null
    }
}
