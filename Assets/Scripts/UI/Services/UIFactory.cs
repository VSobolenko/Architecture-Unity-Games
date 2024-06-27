using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Services.Ads;
using Infrastructure.Services.IAP;
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
    private readonly IIAPService _iapService;
    private readonly IAdsService _adsService;

    public UIFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService, IIAPService iapService, IAdsService adsService)
    {
        _assets = assets;
        _staticData = staticData;
        _progressService = progressService;
        _iapService = iapService;
        _adsService = adsService;
    }

    public GameObject CreateShop()
    {
        var config = _staticData.ForWindow(WindowId.Shop);
        var shop = Object.Instantiate(config.prefab, _uiRoot) as ShopWindow;
        shop.Construct(_adsService, _progressService, _assets, _iapService);
        return shop.gameObject;
    }

    public async Task CreateUIRoot()
    {
        var root = await _assets.Instantiate(AssetAddress.UIRootPath);
        _uiRoot = root.transform;
    }
}
}