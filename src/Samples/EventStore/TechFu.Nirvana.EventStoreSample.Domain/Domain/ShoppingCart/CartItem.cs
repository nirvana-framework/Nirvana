using System;
using Nirvana.Data.EntityTypes;

namespace TechFu.Nirvana.EventStoreSample.Domain.Domain.ShoppingCart
{
    public class CartItem : Entity<Guid>
    {
        public Cart Cart { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public string Name { get; set; }
    }
}