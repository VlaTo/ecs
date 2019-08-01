namespace ClassLibrary3
{
    public interface IStateProvider
    {
        object GetState();
    }

    public interface IStateProvider<out TState>
    {
        TState GetState();
    }
}