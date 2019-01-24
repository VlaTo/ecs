using System.Collections.Generic;
using ClassLibrary1.Core;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ObservableProperty<TValue> : ObservableBase<TValue>
    {
        private static readonly IEqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;

        private TValue current;

        public IComponent Component
        {
            get;
        }

        public TValue Value
        {
            get => current;
            set
            {
                if (comparer.Equals(current, value))
                {
                    return;
                }

                current = value;

                Next(value);
            }
        }

        public ObservableProperty(IComponent component)
        {
            Component = component;
        }
    }
}