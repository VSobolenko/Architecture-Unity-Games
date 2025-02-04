using System;
using Infrastructure.Services;
using Infrastructure.Services.Ads;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using Services.Input;
using StaticData;
using UI.Services;
using UnityEngine;

namespace Infrastructure
{
public class BootstrapState : IState
{
    private const string Initial = "Initial";
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

    private void EnterLoadLevel() => _stateMachine.Enter<LoadProgressState>();

    private void RegisterServices()
    {
        _services.RegisterSingle<IInputService>(InputService());
        _services.RegisterSingle<IGameStateMachine>(_stateMachine);
        var randomService = _services.RegisterSingle<IRandomService>(new RandomService());
        var persistentProgress = _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
        var iapProvider = new IAPProvider();
        var adsService = RegisterAds();
        var iapService = RegisterIAP(persistentProgress, iapProvider);
        var staticDataService = RegisterStaticData();
        var asset = RegisterAssetProvider();
        var uiFactory =
            _services.RegisterSingle<IUIFactory>(
                new UIFactory(asset, staticDataService, persistentProgress, iapService, adsService));
        var windowService = _services.RegisterSingle<IWindowsService>(new WindowsService(uiFactory));
        var gameFactory =
            _services.RegisterSingle<IGameFactory>(
                new GameFactory(asset, staticDataService, randomService, persistentProgress, windowService));
        _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(persistentProgress, gameFactory));
    }

    private IAdsService RegisterAds()
    {
        var ads = _services.RegisterSingle<IAdsService>(new AdsService());
        ads.Initialize();

        return ads;
    }

    private IIAPService RegisterIAP(IPersistentProgressService progressService, IAPProvider iapProvider)
    {
        var iap = _services.RegisterSingle<IIAPService>(new IAPService(iapProvider, progressService));
        iap.Initialize();

        return iap;
    }

    private IAssets RegisterAssetProvider()
    {
        var asset = _services.RegisterSingle<IAssets>(new AssetProvider());
        asset.Initialize();

        return asset;
    }

    private IStaticDataService RegisterStaticData()
    {
        var staticDataService = _services.RegisterSingle<IStaticDataService>(new StaticDataService());
        staticDataService.LoadMonsters();

        return staticDataService;
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