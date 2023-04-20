using System;
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

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
    }

    public void Enter()
    {
        RegisterServices();
        _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
    }

    private void EnterLoadLevel() => _stateMachine.Enter<LoadLevelState, string>(Payload);

    private void RegisterServices()
    {
        Game.inputService = RegisterInputService();
    }

    public void Exit()
    {
        
    }
    
    private static IInputService RegisterInputService()
    {
        if (Application.isEditor)
            return new StandaloneInputService();
        else
            return new MobileInputService();
    }
}
}