using System;
using Data;
using Infrastructure.Services.PersistentProgress;

namespace Infrastructure
{
public class LoadProgressState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress)
    {
        _gameStateMachine = gameStateMachine;
        _progressService = progressService;
        _saveLoadProgress = saveLoadProgress;
    }

    public void Enter()
    {
        LoadProgressOnInit();
        _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.positionOnLevel.level);
    }

    public void Exit()
    {
        
    }

    private void LoadProgressOnInit()
    {
        _progressService.Progress = _saveLoadProgress.LoadProgress() ?? NewProgress();
    }

    private PlayerProgress NewProgress()
    {
        return new PlayerProgress("Main");
    }
}
}