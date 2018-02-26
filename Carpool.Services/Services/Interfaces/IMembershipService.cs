using Carpool.Domain.Models;
using Carpool.Domain.Repository;
using System.Threading.Tasks;

namespace Carpool.Domain.Services.Interfaces
{
    interface IMembershipService
    {
        /// <summary>
        /// Creates a user and adds to database
        /// </summary>
        /// <param name="user">The User object to create</param>
        /// <returns>The user created, with Id set</returns>
        Task<User> CreateUserAsync(User user);

        Task<User> UpdateUserAsync(User user);

        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string name);

        Task<PaginatedList<User>> GetAllUsersAsync(int pageIndex, int pageSize);

    }
}
