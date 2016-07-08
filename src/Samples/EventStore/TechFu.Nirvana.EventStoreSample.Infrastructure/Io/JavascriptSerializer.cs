namespace TechFu.Nirvana.EventStoreSample.Infrastructure.Io
{
    public class EnclosingTypeJsonConverter<T> : EnclosingTypeJsonConverter
    {
        public EnclosingTypeJsonConverter() : base(typeof(T))
        {
        }
    }
}