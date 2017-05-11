using IdentityDDD.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityDDD.Data.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private IdentityContext context;
        #endregion

        public T GetRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public int Commit()
        {
            throw new NotImplementedException();
        }

        public Task<int> CommitAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CommitAsync(System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
