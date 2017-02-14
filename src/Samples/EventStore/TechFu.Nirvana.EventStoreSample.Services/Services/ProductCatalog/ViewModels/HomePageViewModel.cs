using System;
using Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.ViewModels
{
    public class HomePageViewModel : ViewModel<Guid>
    {
        public HomePageProductViewModel[] Products { get; set; }
    }
}