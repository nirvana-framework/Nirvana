using System;
using Nirvana.CQRS;
using Nirvana.Mediation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace Nirvana.TestFramework
{
    public abstract class BddTestBase<TSut, TInput, TResult> 
    {
        protected BddTestBase()
        {
            SetupBuildAndRun();
        }

        protected IFixture Depends;
        protected TInput Input;
        public bool RequiresIoCContainer = false;
        protected TResult Result;
        protected TSut Sut;

        public abstract Action Inject { get; }
        public virtual Action Establish => () => { };
        public abstract Action Because { get; }

        private void SetupBuildAndRun()
        {
            Depends = new Fixture().Customize(new AutoNSubstituteCustomization());
            Inject();
            Build();
            Establish();

            RunTest();
        }

        private void RunTest()
        {
            Because();
        }

        public void Build()
        {
            Sut = Depends.Create<TSut>();
        }

        public T DependsOn<T>()
        {
            return Depends.Freeze<T>();
        }

        public T CreateStub<T>()
        {
            return Depends.Create<T>();
        }

        public void DependsOnConcrete<T>(T input)
        {
            Depends.Inject(input);
        }
    }

    public abstract class BddQueryTestBase<TSutType, TQueryType, TResponseType>
        : BddTestBase<TSutType, TQueryType, QueryResponse<TResponseType>>
        where TQueryType : Query<TResponseType>, new()
        where TSutType : IQueryHandler<TQueryType, TResponseType>

    {
        public override Action Because => () => Result = Sut.Handle(Input);
    }

    public abstract class BddCommandTestBase<TSutType, TQueryType, TResponseType>
        : BddTestBase<TSutType, TQueryType, CommandResponse<TResponseType>>
        where TQueryType : Command<TResponseType>, new()
        where TSutType : ICommandHandler<TQueryType, TResponseType>

    {
        public override Action Because => () => Result = Sut.Handle(Input);
    }

    
}