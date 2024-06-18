using System;
using System.Collections.Generic;
using Hero;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure
{
public interface IGameFactory : IService
{
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    GameObject HeroGameObject { get; }
    event Action HeroCreated;
    GameObject CreateHero(GameObject at);
    void CreateHub();
    void Cleanup();
}
}