using System;
using System.Collections.Generic;
using Nirvana.Data.EntityTypes;

namespace TechFu.Nirvana.EventStoreSample.Domain.Domain.ShoppingCart
{
    public class Cart : Entity<Guid>
    {
        public Cart()
        {
            Items = new List<CartItem>();
            Coupons= new List<Coupon>();
        }

        public Guid UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public List<Coupon> Coupons { get; set; }
    }
}