using System;

namespace TechFu.Nirvana.Util.Tine
{
    public class SystemTime : ISystemTime
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}