namespace TechFu.Nirvana.CQRS
{
    public abstract class Command<T>:NirvanaTask
    {

    }

    public abstract class AuthorizedCommand<T>:Command<T>
    {
        public string AuthCode { get; set; }
    }

    public abstract class NopCommand : Command<Nop>
    {
    }

    public enum Nop
    {
        NoValue
    }
    public enum MessageType { Info=1, Warning=2, Error=3, Exception=4 }
}