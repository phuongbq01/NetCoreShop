using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineShop.Authorization;
using OnlineShop.Services.Interfaces;
using OnlineShop.Services.ViewModels.system;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IAuthorizationService _authorizationService;

        public RoleController(IRoleService roleService, IAuthorizationService authorizationService)
        {
            _roleService = roleService;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Index()
        {
            // Kiểm tra quyền View của User cho function Role
            var result = await _authorizationService.AuthorizeAsync(User, "ROLE", Operations.Read);
            if (result.Succeeded == false)  // Nếu không có quyền trả về trang Login
                return new RedirectResult("/Admin/Login/Index");

            return View();
        }


        #region Ajax
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _roleService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (!roleVm.Id.HasValue)
            {
                await _roleService.AddAsync(roleVm);
            }
            else
            {
                await _roleService.UpdateAsync(roleVm);
            }
            return new OkObjectResult(roleVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            await _roleService.DeleteAsync(id);
            return new OkObjectResult(id);
        }

        [HttpPost]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> listPermission, Guid roleId)
        {
            _roleService.SavePermission(listPermission, roleId);
            return new OkResult();
        }

        #endregion
    }
}