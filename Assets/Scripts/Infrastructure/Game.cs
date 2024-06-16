using CodeBase.Logic;
using Infrastructure.Services;
using Services.Input;
using UnityEngine;

namespace Infrastructure
{
internal class Game
{
    public readonly GameStateMachine stateMachine;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
    {
        stateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain, AllServices.Container);
    }
}
}