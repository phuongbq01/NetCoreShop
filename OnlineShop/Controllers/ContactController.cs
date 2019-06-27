using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineShop.Models;
using OnlineShop.Services;
using OnlineShop.Services.Interfaces;
using OnlineShop.Utilities.Constants;
using System.Threading.Tasks;
using IEmailSender = OnlineShop.Services.IEmailSender;

namespace OnlineShop.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IFeedbackService _feedbackService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IViewRenderService _viewRenderService;

        public ContactController(IContactService contactService, IFeedbackService feedbackService, IEmailSender emailSender, IConfiguration configuration, IViewRenderService viewRenderService)
        {
            _contactService = contactService;
            _feedbackService = feedbackService;
            _emailSender = emailSender;
            _configuration = configuration;
            _viewRenderService = viewRenderService;
        }

        [Route("contact.html")]
        [HttpGet]
        public IActionResult Index()
        {
            var contact = _contactService.GetById(CommonConstants.DefaultContactID);
            var model = new ContactPageViewModel { Contact = contact };
            return View(model);
        }

        [Route("contact.html")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Index(ContactPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                _feedbackService.Add(model.Feedback);
                _feedbackService.SaveChanges();
                var content = await _viewRenderService.RenderToStringAsync("Contact/_ContactMail", model.Feedback);
                await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "Have new contact feedback", content);
                ViewData["Success"] = true;
            }

            model.Contact = _contactService.GetById("default");

            return View("Index", model);
        }
    }
}