namespace UnitTestProject1
{
    public interface ITestContext
    {
        void Arrange();
        
        void Act();

        void Cleanup();
    }
}