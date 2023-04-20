using CodeBase.Logic;
using Services.Input;
using UnityEngine;

namespace Infrastructure
{
internal class Game
{
    public static IInputService inputService;
    public readonly GameStateMachine stateMachine;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
    {
        stateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain);
    }
}
}