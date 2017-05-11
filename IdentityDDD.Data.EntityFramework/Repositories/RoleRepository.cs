using IdentityDDD.Domain.Entities;
using IdentityDDD.Domain.Repositories;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDDD.Data.EntityFramework.Repositories
{
    internal class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        internal RoleRepository(DbContext context)
            : base(context)
        {
        }

        public Role FindByName(string roleName)
        {
            return GetSingle(r => r.Name == roleName);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return GetSingleAsync(r => r.Name == roleName);
        }

        public Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName)
        {
            return GetSingleAsync(cancellationToken, r => r.Name == roleName);
        }
    }
}
