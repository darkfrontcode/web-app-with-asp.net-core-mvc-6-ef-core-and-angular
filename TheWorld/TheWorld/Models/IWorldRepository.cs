using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
	public interface IWorldRepository
	{
		IEnumerable<Trip> GetAllTrips();

		Trip GetTripByName(string tripName);
		Task<bool> SaveChangesAsync();

		void AddTrip(Trip trip);
		void AddStop(string tripName, Stop newStop);
	}
}