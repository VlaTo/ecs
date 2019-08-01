namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityObserver : ICompletable
    {
        void OnAdded(IComponent component);

        void OnRemoved(IComponent component);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public interface IEntityObserver<in TComponent> : ICompletable
        where TComponent : IComponent
    {
        void OnAdded(TComponent component);

        void OnRemoved(TComponent component);
    }
}