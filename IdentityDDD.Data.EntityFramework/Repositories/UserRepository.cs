using IdentityDDD.Domain.Entities;
using IdentityDDD.Domain.Repositories;
using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDDD.Data.EntityFramework.Repositories
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        internal UserRepository(DbContext context)
            : base(context)
        {
        }

        public User FindByUserName(string username)
        {
            return GetSingle(u => u.UserName == username);
        }

        public Task<User> FindByUserNameAsync(string username)
        {
            return GetSingleAsync(u => u.UserName == username);
        }

        public Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username)
        {
            return GetSingleAsync(cancellationToken, u => u.UserName == username);
        }

        public User FindByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByEmailAsync(System.Threading.CancellationToken cancellationToken, string email)
        {
            throw new NotImplementedException();
        }
    }
}
