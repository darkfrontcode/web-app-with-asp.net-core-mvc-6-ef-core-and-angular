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

namespace TheWorld
{
    public class Startup
    {

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

		private void ExceptionPage(IApplicationBuilder app, IHostingEnvironment env)
		{
			bool dev = env.IsDevelopment();
			if(dev) app.UseDeveloperExceptionPage();
		}

		public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			ExceptionPage(app, env);
			app.UseStaticFiles();
			app.UseMvc(MapRoute());
		}

	}
}
