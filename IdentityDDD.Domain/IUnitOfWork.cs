using IdentityDDD.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDDD.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        #region Properties
        IExternalLoginRepository ExternalLoginRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        #endregion

        //T GetRepository<T>() where T : class;
        int Commit();
        Task<int> CommitAsync();
        Task<int> CommitAsync(CancellationToken cancellationToken);
        void Rollback();
    }
}
