using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebViewModel.Common;
using WebViewModel.SystemService.Role;

namespace AdminApp.Services
{
    public interface IRoleService
    {
        Task<ApiResult<List<RoleVm>>> GetAll();
        Task CreateRoles(List<RoleVm> roles);
        Task<string> TestRole();
    }

    public class RoleService : IRoleService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<List<RoleVm>>> GetAll()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync("/api/role/getallroles");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<RoleVm> myDeserializedObjList = (List<RoleVm>)JsonConvert.DeserializeObject(body, typeof(List<RoleVm>));
                return new ApiSuccessResult<List<RoleVm>>(myDeserializedObjList);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<RoleVm>>>(body);
        }

        public async Task CreateRoles(List<RoleVm> roles)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(roles);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync($"/api/role/createroles", httpContent);
        }

        public async Task<string> TestRole()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync("/api/role/Role12");
            var body = await response.Content.ReadAsStringAsync();
            return "";

            //return JsonConvert.DeserializeObject<ApiErrorResult<UserVm>>(body);
        }
    }
}
