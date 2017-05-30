using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {

		#region Props

			private IMailService mailService;
			private IConfigurationRoot config;
			private IWorldRepository repository;
			private ILogger<AppController> logger;

		#endregion

		#region Constructor

		public AppController
			(
				IMailService mailService, 
				IConfigurationRoot config,
				IWorldRepository repository,
				ILogger<AppController> logger
				
			)
			{
				this.mailService = mailService;
				this.config = config;
				this.repository = repository;
				this.logger = logger;
			}

		#endregion

		#region Index

			public IActionResult Index()
			{
				return View();
			}

		#endregion

		#region Trips
			
			[Authorize]
			public IActionResult Trips()
			{
				try
				{
					var data = repository.GetAllTrips();
					return View(data);
				}
				catch (Exception ex)
				{
					logger.LogError($"Failed to get trips in Index page: {ex.Message}");
					return Redirect("/error");
				}
			}

		#endregion

		#region About

		public IActionResult About()
			{
				return View();
			}

		#endregion

		#region Contact

			private void ContactPostValidations(ContactViewModel model)
			{
				if(model.Email.Contains("aol.com"))
				{
					ModelState.AddModelError("Email", "We don't support AOL addresses");
				}	
			}

			private void ContactSendEmail(ContactViewModel model)
			{
				if(ModelState.IsValid)
				{
					var to = config["MailSettings:ToAddress"];
					mailService.SendMail(to, model.Email, "From TheWorld", model.Message);
					ModelState.Clear();
					ViewBag.UserMessage = "Message Sent";
				}
			}

			[HttpPost]
			public IActionResult Contact(ContactViewModel model)
			{
				ContactPostValidations(model);
				ContactSendEmail(model);
				return View();
			}

			public IActionResult Contact()
			{
				return View();
			} 

		#endregion

	}
}
