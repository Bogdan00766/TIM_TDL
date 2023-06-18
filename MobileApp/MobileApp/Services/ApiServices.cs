using MobileApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.MobileApp.Dtos.Job;
using TIM_TDL.MobileApp.Dtos.User;
using static System.Net.WebRequestMethods;

namespace MobileApp.Services
{
    public class ApiServices
    {
        private HttpClient httpClient;

        public ApiServices()
        {
            httpClient = new HttpClient();
        }

        public string link = "http://192.168.77.45:5004";
        public async Task <bool> RegisterAsync(string email,  string password)
        {
            var client = httpClient;
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
                string uri = link+"/api/register"; 
                var response = await client.PostAsync(uri, content);
                resp = response.IsSuccessStatusCode;
               
            }
            catch (Exception ex)
            {
                throw;
            }

            //var response = await client.PostAsync(Config.Data.ApiUrl + "/api/register", content);
            return resp;
            //return response.IsSuccessStatusCode;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var client = httpClient;
            var model = new LoginUserDto
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
                string uri = link + "/api/login";
                var response = await client.PostAsync(uri, content);
                resp = response.IsSuccessStatusCode;


                string responseJson = await response.Content.ReadAsStringAsync();
                MyResponseObject responseObject = JsonConvert.DeserializeObject<MyResponseObject>(responseJson);

            }
            catch (Exception ex)
            {
                throw;
            }

            //var response = await client.PostAsync(Config.Data.ApiUrl + "/api/register", content);
            return resp;
            //return response.IsSuccessStatusCode;
        }
        public async Task<List<ReadUpdateJobDto>> GetJobs(string userId)
        {
            var url = $"https://example.com/api/jobs?userId={userId}";
            //do naglowka token

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jobsList = JsonConvert.DeserializeObject<List<ReadUpdateJobDto>>(content);
                return jobsList;
            }
            else
            {
                // Obsługa błędu, np. wyświetlenie komunikatu o błędzie
                return null;
            }
        }


    }
}
