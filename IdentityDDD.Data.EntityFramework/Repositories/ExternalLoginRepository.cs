using IdentityDDD.Domain.Entities;
using IdentityDDD.Domain.Repositories;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDDD.Data.EntityFramework.Repositories
{
    internal class ExternalLoginRepository : GenericRepository<ExternalLogin>, IExternalLoginRepository
    {
        public ExternalLoginRepository(DbContext context)
            : base(context)
        {
        }

        public ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey)
        {
            return GetSingle(el => el.LoginProvider == loginProvider && el.ProviderKey == providerKey);
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey)
        {
            return GetSingleAsync(el => el.LoginProvider == loginProvider && el.ProviderKey == providerKey);
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider, string providerKey)
        {
            return GetSingleAsync(cancellationToken, el => el.LoginProvider == loginProvider && el.ProviderKey == providerKey);
        }
    }
}
