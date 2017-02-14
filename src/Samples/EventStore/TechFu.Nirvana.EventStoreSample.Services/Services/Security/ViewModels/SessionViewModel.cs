using System;
using Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.ViewModels
{
    public class SessionViewModel:ViewModel<Guid>
    {
        public string Name { get; set; }
    }
}