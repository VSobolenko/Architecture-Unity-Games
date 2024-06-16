using System;
using Infrastructure.Services;
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

    private void EnterLoadLevel() => _stateMachine.Enter<LoadLevelState, string>(Payload);

    private void RegisterServices()
    {
        _services.RegisterSingle<IInputService>(InputService());
        _services.RegisterSingle<IAssets>(new AssetProvider());
        var asset = _services.Single<IAssets>();
        _services.RegisterSingle<IGameFactory>(new GameFactory(asset));
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