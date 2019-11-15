using System.Composition;
using System.Composition.Hosting;
using App1;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Game.Towers.Core;
using LibraProgramming.Game.Towers.Extensions;
using LibraProgramming.Game.Towers.Systems;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;
using Windows.UI.Xaml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace LibraProgramming.Game.Towers.Views
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.Register<IGameTimer>(
                () => new GameTimer(AnimatedControl),
                InstanceLifetime.Singleton
            );
            ServiceLocator.Current.Register<IGameRenderer>(
                () => new GameRenderer(AnimatedControl),
                InstanceLifetime.Singleton
            );
            ServiceLocator.Current.Register(
                () =>
                {
                    var loggerFactory = new LoggerFactory(
                        new[]
                        {
                            new DebugLoggerProvider()
                        }
                    );
                    return loggerFactory.CreateLogger("Debug");
                },
                InstanceLifetime.Singleton
            );
            ServiceLocator.Current.Register<IEnemyMoveStrategy, EnemyMoveByPathStrategy>(InstanceLifetime.Singleton);

            var world = new World(new DependencyProviderAdapter(ServiceLocator.Current));

            /*var containerConfiguration = new ContainerConfiguration()
                .WithAssembly(typeof(ISystem).Assembly)
                .WithAssembly(typeof(App).Assembly);

            containerConfiguration.WithPart<IWorld>(new )

            var container = containerConfiguration.CreateContainer();

            container.SatisfyImports(world);*/

            ServiceLocator.Current.Register<IWorld>(() => world, InstanceLifetime.Singleton);

            // register systems
            world.RegisterSystem<UpdateEnemiesSystem>();
            world.RegisterSystem<MoveEnemySystem>();
            //world.RegisterSystem<MoveEntitySystem>();
            world.RegisterSystem<RenderEnemiesSystem>();
            world.RegisterSystem<EnemyWaveSystem>();

            LoadWorld(world, CreateEntityFactory());

            world.ExecuteAsync(CancellationToken.None).RunAndForget();
        }

        private static void LoadWorld(IWorld world, EntityFactory entityFactory)
        {
            var fileProvider = new EmbeddedResourceFileProvider("LibraProgramming.Game.Towers.Data.", Assembly.GetExecutingAssembly());

            using (var file = fileProvider.GetFile("Towers.xml"))
            {
                using (var stream = file.OpenRead())
                {
                    var serializer = new XmlSerializer(typeof(EntityState));
                    var state = (EntityState)serializer.Deserialize(stream);

                    entityFactory.LoadEntity(world.Root, state);
                }
            }
        }

        private static EntityFactory CreateEntityFactory()
        {
            var assemblies = new[]
            {
                typeof(App).Assembly,
                typeof(Entity).Assembly
            };

            var componentResolver = ComponentResolver.FromAssemblies(assemblies);

            return new EntityFactory(componentResolver);
        }
    }
}
