using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public class GeoCoordsService
    {
		private ILogger<GeoCoordsService> logger;
		private IConfigurationRoot config;

		public GeoCoordsService(
			ILogger<GeoCoordsService> logger, 
			IConfigurationRoot config
		)
		{
			this.logger = logger;
			this.config = config;
		}

		public async Task<GeoCoordsResult> GetCoordsAsync(string name)
		{
			GeoCoordsResult result = new GeoCoordsResult()
			{
				Success = false,
				Message = "Failed to get coordinates"
			};

			string apiKey = config["Keys:BingKey"];
			string encodedName = WebUtility.UrlEncode(name);
			string url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={apiKey}";

			var client = new HttpClient();
			var json = await client.GetStringAsync(url);
			
			var results = JObject.Parse(json);
			var resources = results["resourceSets"][0]["resources"];
			if(!results["resourceSets"][0]["resources"].HasValues)
			{
				result.Message = $"Could not find '{name}' as a location";
			}
			else
			{
				var confidence = (string) resources[0]["confidence"];
				if(confidence != "High")
				{
					result.Message = $"Coul not find a confident match for '{name}' as a lo..";
				}
				else
				{
					var coords = resources[0]["geocodePoints"][0]["coordinates"];
					result.Latitude = (double) coords[0];
					result.Longitude = (double) coords[1];
					result.Success = true;
					result.Message = "Success";
				}
			}

			return result;
		}
    }
}
