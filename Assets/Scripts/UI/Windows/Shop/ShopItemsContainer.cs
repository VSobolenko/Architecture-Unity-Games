using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace UI
{
internal class ShopItemsContainer : MonoBehaviour
{
    public GameObject[] shopUnavailableObjects;
    public Transform parent;
    private IIAPService _iapService;
    private IPersistentProgressService _progressService;
    private IAssets _assets;
    private readonly List<ShopItem> _shopItems = new();

    public void Construct(IIAPService iapService, IPersistentProgressService progressService, IAssets assets)
    {
        _iapService = iapService;
        _progressService = progressService;
        _assets = assets;
    }

    public void Initialize()
    {
        RefreshAvailableItems();
    }

    public void Subscribe()
    {
        _iapService.Initialized += RefreshAvailableItems;
        _progressService.Progress.purchaseData.Changed += RefreshAvailableItems;
    }

    public void Cleanup()
    {
        _iapService.Initialized -= RefreshAvailableItems;
        _progressService.Progress.purchaseData.Changed -= RefreshAvailableItems;
    }

    private async void RefreshAvailableItems()
    {
        UpdateUnavailableObjects();

        if (!_iapService.IsInitialized)
            return;

        ClearShopItems();

        await FillShopItems();
    }

    private void ClearShopItems()
    {
        foreach (var shopItem in _shopItems)
        {
            Destroy(shopItem.gameObject);
        }
    }

    private async Task FillShopItems()
    {
        foreach (var productDescription in _iapService.Products())
        {
            var shopItemObject = await _assets.Instantiate(AssetAddress.ShopItemPath, parent);
            var shopItem = shopItemObject.GetComponent<ShopItem>();

            shopItem.Construct(_iapService, productDescription, _assets);
            _shopItems.Add(shopItem);
        }
    }

    private void UpdateUnavailableObjects()
    {
        foreach (var shopUnavailableObject in shopUnavailableObjects)
        {
            shopUnavailableObject.SetActive(!_iapService.IsInitialized);
        }
    }
}
}