using System;
using TechFu.Nirvana.Data.EntityTypes;

namespace TechFu.Nirvana.EventStoreSample.Domain.Domain.Security
{
    public class SiteUser : Entity<Guid>
    {
        public string Name { get; set; }
        public bool IsAnonomous{ get; set; }
        public DateTime LastLogin{ get; set; }
        public int LoginCount{ get; set; }
    }
}