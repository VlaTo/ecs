namespace Tests
{
    public interface ITestContext
    {
        void Arrange();
        
        void Act();

        void Cleanup();
    }
}