namespace ClassLibrary1
{
    public interface ICompletable : IError
    {
        void OnCompleted();
    }
}