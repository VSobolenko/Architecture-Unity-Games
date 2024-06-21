using Data;
using Infrastructure;
using Infrastructure.Services.Randomizer;
using UnityEngine;

namespace Enemy
{
public class LootSpawner : MonoBehaviour
{
    public EnemyDeath enemyDeath;
    private IGameFactory _factory;
    private int _lootMin;
    private int _lootMax;
    private IRandomService _randomService;

    public void Construct(IGameFactory factory, IRandomService randomService)
    {
        _randomService = randomService;
        _factory = factory;
    }
    
    private void Start()
    {
        enemyDeath.Happened += SpawnLoot;
    }

    private void SpawnLoot()
    {
        LootPiece loot = _factory.CreateLoot();
        loot.transform.position = transform.position;
        
        var lootItem = GenerateLoot();
        loot.Initialize(lootItem);
    }

    private Loot GenerateLoot()
    {
        return new Loot()
        {
            value = _randomService.Next(_lootMin, _lootMax),
        };
    }

    public void SetLoot(int min, int max)
    {
        _lootMin = min;
        _lootMax = max;
    }
}
}