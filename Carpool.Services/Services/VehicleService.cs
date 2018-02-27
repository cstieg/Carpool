using Carpool.Domain.Models;
using Carpool.Domain.Repository;
using Carpool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Carpool.Services.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _uow;

        public VehicleService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            _uow.Vehicles.Add(vehicle);
            await _uow.SaveChangesAsync();
            return vehicle;
        }

        public async Task<VehicleMake> AddVehicleMakeAsync(VehicleMake make)
        {
            _uow.VehicleMakes.Add(make);
            await _uow.SaveChangesAsync();
            return make;
        }

        public async Task<VehicleModel> AddVehicleModelAsync(VehicleModel model)
        {
            _uow.VehicleModels.Add(model);
            await _uow.SaveChangesAsync();
            return model;
        }

        public async Task<VehicleModelYear> AddVehicleModelYearAsync(VehicleModelYear modelYear)
        {
            _uow.VehicleModelYears.Add(modelYear);
            await _uow.SaveChangesAsync();
            return modelYear;
        }

        public async Task DeleteVehicleAsync(Vehicle vehicle)
        {
            _uow.Vehicles.Delete(vehicle);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(int id)
        {
            await _uow.Vehicles.DeleteByIdAsync(id);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteVehicleMakeAsync(VehicleMake make)
        {
            _uow.VehicleMakes.Delete(make);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteVehicleMakeAsync(int id)
        {
            await _uow.VehicleMakes.DeleteByIdAsync(id);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteVehicleModelAsync(VehicleModel model)
        {
            _uow.VehicleModels.Delete(model);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteVehicleModelAsync(int id)
        {
            await _uow.VehicleModels.DeleteByIdAsync(id);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteVehicleModelYearAsync(VehicleModelYear modelYear)
        {
            _uow.VehicleModelYears.Delete(modelYear);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteVehicleModelYearAsync(int id)
        {
            await _uow.VehicleModelYears.DeleteByIdAsync(id);
            await _uow.SaveChangesAsync();
        }

        public async Task<Vehicle> EditVehicleAsync(Vehicle vehicle)
        {
            _uow.Vehicles.Edit(vehicle);
            await _uow.SaveChangesAsync();
            return vehicle;
        }

        public async Task<VehicleMake> EditVehicleMakeAsync(VehicleMake make)
        {
            _uow.VehicleMakes.Edit(make);
            await _uow.SaveChangesAsync();
            return make;
        }

        public async Task<VehicleModel> EditVehicleModelAsync(VehicleModel model)
        {
            _uow.VehicleModels.Edit(model);
            await _uow.SaveChangesAsync();
            return model;
        }

        public async Task<VehicleModelYear> EditVehicleModelYearAsync(VehicleModelYear modelYear)
        {
            _uow.VehicleModelYears.Edit(modelYear);
            await _uow.SaveChangesAsync();
            return modelYear;
        }

        public async Task<List<VehicleMake>> GetAllVehicleMakesAsync()
        {
            return await _uow.VehicleMakes.GetAll().ToListAsync();
        }

        public async Task<PaginatedList<VehicleModel>> GetAllVehicleModelsAsync(int pageIndex, int pageSize)
        {
            return await _uow.VehicleModels.GetAll().OrderBy(v => v.Make.Name).ThenBy(v => v.Name).ToPaginatedListAsync(pageIndex, pageSize);
        }

        public async Task<PaginatedList<Vehicle>> GetAllVehiclesAsync(int pageIndex, int pageSize)
        {
            return await _uow.Vehicles.GetAll().OrderBy(v => v.Id).ToPaginatedListAsync(pageIndex, pageSize);
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            return await _uow.Vehicles.FindBy(v => v.Id == id).SingleAsync();
        }

        public async Task<VehicleMake> GetVehicleMakeByIdAsync(int id)
        {
            return await _uow.VehicleMakes.FindBy(v => v.Id == id).SingleAsync();
        }

        public async Task<VehicleModel> GetVehicleModelByIdAsync(int id)
        {
            return await _uow.VehicleModels.FindBy(v => v.Id == id).SingleAsync();
        }

        public async Task<List<VehicleModel>> GetVehicleModelsForMakeAndYearAsync(VehicleMake make, int year)
        {
            return await _uow.VehicleModelYears
                .FindBy(v => v.Model.MakeId == make.Id && v.Year == year)
                .GroupBy(v => v.Model)
                .Select(v => v.Key)
                .ToListAsync();
        }

        public async Task<List<VehicleModel>> GetVehicleModelsForMakeAsync(VehicleMake make)
        {
            return await _uow.VehicleModels.FindBy(v => v.MakeId == make.Id).ToListAsync();
        }

        public async Task<VehicleModelYear> GetVehicleModelYearByIdAsync(int id)
        {
            return await _uow.VehicleModelYears.FindBy(v => v.Id == id).SingleAsync();
        }

        public async Task<List<VehicleModelYear>> GetVehicleModelYearsForModelAsync(VehicleModel model)
        {
            return await _uow.VehicleModelYears.FindBy(v => v.ModelId == model.Id).ToListAsync();
        }

        public async Task<List<Vehicle>> GetVehiclesForOwnerAsync(User owner)
        {
            return await _uow.Vehicles.FindBy(v => v.OwnerId == owner.Id).ToListAsync();
        }
    }
}
