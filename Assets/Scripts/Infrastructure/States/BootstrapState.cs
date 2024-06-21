using System;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using Services.Input;
using StaticData;
using UnityEngine;

namespace Infrastructure
{
public class BootstrapState : IState
{
    private const string Initial = "Initial";
    private const string Payload = "Main";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _services = services;
        
        RegisterServices();
    }

    public void Enter()
    {
        _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
    }

    private void EnterLoadLevel() => _stateMachine.Enter<LoadProgressState>();

    private void RegisterServices()
    {
        _services.RegisterSingle<IInputService>(InputService());
        var randomService = _services.RegisterSingle<IRandomService>(new RandomService());
        var staticDataService = RegisterStaticData();
        var asset = _services.RegisterSingle<IAssets>(new AssetProvider());
        var persistentProgress = _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
        var gameFactory = _services.RegisterSingle<IGameFactory>(new GameFactory(asset, staticDataService, randomService, persistentProgress));
        _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(persistentProgress, gameFactory));
    }

    private IStaticDataService RegisterStaticData()
    {
        var staticDataService = _services.RegisterSingle<IStaticDataService>(new StaticDataService());
        staticDataService.LoadMonsters();

        return staticDataService;
    }

    public void Exit()
    {
        
    }   
    
    private static IInputService InputService()
    {
        if (Application.isEditor)
            return new StandaloneInputService();
        else
            return new MobileInputService();
    }
}
}