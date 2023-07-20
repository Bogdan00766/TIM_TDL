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
        private readonly HttpClient _HttpClient;
        private string _Url = "http://192.168.77.201:5004/";
        
        public ApiServices()
        {
            _HttpClient = new HttpClient();
            _HttpClient.BaseAddress = new Uri(_Url);
            
        }

        public async Task<ReadUpdateJobDto> AddJobAsync(CreateJobDto dto)
        {
            HttpResponseMessage response;
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                HttpContent content = new StringContent(json);
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.AccessToken);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await _HttpClient.PostAsync("api/createJob", content);
                if (response != null && response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ReadUpdateJobDto>(await response.Content.ReadAsStringAsync());
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        internal async Task<ReadUpdateJobDto> UpdateJobAsync(ReadUpdateJobDto dto)
        {
            HttpResponseMessage response;
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                HttpContent content = new StringContent(json);
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.AccessToken);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await _HttpClient.PutAsync("api/updateJob", content);
                if (response != null && response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ReadUpdateJobDto>(await response.Content.ReadAsStringAsync());
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> RegisterAsync(string email, string password)
        {
            var client = _HttpClient;
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
                string uri = "/api/register";
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

        public async Task<HttpResponseMessage> LoginAsync(string email, string password)
        {
            var client = _HttpClient;
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
                string uri = "/api/login";
                var response = await client.PostAsync(uri, content);
                return response;


                //MyResponseObject responseObject = JsonConvert.DeserializeObject<MyResponseObject>(responseJson);

            }
            catch (Exception ex)
            {
                throw;
            }

            //var response = await client.PostAsync(Config.Data.ApiUrl + "/api/register", content);
            
            //return response.IsSuccessStatusCode;
        }
        public async Task<List<ReadUpdateJobDto>> GetJobsAsync()
        {
            try
            {

                var uri = "/api/readJob";
                //do naglowka token
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.AccessToken);

                var response = await _HttpClient.GetAsync(uri);

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
            catch (Exception ex)
            {
                // Obsłuż błąd, na przykład zapisz go w dzienniku lub obsłuż inaczej
                throw;
            }


        }

        internal async Task DeleteJobAsync(ReadUpdateJobDto job)
        {
            HttpResponseMessage response;
            try
            {
                DeleteJobDto dto = new DeleteJobDto
                {
                    Id = job.Id
                };
                var json = JsonConvert.SerializeObject(dto);
                HttpContent content = new StringContent(json);
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.AccessToken);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await _HttpClient.PostAsync("api/deleteJob", content);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public class DeleteJobDto
    {
        public int Id { get; set; }
    }
}

