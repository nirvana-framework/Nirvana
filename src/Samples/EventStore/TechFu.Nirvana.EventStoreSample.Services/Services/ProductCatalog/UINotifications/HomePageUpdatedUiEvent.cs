using System;
using Nirvana.Configuration;
using Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.UINotifications
{
    [ProductCatalogRoot(typeof(HomePageUpdatedUiEvent))]
    public class HomePageUpdatedUiEvent : UiEvent<HomePageUpdatedUiEvent>
    {
        public override Guid AggregateRoot => NirvanaSetup.ApplicationLevelViewModelKey;
    }
}