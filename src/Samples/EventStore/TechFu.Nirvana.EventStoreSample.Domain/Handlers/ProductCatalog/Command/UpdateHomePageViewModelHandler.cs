using System;
using System.Collections.Generic;
using System.Linq;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.Domain;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.ProductCatalog;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.Commands;
using TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.ViewModels;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Command
{
    public class UpdateHomePageViewModelHandler : INopHandler<UpdateHomePageViewModelCommand>
    {
        private readonly IRepository _repository;
        private readonly IViewModelRepository _viewModelRepository;

        public UpdateHomePageViewModelHandler(IViewModelRepository viewModelRepository, IRepository repository)
        {
            _viewModelRepository = viewModelRepository;
            _repository = repository;
        }

        public CommandResponse<Nop> Handle(UpdateHomePageViewModelCommand command)
        {
            var products = _repository.GetAll<Product>().ToArray();

            var homepageModel = new HomePageViewModelBuilder().SetInputs(new Dictionary<string, object>
            {
                {HomePageViewModelBuilder.Keys.Products, products}
            }).Build();
            _viewModelRepository.Save(homepageModel);

            return CommandResponse.Succeeded(Nop.NoValue);
        }
    }

    public class HomePageViewModelBuilder : ViewModelBuilder<HomePageViewModel>
    {
        public override HomePageViewModel Build()
        {
            return new HomePageViewModel
            {
                Id = NirvanaSetup.ApplicationLevelViewModelKey,
                Products = BuildProductViewModel(GetValue(Keys.Products, new Product[0]))
            };
        }

        private HomePageProductViewModel[] BuildProductViewModel(Product[] products)
        {
            return products.Select(x => new HomePageProductViewModel
            {
                Name = x.Name,
                Id = x.Id,
                ShortDescription = x.ShortDescription,
                Price = x.BasePrice,
                RootEntityKey = Guid.Empty
            }).ToArray();
        }

        public static class Keys
        {
            public static readonly string Products = "Products";
        }
    }
}