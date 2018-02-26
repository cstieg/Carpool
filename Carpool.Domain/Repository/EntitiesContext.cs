using Carpool.Domain.Models;
using System.Data.Entity;

namespace Carpool.Domain.Repository
{
    public class EntitiesContext : DbContext
    {
        public EntitiesContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public EntitiesContext() : base() { }

        public IDbSet<Address> Addresses { get; set; }
        public IDbSet<LatLng> LatLngs { get; set; }
        public IDbSet<Location> Locations { get; set; }
        public IDbSet<Review> Reviews { get; set; }
        public IDbSet<Ride> Rides { get; set; }
        public IDbSet<RideCost> RideCosts { get; set; }
        public IDbSet<RideLeg> RideLegs { get; set; }
        public IDbSet<Rider> Rider { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Vehicle> Vehicles { get; set; }
        public IDbSet<VehicleMake> VehicleMakes { get; set; }
        public IDbSet<VehicleModel> VehicleModels { get; set; }
    }
}
