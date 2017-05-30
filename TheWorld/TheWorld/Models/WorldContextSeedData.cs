using Microsoft.AspNetCore.Identity;
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
		private UserManager<WorldUser> userManager;

		public WorldContextSeedData(WorldContext context, UserManager<WorldUser> userManager)
		{
			this.context = context;
			this.userManager = userManager;
		}

		private Trip USProvider()
		{
				return new Trip()
				{
					DateCreated = DateTime.UtcNow,
					Name = "US Trip",
					UserName = "Test",
					Stops = MockTrips.US()
				};
		}

		private Trip WorldProvider()
		{
				return new Trip()
				{
					DateCreated = DateTime.UtcNow,
					Name = "World Trip",
					UserName = "Test",
					Stops = MockTrips.World()
				};
		}

		public async Task EnsureSeedData()
		{
			if(await userManager.FindByEmailAsync("test@gmail.com") == null)
			{
				var user = new WorldUser()
				{
					UserName = "test",
					Email = "test@gmail.com"
				};

				await userManager.CreateAsync(user, "P@ssw0rd!");

			}

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
