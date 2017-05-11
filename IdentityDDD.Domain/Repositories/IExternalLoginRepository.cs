using IdentityDDD.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDDD.Domain.Repositories
{
    public interface IExternalLoginRepository : IGenericRepository<ExternalLogin>
    {
        ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey);
        Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey);
        Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider, string providerKey);
    }
}
