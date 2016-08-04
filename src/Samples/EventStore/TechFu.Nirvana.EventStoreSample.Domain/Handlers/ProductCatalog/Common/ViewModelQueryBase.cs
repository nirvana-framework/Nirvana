using System;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.Domain;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Common
{
    public abstract class ViewModelQueryBase<T, U> : QueryHandlerBase<T, U>
        where U : ViewModel<Guid>
        where T : Query<U>
    {
        private readonly IViewModelRepository _repository;

        protected ViewModelQueryBase(IViewModelRepository repository, IMediatorFactory mediator):base(mediator)
        {
            _repository = repository;
        }

        public QueryResponse<U> GetFromViewModelRepository(T query, Func<T, Guid> match)
        {
            var model = _repository.Get<U>(match.Invoke(query));
            if (model == null)
            {
                return QueryResponse.Failed<U>("Could not Find Model");
            }

            return QueryResponse.Succeeded(model);
        }
    }
}