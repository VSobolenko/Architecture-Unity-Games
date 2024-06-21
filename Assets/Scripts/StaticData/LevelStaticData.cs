using System.Collections.Generic;
using Logic;
using UnityEngine;

namespace StaticData
{
[CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
public class LevelStaticData : ScriptableObject
{
    public string levelKey;

    public List<EnemySpawnerData> enemySpawners;
}
}