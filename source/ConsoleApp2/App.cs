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
        private EntityBase root;
        private readonly EntityFactory factory;

        public App(params string[] args)
        {
            factory = EntityFactory.Default;
            factory.PrototypeResolver.Initialize(new EntityState
            {
                Children = new[]
                {
                    new EntityState
                    {
                        Key = "Enemy",
                        Components = new ComponentState [0]
                    }
                }
            });
        }

        public void Run()
        {
            root = new Entity("Root");

            var enemy = factory.CreateEntity("/Enemy");

            root.Children.Add(enemy);

            using (var cts = new CancellationTokenSource())
            {
                var task = DoRun(cts.Token);
                task.GetAwaiter().GetResult();
            }
        }

        private async Task DoRun(CancellationToken ct)
        {
            var started = TimeSpan.FromTicks(DateTime.UtcNow.Ticks);
            var environment = root.Get<GameTimeComponent>();

            while (false == ct.IsCancellationRequested)
            {
                environment.Elapsed.Value = TimeSpan.FromTicks(DateTime.UtcNow.Ticks) - started;
                await Task.Delay(TimeSpan.FromMilliseconds(300.0d), ct);
            }
        }
    }
}