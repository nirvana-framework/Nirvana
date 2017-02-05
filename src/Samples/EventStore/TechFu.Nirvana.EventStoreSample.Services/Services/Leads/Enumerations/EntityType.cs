using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Leads.Enumerations
{
    public enum EntityTypeValue
    {
        SoleProp=1,
        LLC=2
    }

    public class EntityType : Enumeration<EntityType>
    {
        public static EntityType SoleProp = new EntityType(EntityTypeValue.SoleProp, "Sole Prop");
        public static EntityType LLC = new EntityType(EntityTypeValue.LLC, "LLC");

        public EntityType(EntityTypeValue value, string displayName) : base((int)value, displayName)
        {
        }
    }
}