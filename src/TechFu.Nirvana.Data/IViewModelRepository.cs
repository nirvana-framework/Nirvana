using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data.EntityTypes;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.Data
{
    public interface IViewModelRepository : IDisposable
    {
        T Get<T>(Guid id) where T : ViewModel<Guid>;
        void Save<T>(T input) where T : ViewModel<Guid>;
     
     
        
        void BeginTransaction(IsolationLevel? isolationLevel = null);
        void CommitTransaction();
        void RollbackTransaction();
    }
}