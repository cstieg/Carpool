using Carpool.Domain.Models;
using Carpool.Domain.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpool.Services.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
        Task<Vehicle> EditVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(int id);
        Task<PaginatedList<Vehicle>> GetAllVehiclesAsync(int pageIndex, int pageSize);
        Task<List<Vehicle>> GetVehiclesForOwnerAsync(User owner);
        Task<Vehicle> GetVehicleByIdAsync(int id);

        Task<VehicleMake> AddVehicleMakeAsync(VehicleMake make);
        Task<VehicleMake> EditVehicleMakeAsync(VehicleMake make);
        Task DeleteVehicleMakeAsync(VehicleMake make);
        Task DeleteVehicleMakeAsync(int id);
        Task<List<VehicleMake>> GetAllVehicleMakesAsync();
        Task<VehicleMake> GetVehicleMakeByIdAsync(int id);

        Task<VehicleModel> AddVehicleModelAsync(VehicleModel model);
        Task<VehicleModel> EditVehicleModelAsync(VehicleModel model);
        Task DeleteVehicleModelAsync(VehicleModel model);
        Task DeleteVehicleModelAsync(int id);
        Task<PaginatedList<VehicleModel>> GetAllVehicleModelsAsync(int pageIndex, int pageSize);
        Task<List<VehicleModel>> GetVehicleModelsForMakeAsync(VehicleMake make);
        Task<List<VehicleModel>> GetVehicleModelsForMakeAndYearAsync(VehicleMake make, int year);
        Task<VehicleModel> GetVehicleModelByIdAsync(int id);

        Task<VehicleModelYear> AddVehicleModelYearAsync(VehicleModelYear modelYear);
        Task<VehicleModelYear> EditVehicleModelYearAsync(VehicleModelYear modelYear);
        Task DeleteVehicleModelYearAsync(VehicleModelYear modelYear);
        Task DeleteVehicleModelYearAsync(int id);
        Task<List<VehicleModelYear>> GetVehicleModelYearsForModelAsync(VehicleModel model);
        Task<VehicleModelYear> GetVehicleModelYearByIdAsync(int id);
    }
}
