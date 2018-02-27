using Carpool.Domain.Models;
using Carpool.Domain.Repository;
using Carpool.Domain.Services.Interfaces;
using Cstieg.ObjectHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpool.Domain.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _uow;

        public MembershipService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _uow.Users.Add(user);
            await _uow.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User must not be null");
            }

            var userInDb = await GetUserAsync(user.Id);
            if (userInDb == null)
            {
                throw new NotFoundException("User " + user.Id.ToString() + " is not found.");
            }

            ObjectHelper.CopyProperties(user, userInDb, exclude: new List<string>() { "Id" }, excludeNulls: true);

            _uow.Users.Edit(user);
            await _uow.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _uow.Users.GetSingleAsync(id);
        }

        public async Task<User> GetUserAsync(string name)
        {
            return await _uow.Users.GetSingleByUsernameAsync(name);
        }

        public async Task<User> GetUserAsync(Guid guid)
        {
            return await _uow.Users.GetSingleByGuidAsync(guid);
        }

        public async Task<PaginatedList<User>> GetAllUsersAsync(int pageIndex, int pageSize)
        {
            return await _uow.Users.PaginateAsync(pageIndex, pageSize, u => u.Name);
        }
    }
}
