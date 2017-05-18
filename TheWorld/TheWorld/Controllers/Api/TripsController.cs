using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
		private IWorldRepository repository;

		public TripsController(IWorldRepository repository)
		{
			this.repository = repository;
		}

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
				return BadRequest("Error occurred");
			}
		}

		[HttpPost("")]
		public IActionResult Post([FromBody]TripViewModel trip)
		{
			if (ModelState.IsValid)
			{	
				Trip newTrip = Mapper.Map<Trip>(trip);
				return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModel>(newTrip));
			}

			return BadRequest("Bad data");
		}
    }
}
