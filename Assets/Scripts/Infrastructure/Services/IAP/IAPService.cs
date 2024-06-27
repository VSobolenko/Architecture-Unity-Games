using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace Infrastructure.Services.IAP
{
public class IAPService : IIAPService
{
    private readonly IAPProvider _iapProvider;
    private readonly IPersistentProgressService _progressService;

    public bool IsInitialized => _iapProvider.IsInitialize;

    public event Action Initialized;

    public IAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
    {
        _iapProvider = iapProvider;
        _progressService = progressService;
    }

    public void Initialize()
    {
        _iapProvider.Initialize(this);
        _iapProvider.Initialized += () => Initialized?.Invoke();
    }

    public List<ProductDescription> Products()
    {
        return ProductsDescriptions().ToList();
    }

    private IEnumerable<ProductDescription> ProductsDescriptions()
    {
        var purchaseData = _progressService.Progress.purchaseData;
        foreach (var productId in _iapProvider.Products.Keys)
        {
            var config = _iapProvider.Configs[productId];
            var product = _iapProvider.Products[productId];

            var boughtIAP = purchaseData.boughtIAPs.Find(x => x.productId == productId);
            if (ProductBought(boughtIAP, config))
                continue;

            yield return new ProductDescription
            {
                id = productId,
                config = config,
                product = product,
                availablePurchasesLeft =
                    boughtIAP != null ? config.maxPurchaseCount - boughtIAP.count : config.maxPurchaseCount,
            };
        }
    }

    private static bool ProductBought(BoughtIAP boughtIAP, ProductConfig config) => boughtIAP != null && boughtIAP.count >= config.maxPurchaseCount;

    public void StartPurchase(string productId) => _iapProvider.StartPurchase(productId);

    public PurchaseProcessingResult ProcessPurchase(Product purchaseProduct)
    {
        var productConfig = _iapProvider.Configs[purchaseProduct.definition.id];
        switch (productConfig.itemType)
        {
            case ItemType.None:
                throw new ArgumentOutOfRangeException();
                break;
            case ItemType.Skulls:
                _progressService.Progress.worldData.lootData.Add(productConfig.quantity);
                _progressService.Progress.purchaseData.AddPurchase(purchaseProduct.definition.id);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return PurchaseProcessingResult.Complete;
    }
}
}