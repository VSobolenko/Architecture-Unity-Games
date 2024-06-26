using System;
using Data;
using TMPro;
using UnityEngine;

namespace UI
{
public class LootCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;
    private WorldData _worldData;

    public void Construct(WorldData worldData)
    {
        _worldData = worldData;
        _worldData.lootData.changed += UpdateCounter;

        UpdateCounter();
    }
    
    private void UpdateCounter()
    {
        counter.text = $"{_worldData.lootData.collected}";
    }
}
}