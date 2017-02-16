using System;

namespace Nirvana.CQRS
{
    public interface IAuthorizedTask
    {
        string AuthCode { get; set; }
        Guid AuthorizedUserId { get; set; }
    }
}