using CodeBase.Logic;
using UnityEngine;

namespace Infrastructure
{
public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
{
    public LoadingCurtain curtainPrefab;
    private Game _game;

    private void Awake()
    {
        _game = new Game(this, Instantiate(curtainPrefab));
        _game.stateMachine.Enter<BootstrapState>();
        DontDestroyOnLoad(this);
    }
}
}