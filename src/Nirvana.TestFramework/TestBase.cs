using System;
using System.Linq.Expressions;
using Nirvana.Mediation;

namespace Nirvana.TestFramework
{
    public abstract class TestBase<TSutType, TInputType>
        where TInputType : new()

    {
        protected IMediator Mediator;
        protected TSutType Sut;
        protected TInputType Task;
        public abstract Func<TSutType> Build { get; }

        protected TestBase()
        {
            Task = new TInputType();
        }

        public virtual void SetUpData()
        {
        }


        public void SetupBuildAndRun()
        {
            SetUpData();
            Sut = Build();
            RunTest();
        }

        public abstract void RunTest();

        public T Any<T>()
        {
            return Arg<T>.Is.Anything;
        }

        public T Matches<T>(Expression<Predicate<T>> match)
        {
            return Arg<T>.Matches(match);
        }

        public T Matches<T>(T value)
        {
            return Arg<T>.Matches(x => x.Equals(value));
        }
    }
}