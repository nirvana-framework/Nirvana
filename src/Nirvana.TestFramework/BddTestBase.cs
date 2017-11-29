using System;
using System.IO;
using System.Reflection;
using AutoFixture;
using AutoFixture.AutoNSubstitute;

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

        public string GetFolderPath(string relativePath)
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return Path.Combine(dirPath, relativePath);
        }
    }
}