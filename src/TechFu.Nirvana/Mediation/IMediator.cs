using System;
using System.Diagnostics;
using System.Reflection;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.Mediation
{
    public interface IMediator
    {
        CommandResponse<TResult> Command<TResult>(Command<TResult> command);
        QueryResponse<TResult> Query<TResult>(Query<TResult> query);
    }


    [DebuggerNonUserCode]
    public class Mediator : IMediator
    {
        private readonly NirvanaConfiguration _nirvanaConfiguration;

        public Mediator(NirvanaConfiguration nirvanaConfiguration   )
        {
            _nirvanaConfiguration = nirvanaConfiguration;
        }

        private const string HandleMethod = "Handle";

        public CommandResponse<TResult> Command<TResult>(Command<TResult> command)
        {
            var plan = new MediatorPlan<TResult>(typeof(ICommandHandler<,>), HandleMethod, command.GetType(), _nirvanaConfiguration);
            return plan.InvokeCommand(command);
        }

        public QueryResponse<TResult> Query<TResult>(Query<TResult> query)
        {
            var plan = new MediatorPlan<TResult>(typeof(IQueryHandler<,>), HandleMethod, query.GetType(), _nirvanaConfiguration);
            return plan.InvokeQuery(query);
        }


        private class MediatorPlan<TResult>
        {
            private readonly Func<object> _getHandler;
            private readonly MethodInfo _handleMethod;
            private readonly Type _handlerType;

            public MediatorPlan(Type handlerTypeTemplate, string handlerMethodName, Type messageType,NirvanaConfiguration configuration)
            {
                var genericHandlerType = handlerTypeTemplate.MakeGenericType(messageType, typeof(TResult));
                var handler = configuration.GetService(genericHandlerType);
                var needsNewHandler = false;

                _handleMethod = GetHandlerMethod(genericHandlerType, handlerMethodName, messageType);
                _handlerType = handler.GetType();
                _getHandler = () =>
                {
                    if (needsNewHandler)
                        handler = configuration.GetService(genericHandlerType);

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
                        new[] { messageType },
                        null);
            }

            public QueryResponse<TResult> InvokeQuery(Query<TResult> message)
            {
                Func<QueryResponse<TResult>> execute =
                    () => (QueryResponse<TResult>)_handleMethod.Invoke(_getHandler(), new object[] { message });


                return execute();
            }

            public CommandResponse<TResult> InvokeCommand(Command<TResult> message)
            {
                Func<CommandResponse<TResult>> execute =
                    () => (CommandResponse<TResult>)_handleMethod.Invoke(_getHandler(), new object[] { message });


                return execute();
            }
        }
    }

}