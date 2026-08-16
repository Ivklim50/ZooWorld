using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZooWorld.Animals;
using ZooWorld.Spawning;
using ZooWorld.Stats;
using ZooWorld.UI;
using ZooWorld.World;

namespace ZooWorld.Core
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameSettings _settings;
        [SerializeField] private Camera _camera;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_settings);

            builder.RegisterComponent<Camera>(_camera);
            builder.Register<IWorldBounds, CameraWorldBounds>(Lifetime.Singleton);

            builder.Register<IFoodChainResolver, TagFoodChainResolver>(Lifetime.Singleton);
            builder.Register<IGameEvents, GameEvents>(Lifetime.Singleton);
            builder.Register<IAnimalFactory, AnimalFactory>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameStats>(Lifetime.Singleton).AsSelf();
            builder.RegisterEntryPoint<AnimalSpawner>();

            builder.RegisterComponentInHierarchy<StatsPresenter>();
            builder.RegisterComponentInHierarchy<KillLabelSpawner>();
        }
    }
}
