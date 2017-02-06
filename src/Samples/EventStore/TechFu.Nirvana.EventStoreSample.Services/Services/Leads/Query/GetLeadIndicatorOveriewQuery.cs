using System;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Domain;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Leads.Enumerations;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Leads.Query
{
    [LeadRoot(typeof(GetLeadIndicatorOveriewQuery))]
    public class GetLeadIndicatorOveriewQuery :Query<LeadIndicatorViewModel>
    {
        public Guid LeadId { get; set; }
    }
    public class LeadIndicatorViewModel : ViewModel<Guid>
    {
        public IndicatorType[] AllIndicators { get; set; }
        public IndicatorSource[] DataSources { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public PerformanceIndicatorViewModel[] Indicators { get; set; }
        public BusinesssMeasureViewModel BusinessMeasure{ get; set; }
    }

    public class BusinesssMeasureViewModel
    {
        public double AnnualRevenue { get; set; }
        public int NumberOfEmployees { get; set; }
        public EntityTypeValue EntytyType { get; set; }
    }

    public class PerformanceIndicatorViewModel
    {
        public IndicatorType IndicatorType { get; set; }
        public IndicatorValueViewModel SelectedValue { get; set; }
        public IndicatorValueViewModel[] AllValues { get; set; }
    }

    public class IndicatorValueViewModel
    {
        public Guid LeadId { get; set; }
        public object Value { get; set; }
        public IndicatorType Type { get; set; }
        public IndicatorSource Source { get; set; }
    }
}