using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Entities;
using WebViewModel.SystemService.Role;

namespace WebApplication.SystemService
{
    public interface IRoleService
    {
        Task<List<RoleVm>> GetAll();
        Task CreateRoles(List<RoleVm> roles);
    }

    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleVm>> GetAll()
        {            
            var roles = await _roleManager.Roles
                .Select(x => new RoleVm()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();

            return roles;
        }

        public async Task CreateRoles(List<RoleVm> roles)
        {
            foreach(var item in roles)
            {
                await _roleManager.CreateAsync(new AppRole
                {
                    Id = item.Id,
                    Description = item.Description,
                    Name = item.Name,
                    NormalizedName = item.Name
                });
            }
        }
    }
}
