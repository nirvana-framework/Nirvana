using System;
using System.Collections.Generic;
using TechFu.Nirvana.Data.EntityTypes;

namespace TechFu.Nirvana.EventStoreSample.Domain.Domain.ShoppingCart
{
    public class Cart : Entity<Guid>
    {
        public List<CartItem> Items { get; set; }
        public List<Coupon> Coupons { get; set; }
    }
}