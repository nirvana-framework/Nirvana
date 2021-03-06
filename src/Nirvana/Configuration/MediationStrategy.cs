﻿using Nirvana.Domain;

namespace Nirvana.Configuration
{
    public class MediationStrategy:Enumeration<MediationStrategy>
    {
        //This should be used for sender apps
        public static MediationStrategy None = new MediationStrategy(-1, "Disabled");

        //All Commands and UI Notifications handled in this app
        public static MediationStrategy InProcess = new MediationStrategy(1, "In Process");

        //Forward Commands and UI Notifications to a web mediator
        public static MediationStrategy ForwardToWeb= new MediationStrategy(2, "To Web");

        //Forward Commands and UI Notifications to a queue
        public static MediationStrategy ForwardToQueue= new MediationStrategy(3, "To Queue");
        
        //Specify a convention for determining in process vs queued processing for long running processes
        public static MediationStrategy ForwardLongRunningToQueue= new MediationStrategy(4, "To Queue For Long Running Processes");

        //Forward Commands and UI Notifications to and event store
        //When using this, the long running check is disabled - event sourcing is all or nothing
        public static MediationStrategy ForwardToEventStore= new MediationStrategy(5, "To Event Store");

        public MediationStrategy(int value, string displayName) : base(value, displayName)
        {
        }
    }

   
}