using IdentityDDD.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDDD.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User FindByUserName(string username);
        Task<User> FindByUserNameAsync(string username);
        Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username);

        User FindByEmail(string email);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByEmailAsync(CancellationToken cancellationToken, string email);
    }
}
