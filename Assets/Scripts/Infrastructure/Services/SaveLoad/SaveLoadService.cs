using Data;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure
{
internal class SaveLoadService : ISaveLoadService
{
    private const string ProgressKey = "Progress";
    
    private readonly IPersistentProgressService _persistentProgressService;
    private readonly IGameFactory _gameFactory;

    public SaveLoadService(IPersistentProgressService persistentProgressService, IGameFactory gameFactory)
    {
        _persistentProgressService = persistentProgressService;
        _gameFactory = gameFactory;
    }

    public void SaveProgress()
    {
        foreach (var progressWriter in _gameFactory.ProgressWriters)
        {
            progressWriter.UpdateProgress(_persistentProgressService.Progress);
        }
        
        PlayerPrefs.SetString(ProgressKey, _persistentProgressService.Progress.ToJson());
    }

    public PlayerProgress LoadProgress()
    {
        return PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<PlayerProgress>();
    }
}
}