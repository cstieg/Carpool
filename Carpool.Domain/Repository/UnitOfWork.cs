using Carpool.Domain.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Carpool.Domain.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EntitiesContext _context;

        public IEntityRepository<Address> Addresses { get; }
        public IEntityRepository<Review> Reviews { get; }
        public IEntityRepository<Ride> Rides { get; }
        public IEntityRepository<RideCost> RideCosts { get; }
        public IEntityRepository<RideLeg> RideLegs { get; }
        public IEntityRepository<Rider> Riders { get; }
        public IEntityRepository<User> Users { get; }
        public IEntityRepository<Vehicle> Vehicles { get; }
        public IEntityRepository<VehicleMake> VehicleMakes { get; }
        public IEntityRepository<VehicleModel> VehicleModels { get; }
        
        public UnitOfWork(EntitiesContext context)
        {
            _context = context;
            Addresses = new EntityRepository<Address>(context);
            Reviews = new EntityRepository<Review>(context);
            Rides = new EntityRepository<Ride>(context);
            RideCosts = new EntityRepository<RideCost>(context);
            RideLegs = new EntityRepository<RideLeg>(context);
            Riders = new EntityRepository<Rider>(context);
            Users = new EntityRepository<User>(context);
            Vehicles = new EntityRepository<Vehicle>(context);
            VehicleMakes = new EntityRepository<VehicleMake>(context);
            VehicleModels = new EntityRepository<VehicleModel>(context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
