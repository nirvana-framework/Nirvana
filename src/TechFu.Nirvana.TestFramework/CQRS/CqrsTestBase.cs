namespace TechFu.Nirvana.TestFramework.CQRS
{
    public abstract class CqrsTestBase<TSutType, TTaskType> : TestBase<TSutType, TTaskType> where TTaskType : new()
    {
    }
}