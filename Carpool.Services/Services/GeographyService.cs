using Carpool.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Carpool.Services.Services
{
    public class GeographyService
    {

        public float GetGeographicDistanceBetweenLocations(Location location1, Location location2)
        {
            throw new NotImplementedException();
        }

        public async Task<float> GetRoadDistanceBetweenLocationsAsync(Location location1, Location location2)
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetApproximateTimeBetweenLocations(Location location1, Location location2)
        {
            throw new NotImplementedException();
        }

        public async Task<TimeSpan> GetExactTimeBetweenLocationsAsync(Location location1, Location location2)
        {
            throw new NotImplementedException();
        }

    }
}
