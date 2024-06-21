using System;
using System.Collections.Generic;
using Enemy;
using Hero;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using StaticData;
using UI;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace Infrastructure
{
public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;
    private readonly IStaticDataService _staticData;
    private IRandomService _randomService;
    private IPersistentProgressService _persistentProgressService;
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    private GameObject HeroGameObject { get; set; }

    public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService,
                       IPersistentProgressService persistentProgressService)
    {
        _assets = assets;
        _staticData = staticData;
        _randomService = randomService;
        _persistentProgressService = persistentProgressService;
    }

    public GameObject CreateHero(GameObject at)
    {
        HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);

        return HeroGameObject;
    }

    public GameObject CreateHud()
    {
        var hud = InstantiateRegistered(AssetPath.HudPath);
        hud.GetComponentInChildren<LootCounter>().Construct(_persistentProgressService.Progress.worldData);

        return hud;
    }

    public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
    {
        var monsterData = _staticData.ForMonster(typeId);
        var monster = Object.Instantiate(monsterData.prefab, parent.position, Quaternion.identity, parent);
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

    public LootPiece CreateLoot()
    {
        var loot = InstantiateRegistered(AssetPath.Loot).GetComponent<LootPiece>();
        loot.Construct(_persistentProgressService.Progress.worldData);

        return loot;
    }

    public void Cleanup()
    {
        ProgressReaders.Clear();
        ProgressWriters.Clear();
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
    {
        var gameObject = _assets.Instantiate(prefabPath, at);
        RegisterProgressWatchers(gameObject);

        return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath)
    {
        var gameObject = _assets.Instantiate(prefabPath);
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