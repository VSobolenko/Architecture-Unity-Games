using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StaticData
{
public class StaticDataService : IStaticDataService
{
    private Dictionary<MonsterTypeId,MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;
    
    public void LoadMonsters()
    {
        _monsters = Resources
                    .LoadAll<MonsterStaticData>("StaticData")
                    .ToDictionary(x => x.monsterTypeId, x => x);
        
        _levels = Resources
                  .LoadAll<LevelStaticData>("StaticData")
                  .ToDictionary(x => x.levelKey, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
        _monsters.TryGetValue(typeId, out var staticData) ? staticData : null;

    public LevelStaticData ForLevel(string sceneKey) =>
        _levels.TryGetValue(sceneKey, out var staticData) ? staticData : null;
}
}