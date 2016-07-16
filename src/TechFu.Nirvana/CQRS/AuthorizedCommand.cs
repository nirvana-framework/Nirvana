using System;

namespace TechFu.Nirvana.CQRS
{
    public abstract class AuthorizedCommand<T> : Command<T>, IAuthorizedTask
    {
        public string AuthCode { get; set; }
        public Guid AuthorizedUserId { get; set; }
    }
}