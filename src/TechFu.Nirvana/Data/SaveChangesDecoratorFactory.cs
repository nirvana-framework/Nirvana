using System;

namespace TechFu.Nirvana.Data
{
    public class SaveChangesDecoratorFactory
    {
        public ISaveChangesDecorator[] Build(SaveChangesDecoratorType type)
        {
            switch (type)
            {
                case SaveChangesDecoratorType.Live:
                    return new ISaveChangesDecorator[]
                    {
                        new ModifiedCreatedDecorator()
                    };
                case SaveChangesDecoratorType.IntegrationTest:
                case SaveChangesDecoratorType.Empty:
                    return new ISaveChangesDecorator[0];
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}