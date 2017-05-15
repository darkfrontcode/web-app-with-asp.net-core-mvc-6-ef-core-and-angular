using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;

namespace TheWorld
{
    public class Startup
    {
		#region props

			private IHostingEnvironment environment;
			private IConfigurationRoot config;

		#endregion

		#region Constructor

			public Startup(IHostingEnvironment env)
			{	
				this.environment = env;
				this.config = ConfigBuilder(env);

			}

		#endregion

		#region Configuration Builder
			
			private IConfigurationRoot ConfigBuilder(IHostingEnvironment env)
			{
				var builder = new ConfigurationBuilder()
									.SetBasePath(environment.ContentRootPath)
									.AddJsonFile("config.json")
									.AddEnvironmentVariables();

				return builder.Build();
			}

		#endregion

		#region Map Route

		private Action<IRouteBuilder> MapRoute()
			{
				return config =>
				{
					config.MapRoute(
						name: "Default",
						template: "{controller}/{action}/{id?}",
						defaults: new { controller = "App", action = "Index" }
					);
				};
			}

		#endregion

		#region Exception page config

			private void ExceptionPage(IApplicationBuilder app, IHostingEnvironment env)
			{
				bool dev = env.IsDevelopment();
				if(dev) app.UseDeveloperExceptionPage();
			}

		#endregion

		#region Add Imail Service

			private void AddIMailService(IServiceCollection services)
			{
				bool dev = this.environment.IsDevelopment();
				if (dev) services.AddScoped<IMailService, DebugMailService>();
				else Console.WriteLine("Implement real service.");

			}

		#endregion

		#region Configure Service

			public void ConfigureServices(IServiceCollection services)
			{
				services.AddSingleton(config);
				AddIMailService(services);
				services.AddDbContext<WorldContext>();
				services.AddMvc();
			}

			public void Configure(IApplicationBuilder app, IHostingEnvironment env)
			{
				ExceptionPage(app, env);
				app.UseStaticFiles();
				app.UseMvc(MapRoute());
			}

		#endregion

	}
}
