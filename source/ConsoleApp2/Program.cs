using ConsoleApp2.Components;
using ConsoleApp2.Core;
using ConsoleApp2.Systems;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Extensions;
using System;
using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var done = new ManualResetEventSlim(false);
            using (var shutdownCts = new CancellationTokenSource())
            {
                try
                {
                    AttachCtrlcSigtermShutdown(shutdownCts, done);

                    // TODO: Start your tasks here
                    Console.WriteLine("Application is running. Press Ctrl+C to shut down.");

                    ServiceLocator.Current.Register<IGameTimer>(
                        () => new GameTimer(TimeSpan.FromSeconds(1.0d)),
                        InstanceLifetime.Singleton
                    );
                    ServiceLocator.Current.Register<IRenderer, GameRenderer>(InstanceLifetime.Singleton);

                    var world = new World(new DependencyProviderAdapter(ServiceLocator.Current));
                    var waves = new Entity("Waves");
                    var wave1 = CreateWave1();

                    waves.Children.Add(wave1);

                    // register systems
                    ServiceLocator.Current.Register<IWorld>(() => world, InstanceLifetime.Singleton);

                    world.RegisterSystem<UpdateEnemiesSystem>();
                    world.RegisterSystem<MoveEnemiesSystem>();
                    world.RegisterSystem<RenderEnemiesSystem>();

                    // build initial entities
                    world.Root.Add(new ViewportComponent
                    {
                        Width = 10.0f,
                        Height = 8.0f
                    });
                    world.Root.Children.Add(waves);
                    world.Root.Children.Add(new Entity("CurrentWave", wave1));

                    await world.ExecuteAsync(shutdownCts.Token);

                    Console.WriteLine("Application is shutting down...");
                    // TODO: Stop your tasks here
                }
                finally
                {
                    done.Set();
                }
            }
        }

        private static void AttachCtrlcSigtermShutdown(CancellationTokenSource cts, ManualResetEventSlim resetEvent)
        {
            void Shutdown()
            {
                if (resetEvent.IsSet)
                {
                    return;
                }

                try
                {
                    cts.Cancel(false);
                }
                catch (ObjectDisposedException)
                {
                }
                resetEvent.Wait();
            };

            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) => Shutdown();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Shutdown();
                // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
                eventArgs.Cancel = true;
            };
        }

        private static EntityBase CreateWave1()
        {
            var wave1 = new Entity("Wave1");
            var enemies = new Entity("Enemies");

            wave1.Add(new WaveComponent
            {
                Number = 1
            });

            wave1.Children.Add(enemies);

            var position = Vector2.One;

            for (var index = 0; index < 3; index++)
            {
                var enemy = new Entity("Enemy");

                enemy.Add(new RenderComponent());
                enemy.Add(new UpdateComponent());
                enemy.Add(new MoveComponent
                {
                    Position = Vector2.Add(position, new Vector2(0.75f * index, 0.5f * index)),
                    Speed = new Vector2(0.5f, 1.0f)
                });

                enemies.Children.Add(enemy);
            }

            return wave1;
        }
    }
}
