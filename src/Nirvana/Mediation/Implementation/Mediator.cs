using System;
using System.Reflection;
using Nirvana.Configuration;
using Nirvana.CQRS;

namespace Nirvana.Mediation.Implementation
{
    public class Mediator : ILocalMediator
    {

        private readonly NirvanaSetup _setup;

        public Mediator(NirvanaSetup setup)
        {
            _setup = setup;
        }


        private const string HandleMethod = "Handle";

        public CommandResponse<TResult> Command<TResult>(Command<TResult> command)
        {
           
                var plan = new MediatorPlan<TResult>(typeof(ICommandHandler<,>), HandleMethod, command.GetType());
                return plan.InvokeCommand(command);
            
        }

        public QueryResponse<TResult> Query<TResult>(Query<TResult> query)
        {
            var plan = new MediatorPlan<TResult>(typeof(IQueryHandler<,>), HandleMethod, query.GetType());
            return plan.InvokeQuery(query);
        }
 
        public UIEventResponse UiNotification<T>(UiEvent<T> uiEevent)
        {
            throw new NotImplementedException();
        }

        public InternalEventResponse InternalEvent(InternalEvent internalEvent)
        {
           return (_setup.GetService(typeof(InternalEventProcessor)) as InternalEventProcessor).Process(internalEvent);
        }


        private class MediatorPlan<TResult>
        {
            private readonly Func<object> _getHandler;
            private readonly MethodInfo _handleMethod;

            private readonly NirvanaSetup _setup;

            public MediatorPlan(NirvanaSetup setup)
            {
                _setup = setup;
            }

            public MediatorPlan(Type handlerTypeTemplate, string handlerMethodName, Type messageType)
            {
                var genericHandlerType = handlerTypeTemplate.MakeGenericType(messageType, typeof(TResult));
                var handler = _setup.GetService(genericHandlerType);
                var needsNewHandler = false;

                _handleMethod = GetHandlerMethod(genericHandlerType, handlerMethodName, messageType);
                _getHandler = () =>
                {
                    if (needsNewHandler)
                        handler = _setup.GetService(genericHandlerType);
                    //Get a new instance on retry
                    needsNewHandler = true;

                    return handler;
                };
            }

            private static MethodInfo GetHandlerMethod(Type handlerType, string handlerMethodName, Type messageType)
            {
                return handlerType
                    .GetMethod(handlerMethodName,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                        null, CallingConventions.HasThis,
                        new[] {messageType},
                        null);
            }

            public QueryResponse<TResult> InvokeQuery(Query<TResult> message)
            {
                Func<QueryResponse<TResult>> execute =
                    () => (QueryResponse<TResult>) _handleMethod.Invoke(_getHandler(), new object[] {message});


                return execute();
            }

            public CommandResponse<TResult> InvokeCommand(Command<TResult> message)
            {
                Func<CommandResponse<TResult>> execute =
                    () => (CommandResponse<TResult>) _handleMethod.Invoke(_getHandler(), new object[] {message});


                return execute();
            }
        }
    }
}