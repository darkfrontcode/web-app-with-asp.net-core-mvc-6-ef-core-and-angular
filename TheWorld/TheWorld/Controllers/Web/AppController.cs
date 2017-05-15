using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
			private WorldContext context;

		#endregion

		#region Constructor

			public AppController
			(
				IMailService mailService, 
				IConfigurationRoot config, 
				WorldContext context
			)
			{
				this.mailService = mailService;
				this.config = config;
				this.context = context;
			}

		#endregion

		#region Index

			public IActionResult Index()
			{
				var data = context.Trips.ToList();
				return View(data);
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
