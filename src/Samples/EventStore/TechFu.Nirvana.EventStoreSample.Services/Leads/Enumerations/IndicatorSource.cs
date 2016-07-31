using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Leads.Enumerations
{
    public enum InficatorSourceValue
    {
        SelfReported = 1,
        ReportSourceA = 2,
        ReportSourceB = 3,
        ReportSourceC = 4,
        ReportSourceD = 5,
        UnderwriterOverride = 6,
        ClientManagementOverride = 7
    }

    public class IndicatorSource : Enumeration<IndicatorSource>
    {
        public static IndicatorSource SelfReported = new IndicatorSource(InficatorSourceValue.SelfReported,"Self Reported");

        public static IndicatorSource ReportSourceA = new IndicatorSource(InficatorSourceValue.ReportSourceA,"Report Source A");

        public static IndicatorSource ReportSourceB = new IndicatorSource(InficatorSourceValue.ReportSourceB,"Report Source B");

        public static IndicatorSource ReportSourceC = new IndicatorSource(InficatorSourceValue.ReportSourceC,"Report Source C");

        public static IndicatorSource ReportSourceD = new IndicatorSource(InficatorSourceValue.ReportSourceD,"Report Source D");

        public static IndicatorSource UnderwriterOverride = new IndicatorSource(InficatorSourceValue.UnderwriterOverride, "UnderwriterOverride");

        public static IndicatorSource ClientManagementOverride =new IndicatorSource(InficatorSourceValue.ClientManagementOverride, "ClientManagementOverride");

        public IndicatorSource(InficatorSourceValue value, string displayName) : base((int) value, displayName)
        {
        }
        


        public static IndicatorSource FromValue(InficatorSourceValue value)
        {
            return FromInt32((int) value);
        }
    }
}