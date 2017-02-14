using System;
using Nirvana.CQRS;
using Nirvana.Data;
using Nirvana.Domain;
using Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Common
{
    public abstract class ViewModelQueryBase<T, U> : QueryHandlerBase<T, U>
        where U : ViewModel<Guid>
        where T : Query<U>
    {
        private readonly IViewModelRepository _repository;

        protected ViewModelQueryBase(IViewModelRepository repository, IChildMediatorFactory mediator):base(mediator)
        {
            _repository = repository;
        }

        public QueryResponse<U> GetFromViewModelRepository(T query, Func<T, Guid> match)
        {
            var model = _repository.Get<U>(match.Invoke(query));
            if (model == null)
            {
                return QueryResponse.Fail<U>("Could not Find Model");
            }

            return QueryResponse.Success(model);
        }
    }
}