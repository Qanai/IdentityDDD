using IdentityDDD.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDDD.Domain.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Role FindByName(string roleName);
        Task<Role> FindByNameAsync(string roleName);
        Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName);
    }
}
