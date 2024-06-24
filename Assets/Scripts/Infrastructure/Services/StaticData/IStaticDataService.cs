using Infrastructure.Services;
using UI.Services;

namespace StaticData
{
public interface IStaticDataService : IService
{
    void LoadMonsters();
    MonsterStaticData ForMonster(MonsterTypeId typeId);
    LevelStaticData ForLevel(string sceneKey);
    WindowConfig ForWindow(WindowId windowId);
}
}