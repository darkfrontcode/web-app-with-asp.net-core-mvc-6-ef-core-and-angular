using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
	{

		#region constructor

			private WorldContext context;
			private ILogger<WorldRepository> logger;

			public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
			{
				this.context = context;
				this.logger = logger;
			}

		#endregion

		public IEnumerable<Trip> GetAllTrips()
		{
			logger.LogInformation("Getting all Trips from database");
			return context.Trips.ToList();
		}

		public void AddTrip(Trip trip)
		{
			context.Add(trip);
		}

		public async Task<bool> SaveChangesAsync()
		{
			return (await context.SaveChangesAsync()) > 0;
		}
	}
}
