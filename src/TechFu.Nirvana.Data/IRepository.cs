using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TechFu.Nirvana.Data.EntityTypes;

namespace TechFu.Nirvana.Data
{
    public interface ISqlRepository
    {
        IEnumerable<T> Sql<T>(string sql, params object[] parameters);
    }

    public interface IRepository : IReadOnlyRepository
    {
        void SaveOrUpdate<T>(T entity) where T : Entity;
        void SaveOrUpdateCollection<T>(ICollection<T> entities) where T : Entity;
        void Delete<T>(T entity) where T : Entity;
        void DeleteCollection<T>(ICollection<T> entities) where T : Entity;

        T GetAndInclude<T, TProperty>(Guid id, Expression<Func<T, TProperty>> path)
            where T : Entity<Guid>
            where TProperty : Entity;

        int SqlCommand(string sql, params object[] parameters);

        void ClearContext();
        T Refresh<T>(T entity) where T : Entity;
        ICollection<T> Refresh<T>(ICollection<T> entities) where T : Entity;
    }
}