namespace ClassLibrary1
{
    public interface ICondition<in T>
    {
        bool IsMet(T value);
    }
}