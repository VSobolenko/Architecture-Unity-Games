using UnityEngine;

namespace StaticData
{
[CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
public class MonsterStaticData : ScriptableObject
{
    public MonsterTypeId monsterTypeId;

    [Range(1, 100)] public int hp;

    [Range(1, 30)] public int damage;

    public int maxLoot;
    public int minLoot;
    
    [Range(0.5f, 1f)] public float effectiveDistance = 0.6f;

    [Range(0.5f, 1f)] public float cleavage;

    [Range(0f, 10f)] public float moveSpeed = 4f;

    public GameObject prefab;
}
}