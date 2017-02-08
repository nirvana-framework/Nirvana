using System;
using System.Data;
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