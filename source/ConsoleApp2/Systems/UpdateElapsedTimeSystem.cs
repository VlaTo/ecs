using System;
using System.Collections.Generic;
using ClassLibrary1;
using ConsoleApp2.Components;
using ConsoleApp2.Messages;

namespace ConsoleApp2.Systems
{
    public class UpdateElapsedTimeSystem : IEntityObserver<UpdateComponent>
    {
        private readonly IDisposable subscription;
        private readonly IList<UpdateComponent> components;

        internal UpdateElapsedTimeSystem(IObservable<TickMessage> ticks)
        {
            components = new List<UpdateComponent>();
            subscription = ticks.Subscribe(new TickMessageObserver(this));
        }

        public void OnAdded(UpdateComponent component)
        {
            components.Add(component);
        }

        public void OnCompleted()
        {
            components.Clear();
        }

        public void OnError(Exception error)
        {
            components.Clear();
        }

        public void OnRemoved(UpdateComponent component)
        {
            components.Remove(component);
        }

        private void DoTick(TimeSpan elapsed)
        {
            foreach (var component in components)
            {
                component.Elapsed.Value = elapsed;
            }
        }

        private void Unsubscribe()
        {
            subscription.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        private class TickMessageObserver : IObserver<TickMessage>
        {
            private readonly UpdateElapsedTimeSystem system;

            public TickMessageObserver(UpdateElapsedTimeSystem system)
            {
                this.system = system;
            }

            public void OnCompleted()
            {
                system.Unsubscribe();
            }

            public void OnError(Exception error)
            {
                system.Unsubscribe();
            }

            public void OnNext(TickMessage value)
            {
                system.DoTick(value.Elapsed);
            }
        }
    }
}