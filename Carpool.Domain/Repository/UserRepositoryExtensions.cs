using Carpool.Domain.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Carpool.Domain.Repository
{
    public static class UserRepositoryExtensions
    {
        public static async Task<User> GetSingleByUsernameAsync(this IEntityRepository<User> userRepository, string username)
        {
            return await userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == username);
        }

        public static async Task<User> GetSingleByGuidAsync(this IEntityRepository<User> userRepository, Guid guid)
        {
            return await userRepository.GetAll().FirstOrDefaultAsync(x => x.IdentityUserId == guid);
        }
    }
}
