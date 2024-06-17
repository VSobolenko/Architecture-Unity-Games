using System;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Services.Input;
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
        var asset = _services.RegisterSingle<IAssets>(new AssetProvider());
        var gameFactory = _services.RegisterSingle<IGameFactory>(new GameFactory(asset));
        var persistentProgress = _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
        _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(persistentProgress, gameFactory));
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