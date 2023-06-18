using MobileApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Application.Dtos.User;

namespace MobileApp.Services
{
    public class ApiServices
    {
        public async Task <bool> RegisterAsync(string email,  string password)
        {
            var client = new HttpClient();

            var model = new RegisterUserDto 
            { 
             Email = email, 
             Password = password 
            };
            var json = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var resp = false;
            try
            {
                string uri = "http://192.168.77.45:5004/api/login";
                var response = await client.PostAsync(uri, content);
                resp = response.IsSuccessStatusCode;
               
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                resp = false;
                throw;
            }

            //var response = await client.PostAsync(Config.Data.ApiUrl + "/api/register", content);
            return resp;
            //return response.IsSuccessStatusCode;
        }
    }
}
