﻿using System;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Command
{
    [SecurityRoot("CreateNewSessionViewModelCommand")]
    public class CreateNewSessionViewModelCommand : NopCommand
    {
        public Guid SessionId { get; set; }
    }
}