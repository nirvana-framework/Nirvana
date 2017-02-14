using System;

namespace Nirvana.CQRS
{
    public abstract class Query<T> : NirvanaTask
    {
    }

    public abstract class AuthorizedQuery<T> : Query<T>, IAuthorizedTask
    {
        public string AuthCode { get; set; }
        public Guid AuthorizedUserId { get; set; }
    }
}