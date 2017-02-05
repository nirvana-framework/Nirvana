using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.SqlProvider.Decorators
{
    public interface ISaveChangesDecorator
    {
        int Decorate<T>(SaveChangesContext<T> context) where T: AggregateRootAttribute;
    }
}