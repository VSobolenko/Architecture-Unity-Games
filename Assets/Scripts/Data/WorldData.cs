using System;
using UnityEngine;

namespace Data
{
[Serializable]
public class WorldData
{
    public PositionOnLevel positionOnLevel;
    public LootData lootData;

    public WorldData(string initLevel)
    {
        positionOnLevel = new PositionOnLevel(initLevel);
        lootData = new LootData();
    }
}
}