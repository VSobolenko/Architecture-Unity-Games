using System;
using Data;
using Enemy;
using Hero;
using Infrastructure;
using Infrastructure.Services;
using StaticData;
using UnityEngine;

namespace Logic
{
public class EnemySpawner : MonoBehaviour, ISavedProgress
{
    public MonsterTypeId monsterTypeId;
    private string _id;

    public bool slain;
    private IGameFactory _factory;
    private EnemyDeath _enemyDeath;

    private void Awake()
    {
        _id = GetComponent<UniqueId>().id;
        _factory = AllServices.Container.Single<IGameFactory>();
    }

    public void LoadProgress(PlayerProgress progress)
    {
        if (progress.killData.clearedSpawners.Contains(_id))
            slain = true;
        else
            Spawn();
    }

    private void Spawn()
    {
        var monster = _factory.CreateMonster(monsterTypeId, transform);
        _enemyDeath = monster.GetComponent<EnemyDeath>();
        _enemyDeath.Happened += Slay;
    }

    private void Slay()
    {
        if (_enemyDeath is not null) _enemyDeath.Happened -= Slay;
        slain = true;
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        if (slain)
            progress.killData.clearedSpawners.Add(_id);
    }
}
}