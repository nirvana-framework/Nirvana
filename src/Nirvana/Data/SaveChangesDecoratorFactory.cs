using System;

namespace Nirvana.Data
{
    public class SaveChangesDecoratorFactory
    {
        public ISaveChangesDecorator[] Build(ActiveDecoratorType type)
        {
            switch (type)
            {
                case ActiveDecoratorType.Live:
                    return new ISaveChangesDecorator[]
                    {
                        new ModifiedCreatedDecorator()
                    };
                case ActiveDecoratorType.IntegrationTest:
                case ActiveDecoratorType.Empty:
                    return new ISaveChangesDecorator[0];
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}