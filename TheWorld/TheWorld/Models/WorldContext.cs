using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TheWorld.Models
{
    public class WorldContext : IdentityDbContext<WorldUser>
    {
		
		public DbSet<Trip> Trips { get; set; }
		public DbSet<Stop> Stops { get; set; }
		public IConfigurationRoot config { get; private set; }

		public WorldContext(IConfigurationRoot config, DbContextOptions options) : base(options)
		{
			this.config = config;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(config["ConnectionsStrings:WorldContextConnection"]);
		}
	}
}
