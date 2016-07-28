using System;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.ProductCatalog.ViewModels
{
    public class HomePageViewModel : ViewModel<Guid>
    {
        public HomePageProductViewModel[] Products { get; set; }
    }
}