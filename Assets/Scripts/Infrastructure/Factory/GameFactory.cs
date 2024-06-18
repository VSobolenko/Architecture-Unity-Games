using System;
using System.Collections.Generic;
using Hero;
using UnityEngine;

namespace Infrastructure
{
public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    public GameObject HeroGameObject { get; private set; }
    public event Action HeroCreated;

    public GameFactory(IAssets assets)
    {
        _assets = assets;
    }

    public GameObject CreateHero(GameObject at)
    {
        HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
        HeroCreated?.Invoke();
         
        return HeroGameObject;
    }

    public void CreateHub() => InstantiateRegistered(AssetPath.HudPath);

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

    private void Register(ISavedProgressReader progressReader)
    {
        if (progressReader is ISavedProgress progressWriter)
        {
            ProgressWriters.Add(progressWriter);
        }

        ProgressReaders.Add(progressReader);
    }
}
}