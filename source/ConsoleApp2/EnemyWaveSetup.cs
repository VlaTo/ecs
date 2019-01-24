using System;
using ClassLibrary1;
using ClassLibrary1.Extensions;
using ConsoleApp2.Components;
using ConsoleApp2.Messages;
using ConsoleApp2.Systems;

namespace ConsoleApp2
{
    internal class EnemyWave
    {
        public int NumberOfEnemies
        {
            get;
            set;
        }
    }

    internal class EnemyWaveSetup
    {
        public IObservable<TickMessage> Tick
        {
            get;
            set;
        }

        public EnemyWave[] Waves
        {
            get;
            set;
        }

        public void Apply(Entity entity)
        {
            foreach (var config in Waves)
            {
                var wave = entity.Add<EnemyWavesComponent>();
                wave.NumberOfEnemies = config.NumberOfEnemies;
            }

            entity.Add<UpdateComponent>();
            entity.Add<CurrentWaveComponent>(wave =>
            {
                wave.Number.Value = 1;
            });

            var system = new CurrentWaveSystem(Tick);

            entity.Subscribe(system);
        }
    }
}