using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.Data.EntityTypes;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.MongoProvider
{
    public  class MonogoViewModelRepository: IViewModelRepository
    {
        public void Dispose()
        {
            
        }

        public T Get<T>(Guid id) where T : ViewModel<Guid>
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll<T>() where T : Entity
        {
            return new List<T>().AsQueryable();
        }

        public IQueryable<T> GetAllAndInclude<T, TProperty>(Expression<Func<T, TProperty>> path) where T : Entity
        {
            return new List<T>().AsQueryable();
        }

        public PagedResult<T> GetPaged<T>(PaginationQuery pageInfo, IList<Expression<Func<T, bool>>> conditions, IList<Expression<Func<T, object>>> orders = null, IList<Expression<Func<T, object>>> includes = null) where T : Entity
        {
            return new PagedResult<T>();
        }
        

        public void BeginTransaction(IsolationLevel? isolationLevel = null)
        {
        }

        public void CommitTransaction()
        {
        }

        public void RollbackTransaction()
        {
        }
    }
}