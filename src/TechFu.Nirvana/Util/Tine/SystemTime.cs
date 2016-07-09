using System;
using TechFu.Core.Util.DateTimeHelpers;

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