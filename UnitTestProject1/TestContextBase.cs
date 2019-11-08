using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    public abstract class TestContextBase : ITestContext
    {
        [TestInitialize]
        public void Setup()
        {
            Arrange();
            Act();
        }

        public abstract void Arrange();

        public abstract void Act();

        [TestCleanup]
        public virtual void Cleanup()
        {
        }
    }
}