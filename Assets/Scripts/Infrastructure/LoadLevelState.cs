using CameraLogic;
using CodeBase.Logic;
using UnityEngine;

namespace Infrastructure
{
public class LoadLevelState : IPayloadedState<string>
{
    private const string InitialPointTag = "InitialPoint";
    private const string HeroPath = "Hero";
    private const string HudPath = "Hud";
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _loadingCurtain = loadingCurtain;
    }

    public void Enter(string sceneName)
    {
        _loadingCurtain.Show();
        _sceneLoader.Load(sceneName, onLoaded: OnLoaded);
    }

    public void Exit() => _loadingCurtain.Hide();

    private void OnLoaded()
    {
        var initialPoint = GameObject.FindWithTag(InitialPointTag);
        var hero = Instantiate(HeroPath, initialPoint.transform.position);
        Instantiate(HudPath);
        CameraFollow(hero);
        _gameStateMachine.Enter<GameLoopState>();
    }
    
    private static void CameraFollow(GameObject hero)
    {
        Camera.main.GetComponent<CameraFollow>().Follow(hero);
    }

    private static GameObject Instantiate(string path)
    {
        var prefab = Resources.Load<GameObject>(path);
        return Object.Instantiate(prefab);
    }
    
    private static GameObject Instantiate(string path, Vector3 at)
    {
        var prefab = Resources.Load<GameObject>(path);
        return Object.Instantiate(prefab, at, Quaternion.identity);
    }
}
}