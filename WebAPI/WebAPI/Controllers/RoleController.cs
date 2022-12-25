using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.SystemService;
using WebViewModel.SystemService.Role;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("getallroles")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAll();
            return Ok(roles);
        }

        [HttpGet]
        [Route("Role12")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Role1")]
        public IActionResult TestRole12()
        {
            return Content("Role1, Role2");
        }

        [HttpGet]
        [Route("Role13")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Role1, Role3")]
        public IActionResult TestRole13()
        {
            return Content("Role1, Role3");
        }

        [HttpGet]
        [Route("Role4")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Role4")]
        public IActionResult TestRole4()
        {
            return Content("Role4");
        }

        [HttpGet]
        [Route("Role14")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Role1")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Role4")]
        public IActionResult TestRole14()
        {
            return Content("Role1&4");
        }

        //[Route("createroles")]
        //public async Task<IActionResult> CreateRoles(List<RoleVm> roles)
        //{
        //    await _roleService.CreateRoles(roles);
        //    return Ok(roles);
        //}
    }
}
