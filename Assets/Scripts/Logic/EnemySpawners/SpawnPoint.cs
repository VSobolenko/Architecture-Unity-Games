using Data;
using Enemy;
using Hero;
using Infrastructure;
using Infrastructure.Services;
using StaticData;
using UnityEngine;

namespace Logic
{
public class SpawnPoint : MonoBehaviour, ISavedProgress
{
    public MonsterTypeId monsterTypeId;
    public string ID { get; set; }

    private bool _slain;
    private IGameFactory _factory;
    private EnemyDeath _enemyDeath;

    public void Construct(IGameFactory gameFactory)
    {
        _factory = gameFactory;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        if (progress.killData.clearedSpawners.Contains(ID))
            _slain = true;
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
        _slain = true;
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        if (_slain)
            progress.killData.clearedSpawners.Add(ID);
    }
}
}