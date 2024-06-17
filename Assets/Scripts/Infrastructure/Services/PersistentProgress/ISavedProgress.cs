using Data;

namespace Hero
{
public interface ISavedProgressReader
{
    void LoadProgress(PlayerProgress progress);
}

public interface ISavedProgress : ISavedProgressReader
{
    void UpdateProgress(PlayerProgress progress);
}
}