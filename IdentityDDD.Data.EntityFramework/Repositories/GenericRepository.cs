using IdentityDDD.Domain.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDDD.Data.EntityFramework.Repositories
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DbContext context;

        internal GenericRepository(DbContext ctx)
        {
            context = ctx;
        }

        public IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list = null;

            list = GetQuery(navigationProperties)
                .ToList<T>();

            return list;
        }

        public async Task<IList<T>> GetAllAsync(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list = null;

            list = await GetQuery(navigationProperties)
                .ToListAsync<T>();

            return list;
        }

        public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list = null;

            list = await GetQuery(navigationProperties)
                .ToListAsync<T>(cancellationToken);

            return list;
        }

        public IList<T> PageAll(int skip, int take, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list = null;

            list = GetQuery(navigationProperties)
                .Skip(skip).Take(take)
                .ToList<T>();

            return list;
        }

        public async Task<IList<T>> PageAllAsync(int skip, int take, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list = null;

            list = await GetQuery(navigationProperties)
                .Skip(skip).Take(take)
                .ToListAsync<T>();

            return list;
        }

        public async Task<IList<T>> PageAllAsync(CancellationToken cancellationToken, int skip, int take, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list = null;

            list = await GetQuery(navigationProperties)
                .Skip(skip).Take(take)
                .ToListAsync<T>(cancellationToken);

            return list;
        }

        public IList<T> GetList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list = null;

            list = GetQuery(navigationProperties)
                .Where(where)
                .ToList<T>();

            return list;
        }

        public async Task<IList<T>> GetListAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list = null;

            list = await GetQuery(navigationProperties)
                .Where(where)
                .ToListAsync<T>();

            return list;
        }

        public async Task<IList<T>> GetListAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list = null;

            list = await GetQuery(navigationProperties)
                .Where(where)
                .ToListAsync<T>(cancellationToken);

            return list;
        }

        public T GetSingle(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;

            item = GetQuery(navigationProperties)
                .FirstOrDefault(where);

            return item;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;

            item = await GetQuery(navigationProperties)
                .FirstOrDefaultAsync(where);

            return item;
        }

        public async Task<T> GetSingleAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;

            item = await GetQuery(navigationProperties)
                .FirstOrDefaultAsync(where, cancellationToken);

            return item;
        }

        public void Add(params T[] items)
        {
            foreach (T item in items)
            {
                context.Entry(item).State = EntityState.Added;
            }
        }

        public void Update(params T[] items)
        {
            foreach (var item in items)
            {
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public void Remove(params T[] items)
        {
            foreach (var item in items)
            {
                context.Entry(item).State = EntityState.Deleted;
            }
        }

        public void UpdateRelated(Expression<Func<T, bool>> where, IEnumerable<object> updatedSet, string relatedPropertyName, string relatedPropertyKeyName)
        {
            context.Database.Log = message => Trace.Write(message);

            // Get the generic type of the set
            var type = updatedSet.First().GetType();
            var keyType = type.GetProperty(relatedPropertyKeyName).PropertyType;

            var items = context.Set<T>()
                .Include(relatedPropertyName)
                .ToList()
                .Where(where.Compile());

            foreach (var item in items)
            {
                var values = CreateList(type);

                //var qry = updatedSet
                //        .Select(obj => (int)(obj
                //        .GetType()
                //        .GetProperty(relatedPropertyKeyName)
                //        .GetValue(obj, null)));

                var qry = updatedSet
                .Select(obj =>
                    Convert.ChangeType
                    (
                        obj.GetType()                                          
                        .GetProperty(relatedPropertyKeyName)                                          
                        .GetValue(obj, null), keyType
                    )
                );

                var relatedEntries = qry
                    .Select(val => context.Set(type).Find(val));

                foreach (var entry in relatedEntries)
                {
                    //await context.Entry(entry).ReloadAsync();
                    values.Add(entry);
                }

                context.Entry(item).Collection(relatedPropertyName).CurrentValue = values;
            }
        }

        private IList CreateList(Type type)
        {
            var genericList = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericList);
        }

        private IQueryable<T> GetQuery(System.Linq.Expressions.Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = ApplyEagerLoading(navigationProperties);

            var result = dbQuery
                .AsNoTracking();

            return result;
        }

        private IQueryable<T> ApplyEagerLoading(System.Linq.Expressions.Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = context.Set<T>();

            foreach (var navigationProperty in navigationProperties)
            {
                dbQuery = dbQuery.Include<T, object>(navigationProperty);
            }

            return dbQuery;
        }
    }
}
