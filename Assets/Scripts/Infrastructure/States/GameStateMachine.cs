using System;
using System.Collections.Generic;
using CodeBase.Logic;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UnityEngine;

namespace Infrastructure
{
public class GameStateMachine
{
    private Dictionary<Type, IExitable> _states;
    private IExitable _activeState;

    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
    {
        _states = new Dictionary<Type, IExitable>
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
            [typeof(LoadSceneState)] =
                new LoadSceneState(this, sceneLoader, loadingCurtain, services.Single<IGameFactory>(),
                                   services.Single<IPersistentProgressService>(), services.Single<IStaticDataService>()),
            [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IPersistentProgressService>(),
                                                                services.Single<ISaveLoadService>()),
            [typeof(GameLoopState)] = new GameLoopState(this),
        };
    }

    public void Enter<TState>() where TState : class, IState
    {
        var state = ChangeState<TState>();
        state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
        var state = ChangeState<TState>();
        state.Enter(payload);
    }

    private TState ChangeState<TState>() where TState : class, IExitable
    {
        _activeState?.Exit();
        
        var state = GetState<TState>();
        _activeState = state;

        return state;
    }

    private TState GetState<TState>() where TState : class, IExitable
    {
        return _states[typeof(TState)] as TState;
    }
}
}