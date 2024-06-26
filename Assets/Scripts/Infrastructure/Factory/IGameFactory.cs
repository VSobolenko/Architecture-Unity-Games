using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enemy;
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
    Task<GameObject> CreateHero(Vector3 at);
    Task<GameObject> CreateHud();
    void Cleanup();
    Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent);
    Task<LootPiece> CreateLoot();
    Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
    Task WarmUp();
}
}