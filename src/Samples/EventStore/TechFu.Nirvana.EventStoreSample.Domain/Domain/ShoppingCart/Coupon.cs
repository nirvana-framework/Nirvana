using System;
using System.Collections.Generic;
using TechFu.Nirvana.Data.EntityTypes;

namespace TechFu.Nirvana.EventStoreSample.Domain.Domain.ShoppingCart
{
    public class Coupon : Entity<Guid>
    {
        public string Name { get; set; }
        public CouponTypeValue CouponTypeValue { get; set; }
        public decimal Amount { get; set; }
        public Guid ProductId { get; set; }
        public List<Guid> ChildProductsAffected { get; set; }
    }
}