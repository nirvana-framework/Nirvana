using Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Leads.Enumerations
{
    public enum IndicatorSourceValue
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
        public static IndicatorSource SelfReported = new IndicatorSource(IndicatorSourceValue.SelfReported,"Self Reported");

        public static IndicatorSource ReportSourceA = new IndicatorSource(IndicatorSourceValue.ReportSourceA,"Report Source A");

        public static IndicatorSource ReportSourceB = new IndicatorSource(IndicatorSourceValue.ReportSourceB,"Report Source B");

        public static IndicatorSource ReportSourceC = new IndicatorSource(IndicatorSourceValue.ReportSourceC,"Report Source C");

        public static IndicatorSource ReportSourceD = new IndicatorSource(IndicatorSourceValue.ReportSourceD,"Report Source D");

        public static IndicatorSource UnderwriterOverride = new IndicatorSource(IndicatorSourceValue.UnderwriterOverride, "UnderwriterOverride");

        public static IndicatorSource ClientManagementOverride =new IndicatorSource(IndicatorSourceValue.ClientManagementOverride, "ClientManagementOverride");

        public IndicatorSource(IndicatorSourceValue value, string displayName) : base((int) value, displayName)
        {
        }
        


        public static IndicatorSource FromValue(IndicatorSourceValue value)
        {
            return FromInt32((int) value);
        }
    }
}