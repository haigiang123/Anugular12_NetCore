using AdminApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebViewModel.Common;
using WebViewModel.SystemService.Role;
using WebViewModel.SystemService.User;

namespace AdminApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;

        public HomeController(IUserService userService, IConfiguration configuration, IRoleService roleService)
        {
            _userService = userService;
            _configuration = configuration;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userService.GetUsersPaging(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userService.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Thêm mới người dùng thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        public async Task CreateRoles()
        {
            List<RoleVm> roles = new List<RoleVm>
            {
                new RoleVm { Id = Guid.NewGuid(), Description = "Role1", Name = "Role1" },
                new RoleVm { Id = Guid.NewGuid(), Description = "Role2", Name = "Role2" },
                new RoleVm { Id = Guid.NewGuid(), Description = "Role3", Name = "Role3" },
                new RoleVm { Id = Guid.NewGuid(), Description = "Role4", Name = "Role4" }
            };

            await _roleService.CreateRoles(roles);
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userService.RoleAssign(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật quyền thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetRoleAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userService.GetById(id);
            var roleObj = await _roleService.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }

        [HttpGet]
        public async Task<IActionResult> TestRole()
        {

            var result = await _roleService.TestRole();

            return View(result);
        }

    }
}
