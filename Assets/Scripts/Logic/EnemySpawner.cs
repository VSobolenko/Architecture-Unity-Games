using System;
using Data;
using Hero;
using StaticData;
using UnityEngine;

namespace Logic
{
public class EnemySpawner : MonoBehaviour, ISavedProgress
{
    public MonsterTypeId monsterTypeId;
    private string _id;

    public bool slain;
    private void Awake()
    {
        _id = GetComponent<UniqueId>().id;
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
        
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        if (slain) 
            progress.killData.clearedSpawners.Add(_id);
    }
}
}