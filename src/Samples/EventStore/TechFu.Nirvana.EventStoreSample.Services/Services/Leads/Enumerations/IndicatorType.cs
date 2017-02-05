using System;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Leads.Enumerations
{
    public enum IndicatorTypeValue
    {
        EmplyeeCount = 1,
        AnnualRevenue = 2,
        EntityType = 3,
        Address = 4
    }


    public class IndicatorType : Enumeration<IndicatorType>
    {
        public static IndicatorType Address = new IndicatorType(IndicatorTypeValue.Address, "Address", typeof(string[]));

        public static IndicatorType AnnualRevenue = new IndicatorType(IndicatorTypeValue.AnnualRevenue, "AnnualRevenue",
            typeof(decimal));

        public static IndicatorType EmployeeCount = new IndicatorType(IndicatorTypeValue.EmplyeeCount, "Employee Count",
            typeof(int));

        public static IndicatorType EntityType = new IndicatorType(IndicatorTypeValue.EntityType, "Entity Type",
            typeof(EntityType));

        public Type ValueType { get; set; }

        public static IndicatorType FromValue(IndicatorTypeValue value)
        {
            return FromInt32((int) value);
        }

        public IndicatorType(IndicatorTypeValue value, string displayName, Type valueType)
            : base((int) value, displayName)
        {
            ValueType = valueType;
        }
    }
}