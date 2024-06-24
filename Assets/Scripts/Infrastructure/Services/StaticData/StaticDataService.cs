using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UI.Services;
using UnityEngine;

namespace StaticData
{
public class StaticDataService : IStaticDataService
{
    private const string StaticDataWindowsPath = "StaticData/Windows/WindowStaticData";
    private const string StaticDataLevelPath = "StaticData";
    private const string StaticDataMonsterPath = "StaticData";
    private Dictionary<MonsterTypeId,MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WindowId, WindowConfig> _windowConfigs;
    
    public void LoadMonsters()
    {
        _monsters = Resources
                    .LoadAll<MonsterStaticData>(StaticDataMonsterPath)
                    .ToDictionary(x => x.monsterTypeId, x => x);
        
        _levels = Resources
                  .LoadAll<LevelStaticData>(StaticDataLevelPath)
                  .ToDictionary(x => x.levelKey, x => x);

        _windowConfigs = Resources
                  .Load<WindowsStaticData>(StaticDataWindowsPath)
                  .configs
                  .ToDictionary(x => x.windowId, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
        _monsters.TryGetValue(typeId, out var staticData) ? staticData : null;

    public LevelStaticData ForLevel(string sceneKey) =>
        _levels.TryGetValue(sceneKey, out var staticData) ? staticData : null;

    public WindowConfig ForWindow(WindowId windowId) =>
        _windowConfigs.TryGetValue(windowId, out var staticData) ? staticData : null;
}
}