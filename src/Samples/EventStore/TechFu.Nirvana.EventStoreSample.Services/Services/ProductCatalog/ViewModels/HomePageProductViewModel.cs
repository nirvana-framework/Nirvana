using System;
using Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.ViewModels
{
    public class HomePageProductViewModel : ViewModel<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
    }
}