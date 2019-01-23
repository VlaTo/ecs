using System;
using ClassLibrary1;
using ConsoleApp2.Components;
using ConsoleApp2.Messages;
using ClassLibrary1.Extensions;

namespace ConsoleApp2.Systems
{
    public class CurrentWaveSystem : IEntityObserver<CurrentWaveComponent>
    {
        private IDisposable tickSubscription;
        private IDisposable numberSubscription;
        private IDisposable cooldownSubscription;
        private TimeSpan last;

        public CurrentWaveSystem(IObservable<TickMessage> tick)
        {
            last = TimeSpan.Zero;
            tickSubscription = tick.Subscribe(DoTick);
        }

        public void OnAdded(CurrentWaveComponent component)
        {
            component.Number.Value = 1;
            component.Cooldown.Value = TimeSpan.FromSeconds(1.0d);

            numberSubscription = component.Number.Subscribe(DoCurrentWaveNumber);
            cooldownSubscription = component.Cooldown.Subscribe(DoCurrentWaveCooldown);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            numberSubscription.Dispose();
            cooldownSubscription.Dispose();
        }

        public void OnRemoved(CurrentWaveComponent component)
        {
            numberSubscription.Dispose();
            cooldownSubscription.Dispose();
        }

        private void DoCurrentWaveNumber(int value)
        {
            Console.WriteLine($"[CurrentWaveSystem.DoCurrentWaveNumber] value: {value}");
        }

        private void DoCurrentWaveCooldown(TimeSpan value)
        {

        }

        private void DoTick(TickMessage message)
        {
            if (TimeSpan.Zero == last)
            {
                last = message.Elapsed;
            }

            var duration = message.Elapsed - last;

            foreach (var component in Components)
            {
                component.Cooldown.Value -= duration;

                if (TimeSpan.Zero <= component.Cooldown.Value)
                {
                    component.Number.Value += 1;
                    component.Cooldown.Value = TimeSpan.FromSeconds(3.0d);
                }
            }

            last = message.Elapsed;
        }
    }
}