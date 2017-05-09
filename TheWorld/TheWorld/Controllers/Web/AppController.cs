using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
		private IMailService mailService;

		public AppController(IMailService mailService)
		{
			this.mailService = mailService;
		}
        
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Contact(ContactViewModel model)
		{
			mailService.SendMail("fake@notserver.com", model.Email, "From TheWorld", model.Message);
			return View();
		}

		public IActionResult Contact()
		{
			return View();
		}

	}
}
