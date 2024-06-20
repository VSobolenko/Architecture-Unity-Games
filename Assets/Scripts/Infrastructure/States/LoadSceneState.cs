using System;
using CameraLogic;
using CodeBase.Logic;
using Hero;
using Infrastructure.Services.PersistentProgress;
using Logic;
using UI;
using UnityEngine;

namespace Infrastructure
{
public class LoadSceneState : IPayloadedState<string>
{
    private const string InitialPointTag = "InitialPoint";
    private const string EnemySpawnerTag = "EnemySpawner";
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _persistentProgressService;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
                          IGameFactory gameFactory, IPersistentProgressService persistentProgressService)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _loadingCurtain = loadingCurtain;
        _gameFactory = gameFactory;
        _persistentProgressService = persistentProgressService;
    }

    public void Enter(string sceneName)
    {
        _loadingCurtain.Show();
        _gameFactory.Cleanup();
        _sceneLoader.Load(sceneName, onLoaded: OnLoaded);
    }

    public void Exit() => _loadingCurtain.Hide();

    private void OnLoaded()
    {
        InitGameWorld();
        InformProgressReaders();
        _gameStateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders()
    {
        foreach (var progressReader in _gameFactory.ProgressReaders)
        {
            progressReader.LoadProgress(_persistentProgressService.Progress);
        }
    }

    private void InitGameWorld()
    {
        InitSpawners();
        var hero = InitHero();
        InitHud(hero);
        CameraFollow(hero);
    }

    private void InitSpawners()
    {
        foreach (var spawnerGO in GameObject.FindGameObjectsWithTag(EnemySpawnerTag))
        {
            var spawner = spawnerGO.GetComponent<EnemySpawner>();
            _gameFactory.Register(spawner);
        }
    }

    private GameObject InitHero()
    {
        var initialPoint = GameObject.FindWithTag(tag: InitialPointTag);
        var hero = _gameFactory.CreateHero(at: initialPoint);

        return hero;
    }

    private void InitHud(GameObject hero)
    {
        var hud = _gameFactory.CreateHud();
        hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }

    private static void CameraFollow(GameObject hero)
    {
        Camera.main.GetComponent<CameraFollow>().Follow(hero);
    }
}
}