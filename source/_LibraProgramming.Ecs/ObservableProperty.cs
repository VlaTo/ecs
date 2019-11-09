using LibraProgramming.Ecs.Core.Reactive;
using System.Collections.Generic;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ObservableProperty<TValue> : ObservableBase<TValue>
    {
        private static readonly IEqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;

        private TValue current;

        /// <summary>
        /// 
        /// </summary>
        public IComponent Component
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        public ObservableProperty(IComponent component)
        {
            Component = component;
        }
    }
}