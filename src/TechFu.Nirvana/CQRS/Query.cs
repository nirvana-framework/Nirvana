namespace TechFu.Nirvana.CQRS
{
    public abstract class Query<T> : NirvanaTask
    {
    }

    public abstract class AuthorizedQuery<T> : Query<T>
    {
        public string AuthCode { get; set; }
    }
}