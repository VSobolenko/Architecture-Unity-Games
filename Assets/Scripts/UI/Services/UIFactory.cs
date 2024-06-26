using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UnityEngine;

namespace UI.Services
{
internal class UIFactory : IUIFactory
{
    private readonly IAssets _assets;
    private readonly IStaticDataService _staticData;
    private Transform _uiRoot;
    private readonly IPersistentProgressService _progressService;

    public UIFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService)
    {
        _assets = assets;
        _staticData = staticData;
        _progressService = progressService;
    }

    public GameObject CreateShop()
    {
        var config = _staticData.ForWindow(WindowId.Shop);
        var shop = Object.Instantiate(config.prefab, _uiRoot);
        shop.Construct(_progressService);
        return shop.gameObject;
    }

    public async Task CreateUIRoot()
    {
        var root = await _assets.Instantiate(AssetAddress.UIRootPath);
        _uiRoot = root.transform;
    }
}
}