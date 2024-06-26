using System.Collections.Generic;
using System.Threading.Tasks;
using Enemy;
using Hero;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using Logic;
using StaticData;
using UI;
using UI.Elements;
using UI.Services;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace Infrastructure
{
public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;
    private readonly IStaticDataService _staticData;
    private readonly IRandomService _randomService;
    private readonly IPersistentProgressService _persistentProgressService;
    private readonly IWindowsService _windowServices;
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    private GameObject HeroGameObject { get; set; }

    public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService,
                       IPersistentProgressService persistentProgressService, IWindowsService windowServices)
    {
        _assets = assets;
        _staticData = staticData;
        _randomService = randomService;
        _persistentProgressService = persistentProgressService;
        _windowServices = windowServices;
    }

    public async Task WarmUp()
    {
        await _assets.Load<GameObject>(AssetAddress.Loot);
        await _assets.Load<GameObject>(AssetAddress.Spawner);
    }

    public async Task<GameObject> CreateHero(Vector3 at)
    {
        HeroGameObject = await InstantiateRegisteredAsync(AssetAddress.HeroPath, at);

        return HeroGameObject;
    }

    public async Task<GameObject> CreateHud()
    {
        var hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);
        hud.GetComponentInChildren<LootCounter>().Construct(_persistentProgressService.Progress.worldData);

        foreach (var openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
        {
            openWindowButton.Construct(_windowServices);
        }

        return hud;
    }

    public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
    {
        var monsterData = _staticData.ForMonster(typeId);

        var prefab = await _assets.Load<GameObject>(monsterData.prefabReference);

        var monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
        var health = monster.GetComponent<IHealth>();
        health.Current = monsterData.hp;
        health.Max = monsterData.hp;

        monster.GetComponent<ActorUI>().Construct(health);
        monster.GetComponent<AgentMoveToHero>().Construct(HeroGameObject.transform);
        monster.GetComponent<NavMeshAgent>().speed = monsterData.moveSpeed;
        monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);

        var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
        lootSpawner.Construct(this, _randomService);
        lootSpawner.SetLoot(monsterData.minLoot, monsterData.maxLoot);

        var attack = monster.GetComponent<Attack>();
        attack.Construct(HeroGameObject.transform);
        attack.damage = monsterData.damage;
        attack.cleavage = monsterData.cleavage;
        attack.effectiveDistance = monsterData.effectiveDistance;

        return monster;
    }

    public async Task<LootPiece> CreateLoot()
    {
        var prefab = await _assets.Load<GameObject>(AssetAddress.Loot);

        var loot = InstantiateRegisteredAsync(prefab).GetComponent<LootPiece>();
        loot.Construct(_persistentProgressService.Progress.worldData);

        return loot;
    }

    public async Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
    {
        var prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
        var spawner = InstantiateRegisteredAsync(prefab, at)
            .GetComponent<SpawnPoint>();

        spawner.ID = spawnerId;
        spawner.monsterTypeId = monsterTypeId;
        spawner.Construct(this);
    }

    public void Cleanup()
    {
        ProgressReaders.Clear();
        ProgressWriters.Clear();
        _assets.Cleanup();
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
    {
        var gameObject = await _assets.Instantiate(prefabPath, at);
        RegisterProgressWatchers(gameObject);

        return gameObject;
    }

    private GameObject InstantiateRegisteredAsync(GameObject prefab, Vector3 at)
    {
        var gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
        RegisterProgressWatchers(gameObject);

        return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
    {
        var gameObject = await _assets.Instantiate(prefabPath);
        RegisterProgressWatchers(gameObject);

        return gameObject;
    }

    private GameObject InstantiateRegisteredAsync(GameObject prefab)
    {
        var gameObject = Object.Instantiate(prefab);
        RegisterProgressWatchers(gameObject);

        return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
        foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
        {
            Register(progressReader);
        }
    }

    public void Register(ISavedProgressReader progressReader)
    {
        if (progressReader is ISavedProgress progressWriter)
        {
            ProgressWriters.Add(progressWriter);
        }

        ProgressReaders.Add(progressReader);
    }
}
}