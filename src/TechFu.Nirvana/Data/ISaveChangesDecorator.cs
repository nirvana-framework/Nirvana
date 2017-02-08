
namespace TechFu.Nirvana.Data
{
    public interface ISaveChangesDecorator
    {
        int Decorate(SaveChangesContext context);
    }
}