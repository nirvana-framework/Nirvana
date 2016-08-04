using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntityFramework.DynamicFilters;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.Data.EntityTypes;
using TechFu.Nirvana.Util.Extensions;
using TechFu.Nirvana.Util.Tine;

namespace TechFu.Nirvana.SqlProvider.Domain
{
    public class SqlRepository : IRepository
    {
        public static Action<DbContext> Configure = a => { };

        private readonly ConcurrentDictionary<Type, object> _dbSets = new ConcurrentDictionary<Type, object>();

        private DbContext _context;

        public SqlRepository(DbContext context)
        {
            _context = context;
        }

        public T Get<T>(Guid id) where T : Entity<Guid>
        {
            return GetDbSet<T>().Where(e => e.Id == id).ToList().SingleOrDefault();
        }

        public T GetAndInclude<T, TProperty>(Guid id, Expression<Func<T, TProperty>> path) where T : Entity<Guid>
            where TProperty : Entity
        {
            var entity = GetDbSet<T>().Find(id);

            _context.Entry(entity).Reference(path).Load();

            return entity;
        }

        public T Get<T>(long id) where T : Entity<long>
        {
            return GetDbSet<T>().Find(id);
        }

        public IQueryable<T> GetAll<T>() where T : Entity
        {
            return GetDbSet<T>();
        }

        public IQueryable<T> GetAllAndInclude<T>(IList<Expression<Func<T, object>>> includes) where T : Entity
        {
            return Include(GetAll<T>(), includes);
        }

        public IQueryable<T> Include<T>(IQueryable<T> queryable, IList<Expression<Func<T, object>>> includes) where T : Entity
        {
            queryable = includes.Aggregate(queryable, (current, expression) => current.Include(expression));
            return queryable;
        }


        public PagedResult<T> GetPaged<T>(PaginationQuery pageInfo,
            IList<Expression<Func<T, bool>>> conditions
            , IList<Expression<Func<T, object>>> orders = null,
            IList<Expression<Func<T, object>>> includes = null
        ) where T : Entity
        {
            var queryable = GetAll<T>();


            if (includes != null)
            {
                queryable = includes.Aggregate(queryable, (current, expression) => current.Include(expression));
            }

            if (conditions != null)
            {
                queryable = conditions.Aggregate(queryable, (current, expression) => current.Where(expression));
            }

            var total = queryable.Count();

            var totalPages = total/pageInfo.ItemsPerPage;
            if (total%pageInfo.ItemsPerPage != 0)
            {
                totalPages += 1;
            }

            if (orders == null || orders.Count == 0)
            {
                queryable = queryable.OrderByDescending(x => x.Created);
            }
            else
            {
                queryable = orders.Aggregate(queryable.OrderBy(x => 1), (current, order) => current.ThenBy(order));
            }


            var firstRecord = (pageInfo.PageNumber - 1)*pageInfo.ItemsPerPage;

            T[] pagedData;
            if (firstRecord != 0)
            {
                pagedData = queryable
                    .Skip(firstRecord)
                    .Take(pageInfo.ItemsPerPage)
                    .ToArray();
            }
            else
            {
                pagedData = queryable
                    .Take(pageInfo.ItemsPerPage)
                    .ToArray();
            }


            return new PagedResult<T>
            {
                Results = pagedData,
                PerPage = pageInfo.ItemsPerPage,
                Page = pageInfo.PageNumber,
                Total = total,
                LastPage = totalPages
            };
        }


        public IQueryable<T> GetAllAndInclude<T, TProperty>(Expression<Func<T, TProperty>> path) where T : Entity
        {
            return GetDbSet<T>().Include(path);
        }


        public void SaveOrUpdateCollection<T>(IEnumerable<T> entities) where T : Entity
        {
            if (!entities.Any())
                return;


            var autoDetectChanges = _context.Configuration.AutoDetectChangesEnabled;

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                foreach (var entity in entities)
                {
                    AddOrUpdate(entity);
                }
            }
            finally
            {
                _context.Configuration.AutoDetectChangesEnabled = autoDetectChanges;
            }

            _context.SaveChanges();
        }


        public void SaveOrUpdate<T>(T entity) where T : Entity
        {
            AddOrUpdate(entity);
            _context.SaveChanges();
        }

        public void Delete<T>(T entity) where T : Entity
        {
            if (!(entity is ISoftDelete)) GetDbSet<T>().Remove(entity);
            else
            {
                var e = (ISoftDelete) entity;
                e.Deleted = new SystemTime().UtcNow();
                e.DeletedBy = "unknown";

                _context.Entry(entity).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }


        public void DeleteCollection<T>(IEnumerable<T> entities) where T : Entity
        {
            if (!entities.Any())
                return;

            if (!(entities.First() is ISoftDelete))
            {
                GetDbSet<T>().RemoveRange(entities);
            }
            else
            {
                foreach (var e in entities.Cast<ISoftDelete>())
                {
                    e.Deleted = new SystemTime().UtcNow();
                    e.DeletedBy = "unknown";
                }
            }
            _context.SaveChanges();
        }

        public int SqlCommand(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public void ClearContext()
        {
            var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted |
                                                                            EntityState.Modified | EntityState.Unchanged)
                .Where(x => x.State != EntityState.Detached)
                .Select(x => x.Entity)
                .Where(x => x != null)
                .OfType<Entity>()
                .Distinct()
                .ForEach(x => objectContext.Detach(x));
        }

        public T Refresh<T>(T entity) where T : Entity
        {
            if (entity != null)
            {
                ((IObjectContextAdapter) _context).ObjectContext.Refresh(RefreshMode.StoreWins, entity);
            }
            return entity;
        }

        public IEnumerable<T> Refresh<T>(IEnumerable<T> entities) where T : Entity
        {
            if (entities.Any())
            {
                ((IObjectContextAdapter) _context).ObjectContext.Refresh(RefreshMode.StoreWins, entities);
            }
            return entities;
        }

        public void RollbackTransaction()
        {
            throw new NotImplementedException();
        }

        public void EnableSoftDeleteLoading()
        {
            _context.DisableFilter("SoftDelete");
        }

        public void BeginTransaction(IsolationLevel? isolationLevel = null)
        {
            throw new NotImplementedException();
        }

        public void CommitTransaction()
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            if (_context == null)
                return;

            _context.Dispose();
            _context = null;
        }

        public Task SaveOrUpdateCollectionAsync<T>(ICollection<T> entities) where T : Entity
        {
            throw new NotImplementedException();
        }


        public IEnumerable<T> Sql<T>(string sql, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(sql, parameters);
        }

        public IDisposable EnableSqlLogging()
        {
            throw new NotImplementedException();
        }

        public IDisposable DisableSqlLogging()
        {
            throw new NotImplementedException();
        }


        private void AddOrUpdate<T>(T entity) where T : Entity
        {
            var entityState = _context.Entry(entity).State;
            if (entityState == EntityState.Detached || entityState == EntityState.Added)
            {
                GetDbSet<T>().Add(entity);
            }
        }

        public IList<T> Refresh<T>(IList<T> entities) where T : Entity
        {
            if (entities.Any())
            {
                ((IObjectContextAdapter) _context).ObjectContext.Refresh(RefreshMode.StoreWins, entities);
            }
            return entities;
        }


        public void DisableSoftDeleteLoading()
        {
            _context.EnableFilter("SoftDelete");
        }


        public void DisableAutoDetectChanges()
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
        }

        private DbSet<T> GetDbSet<T>() where T : Entity
        {
            return (DbSet<T>) _dbSets.GetOrAdd(typeof(T), x => _context.Set<T>());
        }
    }
}