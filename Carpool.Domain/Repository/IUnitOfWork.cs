using Carpool.Domain.Models;
using System.Threading.Tasks;

namespace Carpool.Domain.Repository
{
    public interface IUnitOfWork
    {
        IEntityRepository<Address> Addresses { get; }
        IEntityRepository<Review> Reviews { get; }
        IEntityRepository<Ride> Rides { get; }
        IEntityRepository<RideCost> RideCosts { get; }
        IEntityRepository<RideLeg> RideLegs { get; }
        IEntityRepository<Rider> Riders { get; }
        IEntityRepository<User> Users { get; }
        IEntityRepository<Vehicle> Vehicles { get; }
        IEntityRepository<VehicleMake> VehicleMakes { get; }
        IEntityRepository<VehicleModel> VehicleModels { get; }
        IEntityRepository<VehicleModelYear> VehicleModelYears { get; }

        Task SaveChangesAsync();
    }
}
