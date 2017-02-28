using System;

namespace Nirvana.Security
{
    public interface IClaimAuthorizer
    {
        bool CheckByUserId(string userId, ClaimType requiredClaimType, AccessType requiredAccessType);
    }

    public enum AccessType
    {
        Create=1,
        Read=2,
        Update=4,
        Delete= 8,
        Any = 16
    }

    public class ClaimType
    {
    }


    public abstract class ClaimTypeAttribute : Attribute
    {
        public ClaimType ClaimType { get; set; }
        public AccessType[] AllowedActions { get; set; }
    }
}