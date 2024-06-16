using CameraLogic;
using CodeBase.Logic;
using UnityEngine;

namespace Infrastructure
{
public class LoadLevelState : IPayloadedState<string>
{
    private const string InitialPointTag = "InitialPoint";
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _loadingCurtain = loadingCurtain;
        _gameFactory = gameFactory;
    }

    public void Enter(string sceneName)
    {
        _loadingCurtain.Show();
        _sceneLoader.Load(sceneName, onLoaded: OnLoaded);
    }

    public void Exit() => _loadingCurtain.Hide();

    private void OnLoaded()
    {
        var initialPoint = GameObject.FindWithTag(tag: InitialPointTag);
        var hero = _gameFactory.CreateHero(at: initialPoint);
        _gameFactory.CreateHub();
        CameraFollow(hero);
        _gameStateMachine.Enter<GameLoopState>();
    }

    private static void CameraFollow(GameObject hero)
    {
        Camera.main.GetComponent<CameraFollow>().Follow(hero);
    }
}
}