using System;
using CameraLogic;
using CodeBase.Logic;
using Hero;
using Infrastructure.Services.PersistentProgress;
using Logic;
using StaticData;
using UI;
using UI.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
public class LoadLevelState : IPayloadedState<string>
{
    private const string InitialPointTag = "InitialPoint";
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _persistentProgressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
                          IGameFactory gameFactory, IPersistentProgressService persistentProgressService, IStaticDataService staticData, IUIFactory uiFactory)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _loadingCurtain = loadingCurtain;
        _gameFactory = gameFactory;
        _persistentProgressService = persistentProgressService;
        _staticData = staticData;
        _uiFactory = uiFactory;
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
        InitUIRoot();   
        InitGameWorld();
        InformProgressReaders();
        _gameStateMachine.Enter<GameLoopState>();
    }

    private void InitUIRoot()
    {
        _uiFactory.CreateUIRoot();
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
        string sceneKey = SceneManager.GetActiveScene().name;
        var levelData = _staticData.ForLevel(sceneKey);
        foreach (var spawnerData in levelData.enemySpawners)
        {
            _gameFactory.CreateSpawner(spawnerData.position, spawnerData.id, spawnerData.monsterTypeId);
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