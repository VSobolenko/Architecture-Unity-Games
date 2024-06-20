using System;
using System.Collections.Generic;
using Hero;
using Infrastructure.Services;
using Logic;
using StaticData;
using UnityEngine;

namespace Infrastructure
{
public interface IGameFactory : IService
{
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    GameObject CreateHero(GameObject at);
    GameObject CreateHud();
    void Cleanup();
    void Register(ISavedProgressReader progressReader);
    GameObject CreateMonster(MonsterTypeId typeId, Transform parent);
}
}