using IdentityDDD.Data.EntityFramework.Repositories;
using IdentityDDD.Domain;
using IdentityDDD.Domain.Repositories;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDDD.Data.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private IdentityContext context;
        private IExternalLoginRepository externalLoginRepository;
        private IRoleRepository roleRepository;
        private IUserRepository userRepository;
        #endregion

        //public T GetRepository<T>() where T : class
        //{
        //    using (var kernel = new StandardKernel())
        //    {
        //        kernel.Load(Assembly.GetExecutingAssembly());
        //        var result = kernel.Get<T>(new ConstructorArgument("context", context));

        //        if (result != null)
        //        {
        //            return result;
        //        }
        //    }

        //    return null;

        //}

        public int Commit()
        {
            return context.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return context.SaveChangesAsync();
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return context.SaveChangesAsync(cancellationToken);
        }

        public void Rollback()
        {
            context
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }

        public IExternalLoginRepository ExternalLoginRepository
        {
            get { return externalLoginRepository ?? (externalLoginRepository = new ExternalLoginRepository(context)); }
        }

        public IRoleRepository RoleRepository
        {
            get { return roleRepository ?? (roleRepository = new RoleRepository(context)); }
        }

        public IUserRepository UserRepository
        {
            get { return userRepository ?? (userRepository = new UserRepository(context)); }
        }
    }
}
