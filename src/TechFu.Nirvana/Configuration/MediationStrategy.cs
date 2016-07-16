using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.Configuration
{
    public class MediationStrategy:Enumeration<MediationStrategy>
    {
        public static MediationStrategy InProcess = new MediationStrategy(1, "In Process");
        public static MediationStrategy ForwardToWeb= new MediationStrategy(1, "To Web");
        public static MediationStrategy ForwardToQueue= new MediationStrategy(1, "To Queue");
        public static MediationStrategy ForwardToEventStore= new MediationStrategy(1, "To Event Store");

        public MediationStrategy(int value, string displayName) : base(value, displayName)
        {
        }
    }
}