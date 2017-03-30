using System;
using Nirvana.Domain;

namespace Nirvana.Data
{
    public interface IViewModelRepository : IDisposable
    {
        T Get<T>(Guid id) where T : ViewModel<Guid>;
        void Save<T>(T input) where T : ViewModel<Guid>;


        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}