using System;
using System.Threading;
using ClassLibrary1;
using ConsoleApp2.Components;
using ConsoleApp2.Core;
using ConsoleApp2.Systems;
using LibraProgramming.Dependency.Container;

namespace ConsoleApp2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dependencyInjectionAdapter = new InjectionLocatorAdapter(ServiceLocator.Current);

            ServiceLocator.Current.Register<IGameTimer>(
                () => new GameTimer(TimeSpan.FromSeconds(1.0d)),
                InstanceLifetime.Singleton
            );

            var world = new World(dependencyInjectionAdapter);
            var enemy = new Entity("Enemy1");

            world.RegisterSystem<UpdateSystem>();

            enemy.Add(new UpdateComponent());

            world.Root.Children.Add(enemy);

            world.ExecuteAsync(CancellationToken.None).Wait();
        }
    }
}
