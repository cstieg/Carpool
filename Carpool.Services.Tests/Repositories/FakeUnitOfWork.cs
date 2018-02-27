using Carpool.Domain.Models;
using Carpool.Domain.Repository;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Carpool.Services.Tests.Repositories
{
    public class FakeUnitOfWork : IUnitOfWork
    {
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
        public IEntityRepository<VehicleModelYear> VehicleModelYears { get; }

        private List<IEntity> _added = new List<IEntity>();
        private List<IEntity> _changed = new List<IEntity>();
        private List<IEntity> _deleted = new List<IEntity>();
        private readonly EntitiesContext _context;

        public FakeUnitOfWork(EntitiesContext context)
        {
            _context = context;
        }


        public void Add(IEntity entity)
        {
            _context.Set(entity.GetType()).Add(entity);
        }

        public void Edit(IEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(IEntity entity)
        {
            _context.Set(entity.GetType()).Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
