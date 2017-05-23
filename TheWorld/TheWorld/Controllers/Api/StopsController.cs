using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
	[Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {

		private IWorldRepository repository;
		private ILogger<TripsController> logger;

		public StopsController(IWorldRepository repository, ILogger<TripsController> logger)
		{
			this.repository = repository;
			this.logger = logger;
		}
		
		[HttpGet("")]
		public IActionResult Get(string tripName)
		{
			try
			{
				Trip trip = repository.GetTripByName(tripName);
				return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
			}
			catch (Exception ex)
			{
				logger.LogError("Failed to get stops: {0}", ex);
			}

			return BadRequest("Failed to get stops");
		}
    }
}
