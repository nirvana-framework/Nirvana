using System;
using System.Linq;
using Nirvana.CQRS;
using Nirvana.Mediation;
using Nirvana.Util.Io;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Leads.Enumerations;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Leads.Query;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.LeadPrototype
{
    public class GetLeadIndicatorOveriewHandler :
        QueryHandlerBase<GetLeadIndicatorOveriewQuery, LeadIndicatorViewModel>
    {
        private readonly ISerializer _serializer;

        public GetLeadIndicatorOveriewHandler(ISerializer serializer, IChildMediatorFactory mediator):base(mediator)
        {
            _serializer = serializer;
        }

        public override QueryResponse<LeadIndicatorViewModel> Handle(GetLeadIndicatorOveriewQuery query)
        {
            var leadIndicatorViewModel = new LeadIndicatorViewModel
            {
                AllIndicators = GetAllIndicators(),
                DataSources = GetAllDataSources(),
                BusinessMeasure = getBusinessMeasure(),
                Id = query.LeadId,
                Indicators = Indicators(query.LeadId),
                Name = "Test Lead",
                RootEntityKey = query.LeadId,
                Address = "somewhere...over the rainbow="
            };

            return QueryResponse.Success(leadIndicatorViewModel);
        }

        private BusinesssMeasureViewModel getBusinessMeasure()
        {
            return new BusinesssMeasureViewModel
            {
                AnnualRevenue = 123.23,
                EntytyType = EntityTypeValue.LLC,
                NumberOfEmployees = 23
            };
        }

        private IndicatorType[] GetAllIndicators()
        {
            return IndicatorType.GetAll().OrderBy(x => x.DisplayName).ToArray();
        }

        private IndicatorSource[] GetAllDataSources()
        {
            return IndicatorSource.GetAll().OrderBy(x => x.DisplayName).ToArray();
        }

        private PerformanceIndicatorViewModel[] Indicators(Guid leadId)
        {
            return new[]
            {
                new PerformanceIndicatorViewModel
                {
                    SelectedValue =
                        BuildIndicatorTypeValue(leadId, "200000.00", IndicatorType.AnnualRevenue,IndicatorSource.ReportSourceA),
                    IndicatorType = IndicatorType.AnnualRevenue,
                    AllValues = new[]
                    {
                        BuildIndicatorTypeValue(leadId, "500000.00", IndicatorType.AnnualRevenue,IndicatorSource.SelfReported),
                        BuildIndicatorTypeValue(leadId, "200000.00", IndicatorType.AnnualRevenue,IndicatorSource.ReportSourceA),
                        BuildIndicatorTypeValue(leadId, "100000.00", IndicatorType.AnnualRevenue,IndicatorSource.ReportSourceB),
                        BuildIndicatorTypeValue(leadId, "450000.00", IndicatorType.AnnualRevenue,IndicatorSource.ReportSourceC),
                        BuildIndicatorTypeValue(leadId, "345000.00", IndicatorType.AnnualRevenue,IndicatorSource.ReportSourceD)
                    }
                },
                new PerformanceIndicatorViewModel
                {
                    SelectedValue =
                        BuildIndicatorTypeValue(leadId, "[\"123 somewhere St. Suite 200\"]", IndicatorType.Address,IndicatorSource.SelfReported),
                    IndicatorType = IndicatorType.Address,
                    AllValues = new[]
                    {
                        BuildIndicatorTypeValue(leadId, "[\"123 somewhere St. Suite 200\"]", IndicatorType.Address,IndicatorSource.SelfReported),
                        BuildIndicatorTypeValue(leadId, "[\"512 Boone Valley\"]", IndicatorType.Address,IndicatorSource.ReportSourceB),
                        BuildIndicatorTypeValue(leadId, "[\"13809 Research Blvd\"]", IndicatorType.Address,IndicatorSource.ClientManagementOverride)
                    }
                },
                new PerformanceIndicatorViewModel
                {
                    SelectedValue =
                        BuildIndicatorTypeValue(leadId, "24", IndicatorType.EmployeeCount, IndicatorSource.ReportSourceB),IndicatorType = IndicatorType.EmployeeCount,
                    AllValues = new[]
                    {
                        BuildIndicatorTypeValue(leadId, "50", IndicatorType.EmployeeCount, IndicatorSource.SelfReported),
                        BuildIndicatorTypeValue(leadId, "25", IndicatorType.EmployeeCount, IndicatorSource.ReportSourceA),
                        BuildIndicatorTypeValue(leadId, "27", IndicatorType.EmployeeCount, IndicatorSource.ReportSourceC),
                        BuildIndicatorTypeValue(leadId, "24", IndicatorType.EmployeeCount, IndicatorSource.ReportSourceB)
//                   BuildIndicatorTypeValue(leadId,"24",IndicatorType.EntityType,InficatorSource.ReportSourceB), 
                    }
                }
            };
        }


        private IndicatorValueViewModel BuildIndicatorTypeValue(Guid leadId, string serializedValue, IndicatorType type,
            IndicatorSource dataSource)
        {
            var value = _serializer.Deserialize(type.ValueType, serializedValue);

            return new IndicatorValueViewModel {LeadId = leadId, Value = value, Type = type, Source = dataSource};
        }
    }
}