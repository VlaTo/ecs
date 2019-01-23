using System.Collections.Generic;

namespace ClassLibrary1
{
    /// <summary>
    /// Observable collection of the <see cref="Entity" /> items.
    /// </summary>
    public interface IEntityCollection : IList<Entity>, IObservableCollection<Entity>
    {
    }
}