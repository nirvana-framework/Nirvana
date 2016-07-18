using System;

namespace TechFu.Nirvana.CQRS
{
    public class UiEvent<T> : NirvanaTask
    {
        public Guid GroupId { get; set; }
    }
}