using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Authorization;
using OnlineShop.Extensions;
using OnlineShop.Service.Dapper.Interfaces;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IAuthorizationService _authorizationService;

        public HomeController(IReportService reportService, IAuthorizationService authorizationService)
        {
            _reportService = reportService;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "PRODUCT_LIST", Operations.Read);
            if (result.Succeeded == false)  // Nếu không có quyền trả về trang Login
                return new RedirectResult("/Admin/Login/Index");

            return View();
        }

        public async Task<IActionResult> GetRevenue(string fromDate, string toDate)
        {
            return new OkObjectResult(await _reportService.GetReportAsync(fromDate, toDate));
        }

    }
}