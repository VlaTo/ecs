namespace UnitTestProject2
{
    public interface ITestContext
    {
        void Arrange();
        
        void Act();

        void Cleanup();
    }
}