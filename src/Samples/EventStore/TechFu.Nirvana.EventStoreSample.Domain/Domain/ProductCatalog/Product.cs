using System;
using TechFu.Nirvana.Data.EntityTypes;

namespace TechFu.Nirvana.EventStoreSample.Domain.Domain.ProductCatalog
{
    public class Product : Entity<Guid>
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal BasePrice { get; set; }
    }
}