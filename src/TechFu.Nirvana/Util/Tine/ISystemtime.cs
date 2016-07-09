using System;

namespace TechFu.Core.Util.DateTimeHelpers
{
    public interface ISystemTime
    {
        DateTime UtcNow();
    }
}