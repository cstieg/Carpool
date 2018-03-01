using Carpool.Domain.Models;
using Carpool.Domain.Repository;
using Carpool.Services.Services;
using Carpool.Services.Tests.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Carpool.Services.Tests
{
    [TestClass]
    public class RideServiceTest
    {
        private RideService _rideService;
        private TransactionScope _transactionScope;
        private CarpoolTestContext _context;

        // test data
        private User _testOwner;
        private User _testRider;
        private Ride _testRide1;
        private Ride _testRide2;
        private Ride _testRide3;
        private RideLeg _testRideLeg1;
        private RideLeg _testRideLeg2;
        private RideLeg _testRideLeg3;
        private RideLeg _testRideLeg4;
        private RideLeg _testRideLeg5;
        private RideLeg _testRideLeg6;
        private RideLeg _testRideLeg7;
        private RideCost _testDefaultRideCost;
        private RideCost _testRideCost;
        private Location _testLocation1;
        private Location _testLocation2;
        private Location _testLocation3;
        private Location _testLocation4;
        private Location _testLocation5;


        [TestInitialize]
        public void Initialize()
        {
            _context = new CarpoolTestContext();
            var uow = new UnitOfWork(_context);
            _rideService = new RideService(uow);
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
        public async Task AddRideAsync()
        {
            // Arrange

            // Act
            await _rideService.AddRideAsync(_testRide1);

            // Assert
            var testRideInDb = await _context.Rides.FirstAsync(r => r.Id == _testRide1.Id);
            Assert.AreEqual(3, testRideInDb.AvailableSeats);
            Assert.AreEqual((byte)WeekDay.WeekDays, testRideInDb.Repeating);
            Assert.AreEqual(_testOwner.Name, testRideInDb.Creator.Name);
            Assert.AreEqual(true, testRideInDb.IsSeekingRiders);
        }

        [TestMethod]
        public async Task AddRideLegAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            await _context.SaveChangesAsync();

            // Act
            await _rideService.AddRideLegsAsync(_testRide1, _testRideLeg1, _testRideLeg2);

            // Assert
            var testRideInDb = await _context.Rides.Include(r => r.RideLegs).FirstAsync(r => r.Id == _testRide1.Id);
            Assert.AreEqual(2, testRideInDb.RideLegs.Count);
            Assert.AreEqual(2, await _context.RideLegs.CountAsync());
            Assert.AreEqual(1, await _context.Rides.CountAsync());
            Assert.AreEqual(1, testRideInDb.RideLegs.Count(r => r.Origin.Address.Line1 == _testLocation1.Address.Line1));
            Assert.AreEqual(1, testRideInDb.RideLegs.Count(r => r.Origin.Address.Line1 == _testLocation2.Address.Line1));
        }
        
        [TestMethod]
        public async Task AddRiderAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.Users.Add(_testOwner);
            _context.Users.Add(_testRider);
            await _context.SaveChangesAsync();

            // Act
            var rider = await _rideService.AddRiderAsync(_testRide1, _testRider);

            // Assert
            Assert.AreNotEqual(0, rider.Id);
            Assert.AreEqual(_testRider.Name, rider.User.Name);
            Assert.AreEqual(_testRide1.Id, rider.RideId);
        }

        [TestMethod]
        public async Task DeleteRideAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            _context.Users.Add(_testOwner);
            _context.Users.Add(_testRider);
            _context.Riders.Add(new Rider() { Ride = _testRide1, User = _testRider });
            await _context.SaveChangesAsync();

            // Act
            await _rideService.DeleteRideAsync(_testRide1);

            // Assert
            Assert.IsFalse(await _context.Rides.AnyAsync());
            Assert.IsFalse(await _context.RideLegs.AnyAsync());
            Assert.IsFalse(await _context.Riders.AnyAsync());
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testOwner.Name));
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testRider.Name));
        }

        [TestMethod]
        public async Task DeleteRideAsyncById()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            _context.Users.Add(_testOwner);
            _context.Users.Add(_testRider);
            _context.Riders.Add(new Rider() { Ride = _testRide1, User = _testRider });
            await _context.SaveChangesAsync();

            // Act
            await _rideService.DeleteRideAsync(_testRide1.Id);

            // Assert
            Assert.IsFalse(await _context.Rides.AnyAsync());
            Assert.IsFalse(await _context.RideLegs.AnyAsync());
            Assert.IsFalse(await _context.Riders.AnyAsync());
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testOwner.Name));
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testRider.Name));
        }

        [TestMethod]
        public async Task DeleteRideLegAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            _context.Locations.Add(_testLocation1);
            _context.Locations.Add(_testLocation2);
            _context.Users.Add(_testOwner);
            _context.Users.Add(_testRider);
            _context.Riders.Add(new Rider() { Ride = _testRide1, User = _testRider });
            await _context.SaveChangesAsync();

            // Act
            await _rideService.DeleteRideLegAsync(_testRideLeg2);

            // Assert
            Assert.IsFalse(await _context.RideLegs.AnyAsync(r => r.Origin.Id == _testRideLeg2.OriginId));
            Assert.AreEqual(1, await _context.RideLegs.CountAsync(r => r.Origin.Id == _testRideLeg1.Origin.Id));
            Assert.AreEqual(1, await _context.Rides.CountAsync(r => r.Id == _testRide1.Id));
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testOwner.Name));
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testRider.Name));
            Assert.AreEqual(1, await _context.Riders.CountAsync(r => r.RideId == _testRide1.Id && r.UserId == _testRider.Id));
        }

        [TestMethod]
        public async Task DeleteRideLegAsyncById()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            _context.Users.Add(_testOwner);
            _context.Users.Add(_testRider);
            _context.Riders.Add(new Rider() { Ride = _testRide1, User = _testRider });
            await _context.SaveChangesAsync();

            // Act
            await _rideService.DeleteRideLegAsync(_testRideLeg2.Id);

            // Assert
            Assert.IsFalse(await _context.RideLegs.AnyAsync(r => r.Origin.Id == _testRideLeg2.OriginId));
            Assert.AreEqual(1, await _context.RideLegs.CountAsync(r => r.Origin.Id == _testRideLeg1.Origin.Id));
            Assert.AreEqual(1, await _context.Rides.CountAsync(r => r.Id == _testRide1.Id));
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testOwner.Name));
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testRider.Name));
            Assert.AreEqual(1, await _context.Riders.CountAsync(r => r.RideId == _testRide1.Id && r.UserId == _testRider.Id));
        }

        [TestMethod]
        public async Task DeleteRiderAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            _context.Users.Add(_testOwner);
            _context.Users.Add(_testRider);
            var rider =_context.Riders.Add(new Rider() { Ride = _testRide1, User = _testRider });
            await _context.SaveChangesAsync();

            // Act
            await _rideService.DeleteRiderAsync(rider);

            // Assert
            Assert.AreEqual(1, await _context.RideLegs.CountAsync(r => r.Origin.Name == _testRideLeg1.Origin.Name));
            Assert.AreEqual(1, await _context.RideLegs.CountAsync(r => r.Origin.Name == _testRideLeg2.Origin.Name));
            Assert.AreEqual(1, await _context.Rides.CountAsync(r => r.Id == _testRide1.Id));
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testOwner.Name));
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testRider.Name));
            Assert.IsFalse(await _context.Riders.AnyAsync());
        }

        [TestMethod]
        public async Task DeleteRiderAsyncById()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            _context.Users.Add(_testOwner);
            _context.Users.Add(_testRider);
            var rider = _context.Riders.Add(new Rider() { Ride = _testRide1, User = _testRider });
            await _context.SaveChangesAsync();

            // Act
            await _rideService.DeleteRiderAsync(rider.Id);

            // Assert
            Assert.AreEqual(1, await _context.RideLegs.CountAsync(r => r.Origin.Name == _testRideLeg1.Origin.Name));
            Assert.AreEqual(1, await _context.RideLegs.CountAsync(r => r.Origin.Name == _testRideLeg2.Origin.Name));
            Assert.AreEqual(1, await _context.Rides.CountAsync(r => r.Id == _testRide1.Id));
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testOwner.Name));
            Assert.AreEqual(1, await _context.Users.CountAsync(u => u.Name == _testRider.Name));
            Assert.IsFalse(await _context.Riders.AnyAsync());
        }

        [TestMethod]
        public async Task EditRideAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            await _context.SaveChangesAsync();

            var startDate = new DateTime(2018, 12, 25);
            _testRide1.StartDate = startDate;

            // Act
            var ride = await _rideService.EditRideAsync(_testRide1);

            // Assert
            Assert.IsTrue(ride.StartDate.Equals(startDate));
        }

        [TestMethod]
        public async Task EditRideLegAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            await _context.SaveChangesAsync();

            var startTime = new TimeSpan(11, 0, 0);
            _testRideLeg2.StartTime = startTime;

            // Act
            var rideLeg = await _rideService.EditRideLegAsync(_testRideLeg2);

            // Assert
            Assert.IsTrue(rideLeg.StartTime.Equals(startTime));
            Assert.AreEqual(1, await _context.RideLegs.CountAsync(r => r.Origin.Name == _testRideLeg1.Origin.Name));
            Assert.AreEqual(1, await _context.RideLegs.CountAsync(r => r.Origin.Name == _testRideLeg2.Origin.Name));
            Assert.AreEqual(1, await _context.Rides.CountAsync(r => r.Id == _testRide1.Id));
        }

        [TestMethod]
        public async Task FindMatchingRidesAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.Rides.Add(_testRide2);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            _context.RideLegs.Add(_testRideLeg3);
            _context.RideLegs.Add(_testRideLeg4);
            _context.RideLegs.Add(_testRideLeg5);
            await _context.SaveChangesAsync();

            // Act
            var rides = await _rideService.FindMatchingRidesAsync(_testRide3, 10, new TimeSpan(3,0,0));

            // Assert
            Assert.AreEqual(_testRide1.Id, rides.First().Id);
        }

        [TestMethod]
        public async Task GetRideByIdAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            await _context.SaveChangesAsync();

            // Act
            var ride = await _rideService.GetRideByIdAsync(_testRide1.Id);

            // Assert
            Assert.AreEqual(_testRide1.Id, ride.Id);
            Assert.IsTrue(ride.RideLegs.Any(r => r.Id == _testRideLeg1.Id));
            Assert.IsTrue(ride.RideLegs.Any(r => r.Id == _testRideLeg2.Id));
        }

        [TestMethod]
        public async Task GetRideCostAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            _context.RideCosts.Add(_testRideCost);
            _testRide1.RideCost = _testRideCost;
            await _context.SaveChangesAsync();

            // Act
            var cost = await _rideService.GetRideCostAsync(_testRide1, _testRide3);

            // Assert
            Assert.IsTrue(cost > 14 && cost < 17);
        }

        [TestMethod]
        public async Task GetRidersForRideAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.Users.Add(_testOwner);
            _context.Users.Add(_testRider);
            _context.Riders.Add(new Rider() { Ride = _testRide1, User = _testRider });
            await _context.SaveChangesAsync();

            // Act
            var riders = await _rideService.GetRidersForRideAsync(_testRide1);

            // Assert
            Assert.AreEqual(1, riders.Count);
            Assert.AreEqual(_testRider.Id, riders.First().User.Id);
            Assert.AreEqual(_testRide1.Id, riders.First().Ride.Id);
        }

        [TestMethod]
        public async Task SetDefaultRideCostForUserAsync()
        {
            // Arrange
            _context.Users.Add(_testOwner);
            await _context.SaveChangesAsync();

            // Act
            var rideCost = await _rideService.SetDefaultRideCostForUserAsync(_testOwner, _testRideCost);

            // Assert
            var user = await _context.Users.SingleAsync(u => u.Id == _testOwner.Id);
            Assert.AreEqual(0.20M, user.DefaultRideCost.CostPerMile);
        }

        [TestMethod]
        public async Task SetRideCostForRideAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.Users.Add(_testOwner);
            await _context.SaveChangesAsync();

            // Act
            var rideCost = await _rideService.SetRideCostForRideAsync(_testRide1, _testRideCost);

            // Assert
            var ride = await _context.Rides.SingleAsync(r => r.Id == _testRide1.Id);
            Assert.AreEqual(0.20M, ride.RideCost.CostPerMile);
        }

        [TestMethod]
        public async Task AddRideRequestToOfferAsync()
        {
            // Arrange
            _context.Rides.Add(_testRide1);
            _context.Rides.Add(_testRide3);
            _context.RideLegs.Add(_testRideLeg1);
            _context.RideLegs.Add(_testRideLeg2);
            _context.RideLegs.Add(_testRideLeg6);
            _context.RideLegs.Add(_testRideLeg7);
            await _context.SaveChangesAsync();

            // Act
            var newRide = await _rideService.AddRideRequestToOfferAsync(_testRide1, _testRide3);

            // Assert
            Assert.AreEqual(1, newRide.RideLegs.Count(rl => rl.Origin.Name == _testRideLeg1.Origin.Name
                                                   && rl.Destination.Name == _testRideLeg6.Origin.Name));
            Assert.AreEqual(1, newRide.RideLegs.Count(rl => rl.Origin.Name == _testRideLeg6.Origin.Name
                                                   && rl.Destination.Name == _testRideLeg6.Destination.Name));
            Assert.AreEqual(1, newRide.RideLegs.Count(rl => rl.Origin.Name == _testRideLeg6.Destination.Name
                                                   && rl.Destination.Name == _testRideLeg1.Destination.Name));
            Assert.AreEqual(1, newRide.RideLegs.Count(rl => rl.Origin.Name == _testRideLeg2.Origin.Name
                                                   && rl.Destination.Name == _testRideLeg7.Origin.Name));
            Assert.AreEqual(1, newRide.RideLegs.Count(rl => rl.Origin.Name == _testRideLeg7.Origin.Name
                                                   && rl.Destination.Name == _testRideLeg7.Destination.Name));
            Assert.AreEqual(1, newRide.RideLegs.Count(rl => rl.Origin.Name == _testRideLeg7.Destination.Name
                                                   && rl.Destination.Name == _testRideLeg2.Destination.Name));
        }

        private void SetUpTestData()
        {
            DateTime startDate = new DateTime(2000, 1, 1);
            DateTime endDate = new DateTime(2029, 1, 1);

            _testOwner = new User() { Name = "Me", Email = "test@email.com", IsDriver = true, IsRider = false, IdentityUserId = Guid.NewGuid() };
            _testRider = new User() { Name = "Somebody", Email = "somebody@else.com", IsDriver = false, IsRider = true, IdentityUserId = Guid.NewGuid() };

            _testLocation1 = new Location()
            {
                Address = new Address()
                {
                    Line1 = "10236 180th Ave",
                    City = "LeRoy",
                    State = "MI",
                    PostalCode = "49655",
                    Country = "US"
                },
                LatLng = new LatLng()
                {
                    Lat = 43.96299F,
                    Lng = -85.44361F
                },
                Name = "Home",
                Owner = _testOwner
            };
            _testLocation2 = new Location()
            {
                Address = new Address()
                {
                    Line1 = "415 N Mitchell",
                    City = "Cadillac",
                    State = "MI",
                    PostalCode = "49601",
                    Country = "US"
                },
                LatLng = new LatLng()
                {
                    Lat = 44.253227F,
                    Lng = -85.40183F
                },
                Name = "Work",
                Owner = _testOwner
            };
            _testLocation3 = new Location()
            {
                Address = new Address()
                {
                    Line1 = "21400 Perry Ave",
                    City = "Big Rapids",
                    State = "MI",
                    PostalCode = "496307",
                    Country = "US"

                },
                LatLng = new LatLng()
                {
                    Lat = 43.687443F,
                    Lng = -85.51758F
                },
                Name = "WalMart Big Rapids"
            };
            _testLocation4 = new Location()
            {
                Address = new Address()
                {
                    Line1 = "130 Fulton St W",
                    City = "Grand Rapids",
                    State = "MI",
                    PostalCode = "49503",
                    Country = "US"
                },
                LatLng = new LatLng()
                {
                    Lat = 42.96322F,
                    Lng = -85.67093F
                },
                Name = "Van Andel Arena"
            };
            _testLocation5 = new Location()
            {
                Address = new Address()
                {
                    Line1 = "8917 E 34 Rd",
                    City = "Cadillac",
                    State = "MI",
                    PostalCode = "49601",
                    Country = "US"
                },
                LatLng = new LatLng()
                {
                    Lat = 44.281467F,
                    Lng = -85.397835F
                },
                Name = "WalMart Cadillac"
            };

            _testRide1 = new Ride()
            {
                Creator = _testOwner,
                Driver = _testOwner,
                StartDate = startDate,
                EndDate = endDate,
                AvailableSeats = 3,
                IsSeekingRiders = true,
                IsSeekingDriver = false,
                Repeating = (byte) WeekDay.WeekDays
            };
            _testRide2 = new Ride()
            {
                Creator = _testOwner,
                Driver = _testOwner,
                StartDate = startDate,
                EndDate = endDate,
                AvailableSeats = 3,
                IsSeekingRiders = true,
                IsSeekingDriver = false,
                Repeating = (byte)WeekDay.WeekDays
            };
            _testRide3 = new Ride()
            {
                Creator = _testRider,
                IsSeekingRiders = false,
                IsSeekingDriver = true,
                Repeating = (byte)WeekDay.WeekDays
            };

            _testRideLeg1 = new RideLeg()
            {
                Ride = _testRide1,
                Origin = _testLocation1,
                Destination = _testLocation2,
                StartTime = new TimeSpan(8, 0, 0),
                TravelTime = new TimeSpan(0, 25, 0),
                Distance = 20F
            };
            _testRideLeg2 = new RideLeg()
            {
                Ride = _testRide1,
                Origin = _testLocation2,
                Destination = _testLocation1,
                StartTime = new TimeSpan(17, 0, 0),
                TravelTime = new TimeSpan(0, 25, 0),
                Distance = 20F
            };
            _testRideLeg3 = new RideLeg()
            {
                Ride = _testRide2,
                Origin = _testLocation1,
                Destination = _testLocation3,
                StartTime = new TimeSpan(8, 0, 0),
                TravelTime = new TimeSpan(0, 25, 0)
            };
            _testRideLeg4 = new RideLeg()
            {
                Ride = _testRide2,
                Origin = _testLocation3,
                Destination = _testLocation4,
                StartTime = new TimeSpan(12, 0, 0),
                TravelTime = new TimeSpan(0, 25, 0)
            };
            _testRideLeg5 = new RideLeg()
            {
                Ride = _testRide2,
                Origin = _testLocation4,
                Destination = _testLocation1,
                StartTime = new TimeSpan(17, 0, 0),
                TravelTime = new TimeSpan(0, 25, 0)
            };
            _testRideLeg6 = new RideLeg()
            {
                Ride = _testRide3,
                Origin = _testLocation1,
                Destination = _testLocation5,
                StartTime = new TimeSpan(8, 0, 0),
                TravelTime = new TimeSpan(0, 25, 0),
                Distance = 22F
            };
            _testRideLeg7 = new RideLeg()
            {
                Ride = _testRide3,
                Origin = _testLocation5,
                Destination = _testLocation1,
                StartTime = new TimeSpan(17, 0, 0),
                TravelTime = new TimeSpan(0, 25, 0),
                Distance = 22F
            };

            _testRideCost = new RideCost()
            {
                BaseCost = 2.00M,
                CostPerMile = 0.20M,
                PickupCostPerHour = 20M,
                PickupCostPerMile = .25M,
                PickupDistanceMileLimit = 20,
                PickupTimeLimitMinutes = 30
            };

        }
    }
}
