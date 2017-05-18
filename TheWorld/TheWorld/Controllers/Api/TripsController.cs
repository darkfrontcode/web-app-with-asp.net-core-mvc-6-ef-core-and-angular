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
	[Route("api/trips")]
    public class TripsController : Controller
    {

		#region constructor

			private IWorldRepository repository;
			private ILogger<TripsController> logger;

			public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
			{
				this.repository = repository;
				this.logger = logger;
			}

		#endregion

		#region get

			[HttpGet("")]
			public IActionResult Get()
			{
				try
				{
					var results = repository.GetAllTrips();
					return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
				}
				catch (Exception ex)
				{
					logger.LogError($"Failed to get all trips: {ex}");
					return BadRequest("Error occurred");
				}
			}

		#endregion

		#region post

			[HttpPost("")]
			public async Task<IActionResult> Post([FromBody]TripViewModel trip)
			{
				if (ModelState.IsValid)
				{	
					Trip newTrip = Mapper.Map<Trip>(trip);
					repository.AddTrip(newTrip);

					if (await repository.SaveChangesAsync())
					{
						return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModel>(newTrip));	
					}

				}

				return BadRequest("Failed to save the trip");
			}

		#endregion
    }
}
