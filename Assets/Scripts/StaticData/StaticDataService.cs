using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StaticData
{
public class StaticDataService : IStaticDataService
{
    private Dictionary<MonsterTypeId,MonsterStaticData> _monsters;

    public void LoadMonsters()
    {
        _monsters = Resources
                    .LoadAll<MonsterStaticData>("StaticData")
                    .ToDictionary(x => x.monsterTypeId, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
        _monsters.TryGetValue(typeId, out var staticData) ? staticData : null;
}
}