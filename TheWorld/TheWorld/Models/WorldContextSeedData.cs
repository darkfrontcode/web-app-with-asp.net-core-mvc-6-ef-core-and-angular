using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldContextSeedData
    {
		private WorldContext context;

		public WorldContextSeedData(WorldContext context)
		{
			this.context = context;
		}

		private Trip USProvider()
		{
				return new Trip()
				{
					DateCreated = DateTime.UtcNow,
					Name = "US Trip",
					UserName = "",
					Stops = new List<Stop>(){}
					//Stops = MockTrips.US()
				};
		}

		private Trip WorldProvider()
		{
				return new Trip()
				{
					DateCreated = DateTime.UtcNow,
					Name = "World Trip",
					UserName = "",
					Stops = new List<Stop>(){}
					//Stops = MockTrips.World()
				};
		}

		public async Task EnsureSeedData()
		{
			if (!context.Trips.Any())
			{
				Trip us = USProvider();
				Trip world = WorldProvider();

				context.Trips.Add(us);
				context.Stops.AddRange(us.Stops);
				context.Trips.Add(world);
				context.Stops.AddRange(world.Stops);

				await context.SaveChangesAsync();
			}
		}
    }
}
