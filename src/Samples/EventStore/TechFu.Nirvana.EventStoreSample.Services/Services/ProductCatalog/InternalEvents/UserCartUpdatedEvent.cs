﻿using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.ProductCatalog.InternalEvents
{
    [ProductCatalogRoot("UserCartUpdatedEvent")]
    public class UserCartUpdatedEvent : InternalEvent
    {
        public Guid UserId { get; set; }
    }
}