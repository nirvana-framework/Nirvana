using System;

namespace Nirvana.Security
{
    public interface IClaimAuthorizer
    {
        bool CheckByUserId(string userId, ClaimType requiredClaimType, AccessType requiredAccessType);
    }

    public enum AccessType
    {
        Any,
        Create,
        Read,
        Update,
        Delete
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