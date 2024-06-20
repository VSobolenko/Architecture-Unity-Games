using System;

namespace Data
{
[Serializable]
public class PlayerProgress
{
    public State heroState;
    public WorldData worldData;
    public Stats heroStats;

    public PlayerProgress(string initLevel)
    {
        worldData = new WorldData(initLevel);
        heroState = new State();
        heroStats = new Stats();
    }
}
}