using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Infrastructure.Services.IAP
{
public class IAPProvider : IDetailedStoreListener
{
    private const string IAPConfigsPath = "IAP/ProductConfig";

    private IStoreController _controller;
    private IExtensionProvider _extensions;
    private IAPService _iapService;

    public Dictionary<string, ProductConfig> Configs { get; private set; }
    public Dictionary<string, Product> Products { get; private set; }

    public event Action Initialized;

    public bool IsInitialize => _controller != null && _extensions != null;

    public void Initialize(IAPService iapService)
    {
        _iapService = iapService;
        Configs = new Dictionary<string, ProductConfig>();
        Products = new Dictionary<string, Product>();

        Load();

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (var productConfig in Configs.Values)
            builder.AddProduct(productConfig.id, productConfig.productType);

        UnityPurchasing.Initialize(this, builder);
    }

    public void StartPurchase(string productId)
    {
        _controller.InitiatePurchase(productId);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _controller = controller;
        _extensions = extensions;

        foreach (var product in _controller.products.all)
            Products.Add(product.definition.id, product);

        Initialized?.Invoke();
        Debug.Log("IAP initialized success");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"IAP initialized fail: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"IAP initialized fail: {error}; Message: {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"IAP purchase success: productId={purchaseEvent.purchasedProduct.definition.id}");

        return _iapService.ProcessPurchase(purchaseEvent.purchasedProduct);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"IAP purchase fail: productId={product.definition}; reason={failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log(
            $"IAP purchase fail: productId={product.definition.id}; reason={failureDescription.message}; messId={failureDescription.productId}");
    }

    private void Load() => Configs =
        Resources.Load<TextAsset>(IAPConfigsPath)?.text?.ToDeserialized<ProductConfigWrapper>()?.configs
                 ?.ToDictionary(x => x.id, x => x);
}
}