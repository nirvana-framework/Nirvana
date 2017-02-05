using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Leads.Enumerations
{
    public enum SerializerTypeValue
    {
        String,
        StringArray,
        List,
        EnumType,
        Int,
        Decimal,
        Double,
        Money
    }

    public class SerializerType : Enumeration<SerializerType>
    {
        public static  SerializerType String = new SerializerType(SerializerTypeValue.String, "String");

        public SerializerType(SerializerTypeValue value, string displayName) : base((int)value, displayName)
        {
        }

        public static SerializerType FromValue(SerializerTypeValue input)
        {
            return FromInt32((int) input);
        }
    }
}