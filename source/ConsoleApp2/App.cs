using ClassLibrary1;
using ClassLibrary1.Extensions;
using ConsoleApp2.Components;
using ConsoleApp2.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp2.Core;
using ConsoleApp2.Systems;

namespace ConsoleApp2
{
    public class App
    {
        private readonly Entity root;

        public App(params string[] args)
        {
            root = new Entity();
            root.AddComponent<GameTimeComponent>();

            var environment = root.GetComponent<GameTimeComponent>();
            var queue = new MessageQueue();

            environment.Elapsed.Subscribe(value =>
            {
                queue
                    .For<TickMessage>()
                    .OnNext(new TickMessage(value));
            });
            var update = new UpdateElapsedTimeSystem(queue.For<TickMessage>());
            var temp = root.Subscribe(update);
            var spawn = new Entity();
            var waveSetup = new EnemyWaveSetup
            {
                Tick = queue.For<TickMessage>(),
                Waves = new[]
                {
                    new EnemyWave
                    {
                        NumberOfEnemies = 10
                    }
                }
            };

                waveSetup.Apply(spawn);

                root.Children.Add(spawn);
        }

        public void Run()
        {
            var cts = new CancellationTokenSource();
            var task = DoRun(cts.Token);

            task.GetAwaiter().GetResult();
        }

        private async Task DoRun(CancellationToken ct)
        {
            var started = TimeSpan.FromTicks(DateTime.UtcNow.Ticks);
            var environment = root.GetComponent<GameTimeComponent>();

            while (false == ct.IsCancellationRequested)
            {
                environment.Elapsed.Value = TimeSpan.FromTicks(DateTime.UtcNow.Ticks) - started;
                await Task.Delay(TimeSpan.FromMilliseconds(300.0d), ct);
            }
        }
    }
}