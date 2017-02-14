using Nirvana.Domain;

namespace TechFu.Nirvana.TestFramework.CQRS
{
    public abstract class CqrsTestBase<TSutType, TTaskType, TRootType> : TestBase<TSutType, TTaskType,TRootType> 
        where TTaskType : new()
        where TRootType: RootType
    {
    }
}