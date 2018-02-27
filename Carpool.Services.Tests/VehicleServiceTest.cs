using Carpool.Domain.Models;
using Carpool.Domain.Repository;
using Carpool.Services.Services;
using Carpool.Services.Tests.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Transactions;

namespace Carpool.Services.Tests
{
    [TestClass]
    public class VehicleServiceTest
    {
        private VehicleService _vehicleService;
        private TransactionScope _transactionScope;
        private CarpoolTestContext _context;

        // test data
        private Vehicle _testVehicle1;
        private Vehicle _testVehicle2;
        private Vehicle _testVehicle3;
        private Vehicle _testVehicle4;
        private VehicleMake _testMake1;
        private VehicleMake _testMake2;
        private VehicleModel _testModel1;
        private VehicleModel _testModel2;
        private VehicleModel _testModel3;
        private VehicleModelYear _testModelYear1;
        private VehicleModelYear _testModelYear2;
        private VehicleModelYear _testModelYear3;
        private VehicleModelYear _testModelYear4;
        private User _testUser;

        [TestInitialize]
        public void Initialize()
        {
            _context = new CarpoolTestContext();
            var uow = new UnitOfWork(_context);
            _vehicleService = new VehicleService(uow);
            _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            SetUpTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Transaction.Current.Rollback();
            _transactionScope.Dispose();
        }

        [TestMethod]
        public async Task AddVehicleAsync()
        {
            // Arrange
            var vehicle = _testVehicle1;

            // Act
            await _vehicleService.AddVehicleAsync(vehicle);

            // Assert
            var vehiclesInDb = await _context.Vehicles.ToListAsync();
            var vehicleInDb = vehiclesInDb[0];
            Assert.AreEqual(1, vehiclesInDb.Count);
            Assert.AreEqual(vehicle.ModelYear.Model.Make.Name, vehicleInDb.ModelYear.Model.Make.Name);
            Assert.AreEqual(vehicle.Condition, vehicleInDb.Condition);
        }

        [TestMethod]
        public async Task EditVehicleAsync()
        {
            // Arrange
            _context.Vehicles.Add(_testVehicle1);
            await _context.SaveChangesAsync();

            var vehicle = _testVehicle1;
            vehicle.Condition = "Fair";

            // Act
            await _vehicleService.EditVehicleAsync(vehicle);

            // Assert
            Assert.AreEqual(1, (await _context.Vehicles.ToListAsync()).Count);
            var vehicleInDb = await _context.Vehicles.FirstAsync(v => v.Id == vehicle.Id);
            Assert.AreEqual(vehicle.ModelYear.Model.Make.Name, vehicleInDb.ModelYear.Model.Make.Name);
            Assert.AreEqual(vehicle.Condition, vehicleInDb.Condition);
        }

        [TestMethod]
        public async Task DeleteVehicleByModelAsync()
        {
            // Arrange
            _context.Vehicles.Add(_testVehicle1);
            await _context.SaveChangesAsync();

            // Act
            await _vehicleService.DeleteVehicleAsync(_testVehicle1);

            // Assert
            Assert.IsFalse(await _context.Vehicles.AnyAsync());
        }

        [TestMethod]
        public async Task DeleteVehicleByIdAsync()
        {
            // Arrange
            _context.Vehicles.Add(_testVehicle1);
            await _context.SaveChangesAsync();

            // Act
            await _vehicleService.DeleteVehicleAsync(_testVehicle1.Id);

            // Assert
            Assert.IsFalse(await _context.Vehicles.AnyAsync());
        }

        [TestMethod]
        public async Task GetAllVehiclesAsync()
        {
            // Arrange
            _context.Vehicles.Add(_testVehicle1);
            _context.Vehicles.Add(_testVehicle2);
            _context.Vehicles.Add(_testVehicle3);
            _context.Vehicles.Add(_testVehicle4);
            await _context.SaveChangesAsync();

            // Act
            var vehicles = await _vehicleService.GetAllVehiclesAsync(1, 10);

            // Assert
            Assert.AreEqual(4, vehicles.Count);
        }

        [TestMethod]
        public async Task GetVehiclesForOwnerAsync()
        {
            // Arrange
            _context.Vehicles.Add(_testVehicle1);
            _context.Vehicles.Add(_testVehicle2);
            _context.Vehicles.Add(_testVehicle3);
            _context.Vehicles.Add(_testVehicle4);
            await _context.SaveChangesAsync();

            // Act
            var vehicles = await _vehicleService.GetVehiclesForOwnerAsync(_testUser);

            // Assert
            Assert.AreEqual(3, vehicles.Count);
            Assert.AreEqual(_testUser.Name, vehicles[0].Owner.Name);
        }

        [TestMethod]
        public async Task GetVehiclesByIdAsync()
        {
            // Arrange
            _context.Vehicles.Add(_testVehicle1);
            await _context.SaveChangesAsync();

            // Act
            var vehicle = await _vehicleService.GetVehicleByIdAsync(_testVehicle1.Id);

            // Assert
            Assert.IsNotNull(vehicle);
            Assert.AreEqual(_testVehicle1.ModelYear.ToString(), vehicle.ModelYear.ToString());
        }

        [TestMethod]
        public async Task AddVehicleMakeAsync()
        {
            // Arrange

            // Act
            var vehicleMake = await _vehicleService.AddVehicleMakeAsync(_testMake1);

            // Assert
            var vehicleMakesInDb = await _context.VehicleMakes.ToListAsync();
            Assert.AreEqual(1, vehicleMakesInDb.Count);
            Assert.AreEqual(_testMake1.Name, vehicleMakesInDb[0].Name);
        }

        [TestMethod]
        public async Task EditVehicleMakeAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testMake1);
            await _context.SaveChangesAsync();

            _testMake1.Name = "Chrysler";

            // Act
            var vehicleMake = await _vehicleService.EditVehicleMakeAsync(_testMake1);

            // Assert
            var vehicleMakesInDb = await _context.VehicleMakes.ToListAsync();
            Assert.AreEqual(1, vehicleMakesInDb.Count);
            Assert.AreEqual("Chrysler", vehicleMakesInDb[0].Name);
        }

        [TestMethod]
        public async Task DeleteVehicleMakeAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testMake1);
            await _context.SaveChangesAsync();

            // Act
            await _vehicleService.DeleteVehicleMakeAsync(_testMake1);

            // Assert
            Assert.IsFalse(await _context.VehicleMakes.AnyAsync());
        }

        [TestMethod]
        public async Task DeleteVehicleMakeByIdAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testMake1);
            await _context.SaveChangesAsync();

            // Act
            await _vehicleService.DeleteVehicleMakeAsync(_testMake1.Id);

            // Assert
            Assert.IsFalse(await _context.VehicleMakes.AnyAsync());
        }

        [TestMethod]
        public async Task GetAllVehicleMakesAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testMake1);
            _context.VehicleMakes.Add(_testMake2);
            await _context.SaveChangesAsync();

            // Act
            var vehicleMakes = await _vehicleService.GetAllVehicleMakesAsync();

            // Assert
            Assert.AreEqual(2, vehicleMakes.Count);
        }

        [TestMethod]
        public async Task GetVehicleMakeByIdAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testMake1);
            _context.VehicleMakes.Add(_testMake2);
            await _context.SaveChangesAsync();

            // Act
            var vehicleMake = await _vehicleService.GetVehicleMakeByIdAsync(_testMake1.Id);

            // Assert
            Assert.AreEqual(_testMake1.Name, vehicleMake.Name);
        }

        [TestMethod]
        public async Task AddVehicleModelAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testModel1.Make);
            await _context.SaveChangesAsync();

            // Act
            var vehicleModel = await _vehicleService.AddVehicleModelAsync(_testModel1);

            // Assert
            var vehicleModelsInDb = await _context.VehicleModels.ToListAsync();
            Assert.AreEqual(1, vehicleModelsInDb.Count);
            Assert.AreEqual(_testModel1.Name, vehicleModelsInDb[0].Name);
        }

        [TestMethod]
        public async Task EditVehicleModelAsync()
        {
            // Arrange
            _context.VehicleModels.Add(_testModel1);
            await _context.SaveChangesAsync();

            _testModel1.Name = "Ram";

            // Act
            var vehicleModel = await _vehicleService.EditVehicleModelAsync(_testModel1);

            // Assert
            var vehicleModelsInDb = await _context.VehicleModels.ToListAsync();
            Assert.AreEqual(1, vehicleModelsInDb.Count);
            Assert.AreEqual("Ram", vehicleModelsInDb[0].Name);
        }


        [TestMethod]
        public async Task DeleteVehicleModelAsync()
        {
            // Arrange
            _context.VehicleModels.Add(_testModel1);
            await _context.SaveChangesAsync();

            // Act
            await _vehicleService.DeleteVehicleModelAsync(_testModel1);

            // Assert
            Assert.IsFalse(await _context.VehicleModels.AnyAsync());
        }

        [TestMethod]
        public async Task DeleteVehicleModelByIdAsync()
        {
            // Arrange
            _context.VehicleModels.Add(_testModel1);
            await _context.SaveChangesAsync();

            // Act
            await _vehicleService.DeleteVehicleModelAsync(_testModel1.Id);

            // Assert
            Assert.IsFalse(await _context.VehicleModels.AnyAsync());
        }

        [TestMethod]
        public async Task GetAllVehicleModelsAsync()
        {
            // Arrange
            _context.VehicleModels.Add(_testModel1);
            _context.VehicleModels.Add(_testModel2);
            _context.VehicleModels.Add(_testModel3);
            await _context.SaveChangesAsync();

            // Act
            var vehicleModels = await _vehicleService.GetAllVehicleModelsAsync(1, 10);

            // Assert
            Assert.AreEqual(3, vehicleModels.Count);
            Assert.IsNotNull(vehicleModels[0].Make);
            Assert.AreNotEqual("", vehicleModels[0].Name);
        }

        [TestMethod]
        public async Task GetVehicleModelsForMakeAsync()
        {
            // Arrange
            _context.VehicleModels.Add(_testModel1);
            _context.VehicleModels.Add(_testModel2);
            _context.VehicleModels.Add(_testModel3);
            await _context.SaveChangesAsync();

            // Act
            var vehicleModels = await _vehicleService.GetVehicleModelsForMakeAsync(_testMake2);

            // Assert
            Assert.AreEqual(2, vehicleModels.Count);
            Assert.AreEqual("Ford", vehicleModels[0].Make.Name);
        }

        [TestMethod]
        public async Task GetVehicleModelsForMakeAndYearAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testMake1);
            _context.VehicleMakes.Add(_testMake2);
            _context.VehicleModels.Add(_testModel1);
            _context.VehicleModels.Add(_testModel2);
            _context.VehicleModels.Add(_testModel3);
            _context.VehicleModelYears.Add(_testModelYear1);
            _context.VehicleModelYears.Add(_testModelYear2);
            _context.VehicleModelYears.Add(_testModelYear3);
            _context.VehicleModelYears.Add(_testModelYear4);
            await _context.SaveChangesAsync();

            // Act
            var vehicleModels = await _vehicleService.GetVehicleModelsForMakeAndYearAsync(_testMake2, 1991);

            // Assert
            Assert.AreEqual(1, vehicleModels.Count);
            Assert.AreEqual("Ford", vehicleModels[0].Make.Name);
            Assert.AreEqual("Tempo", vehicleModels[0].Name);
        }

        [TestMethod]
        public async Task GetVehicleModelByIdAsync()
        {
            // Arrange
            _context.VehicleModels.Add(_testModel1);
            _context.VehicleModels.Add(_testModel2);
            _context.VehicleModels.Add(_testModel3);
            await _context.SaveChangesAsync();

            // Act
            var vehicleModel = await _vehicleService.GetVehicleModelByIdAsync(_testModel2.Id);

            // Assert
            Assert.AreEqual("Ford", vehicleModel.Make.Name);
            Assert.AreEqual("Tempo", vehicleModel.Name);
        }

        [TestMethod]
        public async Task AddVehicleModelYearAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testModelYear1.Model.Make);
            _context.VehicleModels.Add(_testModelYear1.Model);
            await _context.SaveChangesAsync();

            // Act
            var vehicleModelYear = await _vehicleService.AddVehicleModelYearAsync(_testModelYear1);

            // Assert
            var vehicleModelYearsInDb = await _context.VehicleModelYears.ToListAsync();
            Assert.AreEqual(1, vehicleModelYearsInDb.Count);
            Assert.AreEqual(_testModelYear1.Model.Make.Name, vehicleModelYearsInDb[0].Model.Make.Name);
            Assert.AreEqual(_testModelYear1.Year, vehicleModelYearsInDb[0].Year);
        }

        [TestMethod]
        public async Task EditVehicleModelYearAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testModelYear1.Model.Make);
            _context.VehicleModels.Add(_testModelYear1.Model);
            _context.VehicleModelYears.Add(_testModelYear1);
            await _context.SaveChangesAsync();

            _testModelYear1.Year = 2018;

            // Act
            var vehicleModelYear = await _vehicleService.EditVehicleModelYearAsync(_testModelYear1);

            // Assert
            var vehicleModelYearsInDb = await _context.VehicleModelYears.ToListAsync();
            Assert.AreEqual(1, vehicleModelYearsInDb.Count);
            Assert.AreEqual(2018, vehicleModelYearsInDb[0].Year);
        }

        [TestMethod]
        public async Task DeleteVehicleModelYearAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testModelYear1.Model.Make);
            _context.VehicleModels.Add(_testModelYear1.Model);
            _context.VehicleModelYears.Add(_testModelYear1);
            await _context.SaveChangesAsync();

            // Act
            await _vehicleService.DeleteVehicleModelYearAsync(_testModelYear1);

            // Assert
            Assert.IsFalse(await _context.VehicleModelYears.AnyAsync());
        }

        [TestMethod]
        public async Task DeleteVehicleModelYearByIdAsync()
        {
            // Arrange
            _context.VehicleMakes.Add(_testModelYear1.Model.Make);
            _context.VehicleModels.Add(_testModelYear1.Model);
            _context.VehicleModelYears.Add(_testModelYear1);
            await _context.SaveChangesAsync();

            // Act
            await _vehicleService.DeleteVehicleModelYearAsync(_testModelYear1.Id);

            // Assert
            Assert.IsFalse(await _context.VehicleModelYears.AnyAsync());
        }

        [TestMethod]
        public async Task GetVehicleModelYearsForModelAsync()
        {
            // Arrange
            _context.VehicleModelYears.Add(_testModelYear1);
            _context.VehicleModelYears.Add(_testModelYear2);
            _context.VehicleModelYears.Add(_testModelYear3);
            _context.VehicleModelYears.Add(_testModelYear4);
            await _context.SaveChangesAsync();

            // Act
            var vehicleModelYears = await _vehicleService.GetVehicleModelYearsForModelAsync(_testModel3);

            // Assert
            Assert.AreEqual(2, vehicleModelYears.Count);
            Assert.IsTrue(vehicleModelYears.Exists(v => v.Year == 1999));
            Assert.IsTrue(vehicleModelYears.Exists(v => v.Year == 2000));
        }

        [TestMethod]
        public async Task GetVehicleModelYearByIdAsync()
        {
            // Arrange
            _context.VehicleModelYears.Add(_testModelYear1);
            _context.VehicleModelYears.Add(_testModelYear2);
            _context.VehicleModelYears.Add(_testModelYear3);
            _context.VehicleModelYears.Add(_testModelYear4);
            await _context.SaveChangesAsync();

            // Act
            var vehicleModelYear = await _vehicleService.GetVehicleModelYearByIdAsync(_testModelYear4.Id);

            // Assert
            Assert.IsNotNull(vehicleModelYear);
            Assert.AreEqual(2000, vehicleModelYear.Year);
        }
        
        private void SetUpTestData()
        {
            _testUser = new User() { Name = "Me", Email = "test@email.com", IsDriver = true, IsRider = false, IdentityUserId = Guid.NewGuid() };

            _testMake1 = new VehicleMake() { Name = "Dodge" };
            _testModel1 = new VehicleModel() { Make = _testMake1, Name = "Caravan" };
            _testModelYear1 = new VehicleModelYear() { Model = _testModel1, Year = 2008 };
            _testVehicle1 = new Vehicle()
            {
                ModelYear = _testModelYear1,
                Condition = "Good",
                Owner = _testUser
            };

            _testMake2 = new VehicleMake() { Name = "Ford" };
            _testModel2 = new VehicleModel() { Make = _testMake2, Name = "Tempo" };
            _testModelYear2 = new VehicleModelYear() { Model = _testModel2, Year = 1991 };
            _testVehicle2 = new Vehicle()
            {
                ModelYear = _testModelYear2,
                Condition = "Fair",
                Owner = _testUser
            };

            _testModel3 = new VehicleModel() { Make = _testMake2, Name = "Taurus" };
            _testModelYear3 = new VehicleModelYear() { Model = _testModel3, Year = 1999 };
            _testVehicle3 = new Vehicle()
            {
                ModelYear = _testModelYear3,
                Condition = "Fair",
                Owner = _testUser
            };

            _testModelYear4 = new VehicleModelYear() { Model = _testModel3, Year = 2000 };
            _testVehicle4 = new Vehicle()
            {
                ModelYear = _testModelYear2,
                Condition = "Excellent",
                Owner = new User() { Name = "Other user", Email = "Somebody@else.com", IsDriver = true, IsRider = false, IdentityUserId = Guid.NewGuid() }
            };
        }
    }
}
