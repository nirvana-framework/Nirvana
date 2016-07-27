using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data.EntityTypes;

namespace TechFu.Nirvana.Data
{
    public interface IViewModelRepository : IDisposable
    {
        T Get<T>(Guid id) where T : ViewModel<Guid>;
     
        IQueryable<T> GetAll<T>() where T : Entity;
        IQueryable<T> GetAllAndInclude<T, TProperty>(Expression<Func<T, TProperty>> path) where T : Entity;

        PagedResult<T> GetPaged<T>(PaginationQuery pageInfo,
            IList<Expression<Func<T, bool>>> conditions,
            IList<Expression<Func<T, object>>> orders = null,
            IList<Expression<Func<T, object>>> includes = null
        ) where T : Entity;

        void EnableSoftDeleteLoading();
        void BeginTransaction(IsolationLevel? isolationLevel = null);
        void CommitTransaction();
        void RollbackTransaction();
    }
}