using Moq.AutoMock;

namespace Calendar.AgendaScheduler.Domain.Tests.Fixtures
{
    public abstract class BaseFixture<T> where T : class
    {
        protected BaseFixture()
        {
            Mocker = new AutoMocker();
        }

        protected AutoMocker Mocker { get; }
        protected T Subject => Mocker.CreateInstance<T>();
    }
}