using System;
using System.Linq.Expressions;
using NSubstitute;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.TestFramework
{
    public abstract class TestBase<TSutType, TTaskType> where TTaskType : new()
    {
        protected IMediator Mediator;
        protected IRepository Repository;
        protected TSutType Sut;
        protected TTaskType Task;
        public abstract Func<TSutType> Build { get; }

        protected TestBase()
        {
            Repository = Substitute.For<IRepository>();
            Mediator = Substitute.For<IMediator>();
            Task = new TTaskType();
            SetupBuildAndRun();
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