using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebViewModel.Common;
using WebViewModel.SystemService.User;

namespace AdminApp.Services
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authentication(LoginRequest request);
    }

    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClient;

        public UserService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<string>> Authentication(LoginRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var result = await client.PostAsync("/api/User/authentication", content);

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(await result.Content.ReadAsStringAsync());
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(await result.Content.ReadAsStringAsync());

            //return await result.Content.ReadAsStringAsync();
        }
    }
}
