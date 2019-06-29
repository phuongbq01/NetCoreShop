using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Extensions;
using OnlineShop.Infrastructure.Interfaces;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AnnouncementController : Controller
    {
        
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }
        [HttpGet]
        public IActionResult GetAllPaging(int page, int pageSize)
        {
            var model = _announcementService.GetAllUnReadPaging(User.GetUserId(), page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult MarkAsRead(string id)
        {
            var result = _announcementService.MarkAsRead(User.GetUserId(), id);
            _announcementService.Save();
            return new OkObjectResult(result);
        }
    }
}