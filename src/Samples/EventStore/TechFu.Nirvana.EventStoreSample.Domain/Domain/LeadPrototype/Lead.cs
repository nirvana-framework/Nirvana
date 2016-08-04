using System;
using TechFu.Nirvana.Data.EntityTypes;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Leads.Enumerations;

namespace TechFu.Nirvana.EventStoreSample.Domain.Domain.LeadPrototype
{
    public class Lead : SoftDeletedEntity<Guid>
    {
        public BusinessMeasure Measure { get; set; }
        public string Address { get; set; }
    }

    public class BusinessMeasure : SoftDeletedEntity<Guid>
    {
        public Lead Lead { get; set; }
        public double AnnualRevenue { get; set; }
        public double NumberOfEmployees { get; set; }
        public EntityTypeValue EntytyType { get; set; }
    }

    public class IndicatorValue : SoftDeletedEntity<Guid>
    {
        public string SerializedValue;
        public IndicatorTypeValue IndicatorType { get; set; }
        public SerializerTypeValue SerializerType{ get; set; }
        public PerformanceIndicator Indicator { get; set; }
    }

    public class PerformanceIndicator : SoftDeletedEntity<Guid>
    {
        public Lead Lead { get; set; }
        public IndicatorTypeValue IndicatorType { get; set; }
        public IndicatorSourceValue SelectedSourceType { get; set; }
        public IndicatorValue[] AllValues { get; set; }
    }
}