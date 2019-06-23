using Microsoft.AspNetCore.Mvc;
using OnlineShop.Extensions;
using OnlineShop.Services.Interfaces;
using OnlineShop.Services.ViewModels.system;
using OnlineShop.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private IFunctionService _functionService;

        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Hiển thị Menu theo quyền
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (roles.Split(";").Contains(CommonConstants.AdminRole))
            {
                functions = await _functionService.GetAll(string.Empty);
            }
            else
            {
                //TODO: Get by permission
                functions = await _functionService.GetAll(string.Empty);
            }
            return View(functions);
        }
    }
}
