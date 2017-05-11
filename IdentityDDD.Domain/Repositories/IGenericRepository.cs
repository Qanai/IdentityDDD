using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDDD.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        Task<IList<T>> GetAllAsync(params Expression<Func<T, object>>[] navigationProperties);
        Task<IList<T>> GetAllAsync(CancellationToken cancellationToken, params Expression<Func<T, object>>[] navigationProperties);

        IList<T> PageAll(int skip, int take, params Expression<Func<T, object>>[] navigationProperties);
        Task<IList<T>> PageAllAsync(int skip, int take, params Expression<Func<T, object>>[] navigationProperties);
        Task<IList<T>> PageAllAsync(CancellationToken cancellationToken, int skip, int take, params Expression<Func<T, object>>[] navigationProperties);

        IList<T> GetList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        Task<IList<T>> GetListAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        Task<IList<T>> GetListAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);

        T GetSingle(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        Task<T> GetSingleAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);

        void Add(params T[] items);
        void Update(params T[] items);
        void Remove(params T[] items);
        void UpdateRelated(Expression<Func<T, bool>> where, IEnumerable<object> updatedSet, string relatedPropertyName, string relatedPropertyKeyName);
    }
}
