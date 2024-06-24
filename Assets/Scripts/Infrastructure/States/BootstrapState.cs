using System;
using Infrastructure.Services;
using Infrastructure.Services.Ads;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using Services.Input;
using StaticData;
using UI.Services;
using UnityEngine;

namespace Infrastructure
{
public class BootstrapState : IState
{
    private const string Initial = "Initial";
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
        var ads = _services.RegisterSingle<IAdsService>(new AdsService());
        ads.Initialize();
        var staticDataService = RegisterStaticData();
        var asset = _services.RegisterSingle<IAssets>(new AssetProvider());
        var persistentProgress = _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
        var uiFactory = _services.RegisterSingle<IUIFactory>(new UIFactory(asset, staticDataService, persistentProgress));
        var windowService = _services.RegisterSingle<IWindowsService>(new WindowsService(uiFactory));
        var gameFactory = _services.RegisterSingle<IGameFactory>(new GameFactory(asset, staticDataService, randomService, persistentProgress, windowService));
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