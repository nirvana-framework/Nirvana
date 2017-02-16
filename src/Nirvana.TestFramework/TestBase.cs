using System;
using System.Linq.Expressions;
using Nirvana.Data;
using Nirvana.Domain;
using Nirvana.Mediation;
using NSubstitute;

namespace TechFu.Nirvana.TestFramework
{
    public abstract class TestBase<TSutType, TTaskType,TRootType> 
        where TTaskType : new()
        where TRootType:RootType

    {
        protected IMediator Mediator;
        protected TSutType Sut;
        protected TTaskType Task;
        protected IRepository<TRootType> Repository;
        public abstract Func<TSutType> Build { get; }

        protected TestBase()
        {
            Repository = Substitute.For<IRepository<TRootType>>();
            Mediator = Substitute.For<IMediator>();
            Task = new TTaskType();

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