using System;
using System.Collections.Generic;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.ProductCatalog;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.InternalEvents;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Command
{
    public class CreateSampleCatalogHandler : INopHandler<CreateSampleCatalogCommand>
    {
        private readonly IRepository _repository;
        private readonly IMediatorFactory _mediator;

        public CreateSampleCatalogHandler(IRepository repository, IMediatorFactory mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public CommandResponse<Nop> Handle(CreateSampleCatalogCommand command)
        {

            var oldCatalog = _repository.GetAll<Product>();
            _repository.DeleteCollection(oldCatalog);

            _repository.SaveOrUpdateCollection(SampleCatalog());
            _mediator.InternalEvent(new CatalogUpdatedEvent());
            return CommandResponse.Succeeded(Nop.NoValue);
        }

        private static List<Product> SampleCatalog()
        {
            return new List<Product>
            {
                new Product
                {
                    Name = "Test 1",
                    BasePrice = 15.25m,
                    Id = Guid.NewGuid(),
                    LongDescription = "Test test",
                    ShortDescription = "Test"
                },
                new Product
                {
                    Name = "Test 2",
                    BasePrice = 25.25m,
                    Id = Guid.NewGuid(),
                    LongDescription = "Test test",
                    ShortDescription = "Test"
                },
                new Product
                {
                    Name = "Test 3",
                    BasePrice = 35.25m,
                    Id = Guid.NewGuid(),
                    LongDescription = "Test test",
                    ShortDescription = "Test"
                },
            };
        }
    }
}