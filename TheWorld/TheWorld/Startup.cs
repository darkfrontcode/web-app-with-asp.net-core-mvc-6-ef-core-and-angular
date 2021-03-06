﻿using System;
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
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

			private void ExceptionPage(
				IApplicationBuilder app, 
				IHostingEnvironment env, 
				ILoggerFactory factory
			)
			{
				bool dev = env.IsDevelopment();
				if(dev)
				{
					app.UseDeveloperExceptionPage();
					factory.AddDebug(LogLevel.Information);
				}
				else
				{
					factory.AddDebug(LogLevel.Error);
				}
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

		#region CamelCase Options

			private Action<Microsoft.AspNetCore.Mvc.MvcJsonOptions> CamelCaseConfig()
			{
				return config => {
					config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				};
			}

		#endregion

		#region Mapper Config

			public Action<IMapperConfigurationExpression> MapperConfig()
			{
				return config => { 
					config.CreateMap<TripViewModel, Trip>().ReverseMap();
					config.CreateMap<StopViewModel, Stop>().ReverseMap();
				};
			}

		#endregion

		#region IdentityRoleConfig

			public Action<IdentityOptions> IdentityRoleConfig()
			{
				return config => { 
					config.User.RequireUniqueEmail = true;
					config.Password.RequiredLength = 8;
					config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
				};
			}

		#endregion

		#region Configure Service

		public void ConfigureServices(IServiceCollection services)
			{
				services.AddSingleton(config);
				AddIMailService(services);
				services.AddDbContext<WorldContext>();
				services.AddScoped<IWorldRepository, WorldRepository>();
				services.AddTransient<GeoCoordsService>();
				services.AddTransient<WorldContextSeedData>();
				services.AddIdentity<WorldUser, IdentityRole>(IdentityRoleConfig()).AddEntityFrameworkStores<WorldContext>();
				services.AddLogging();
				services.AddMvc().AddJsonOptions(CamelCaseConfig());
			}

			public void Configure(
				IApplicationBuilder app, 
				IHostingEnvironment env,
				WorldContextSeedData seeder,
				ILoggerFactory factory
			)
			{
				Mapper.Initialize(MapperConfig());
				ExceptionPage(app, env, factory);
				app.UseStaticFiles();
				app.UseIdentity();
				app.UseMvc(MapRoute());
				seeder.EnsureSeedData().Wait();
			}

		#endregion

	}
}
