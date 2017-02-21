using System;
using System.Collections.Generic;

namespace Nirvana.Security
{
    public enum AccessType
    {
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
        public ClaimType ClaimType{ get; set; }
        public AccessType[] AllowedActions{ get; set; }
    }
}