using Carpool.Domain.Models;
using Carpool.Domain.Repository;
using Carpool.Domain.Services;
using Carpool.Services.Tests.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Transactions;

namespace Carpool.Services.Tests
{
    [TestClass]
    public class MembershipServiceTest
    {
        private MembershipService _membershipService;
        private TransactionScope _transactionScope;
        private CarpoolTestContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = new CarpoolTestContext();
            var uow = new UnitOfWork(_context);
            _membershipService = new MembershipService(uow);
            _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Transaction.Current.Rollback();
            _transactionScope.Dispose();
        }

        [TestMethod]
        public async Task CreateUserAsync()
        {
            // Arrange
            var user = new User()
            {
                IsDriver = true,
                IsRider = true,
                Email = "My.Email@example.com",
                Name = "Me",
                IdentityUserId = Guid.NewGuid()
            };

            // Act
            await _membershipService.CreateUserAsync(user);

            // Assert
            var userInDb = await _context.Users.FirstAsync();
            Assert.IsTrue(userInDb.IsDriver == user.IsDriver
                        && userInDb.IsRider == user.IsRider
                        && userInDb.Email == user.Email
                        && userInDb.Name == user.Name
                        && userInDb.IdentityUserId == user.IdentityUserId);
            Assert.AreEqual(1, await _context.Users.CountAsync());
        }

        [TestMethod]
        public async Task UpdateUserAsync()
        {
            // Arrange
            var user = new User()
            {
                IsDriver = true,
                IsRider = true,
                Email = "My.Email@example.com",
                Name = "Me",
                IdentityUserId = Guid.NewGuid()
            };
            var userAdded = _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.Id = userAdded.Id;
            user.Name = "Myself";

            // Act
            await _membershipService.UpdateUserAsync(user);

            // Assert
            var userInDb = await _context.Users.FirstAsync();
            Assert.AreEqual("Myself", userInDb.Name);
            Assert.AreEqual(1, await _context.Users.CountAsync());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task UpdateUserAsyncThrowsWhenUserNotFound()
        {
            // Arrange
            var user = new User()
            {
                Id = 1,
                IsDriver = true,
                IsRider = true,
                Email = "My.Email@example.com",
                Name = "Me",
                IdentityUserId = Guid.NewGuid()
            };
            user.Name = "Myself";

            var users = await _context.Users.ToListAsync();

            // Act
            await _membershipService.UpdateUserAsync(user);

            // Assert
        }

        [TestMethod]
        public async Task GetUserAsyncById()
        {
            // Arrange
            var user = new User()
            {
                IsDriver = true,
                IsRider = true,
                Email = "My.Email@example.com",
                Name = "Me",
                IdentityUserId = Guid.NewGuid()
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var userInDb = await _membershipService.GetUserAsync(user.Id);

            // Assert
            Assert.AreEqual(user.Id, userInDb.Id);
            Assert.AreEqual(user.Name, userInDb.Name);
            Assert.AreEqual(user.Email, userInDb.Email);
            Assert.AreEqual(user.IsDriver, userInDb.IsDriver);
            Assert.AreEqual(user.IsRider, userInDb.IsRider);
        }

        [TestMethod]
        public async Task GetUserAsyncByName()
        {
            // Arrange
            var user = new User()
            {
                IsDriver = true,
                IsRider = true,
                Email = "My.Email@example.com",
                Name = "Me",
                IdentityUserId = Guid.NewGuid()
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var userInDb = await _membershipService.GetUserAsync("Me");

            // Assert
            Assert.AreEqual(user.Id, userInDb.Id);
            Assert.AreEqual(user.Name, userInDb.Name);
            Assert.AreEqual(user.Email, userInDb.Email);
            Assert.AreEqual(user.IsDriver, userInDb.IsDriver);
            Assert.AreEqual(user.IsRider, userInDb.IsRider);
        }

        [TestMethod]
        public async Task GetUsersAsync()
        {
            // Arrange
            var user = new User()
            {
                IsDriver = true,
                IsRider = true,
                Email = "My.Email@example.com",
                Name = "Me",
                IdentityUserId = Guid.NewGuid()
            };
            var user2 = new User()
            {
                IsDriver = false,
                IsRider = false,
                Email = "My.Email1@example.com",
                Name = "Myself",
                IdentityUserId = Guid.NewGuid()
            };
            _context.Users.Add(user);
            _context.Users.Add(user2);
            await _context.SaveChangesAsync();

            // Act
            var usersInDb = await _membershipService.GetAllUsersAsync(1, 10);

            // Assert
            Assert.AreEqual(2, usersInDb.Count);

        }
    }
}
