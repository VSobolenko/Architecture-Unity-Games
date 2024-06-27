using System;
using Infrastructure;
using Infrastructure.Services.Ads;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using TMPro;
using UI.Shop;

namespace UI
{
internal class ShopWindow : WindowBase
{
    public TextMeshProUGUI SkullText;
    public RewardedAdItem AdItem;
    public ShopItemsContainer ShopItemContainer;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService, IAssets assets,
                          IIAPService iapService)
    {
        base.Construct(progressService);
        if (AdItem != null) AdItem.Construct(adsService, progressService);
        ShopItemContainer.Construct(iapService, progressService, assets);
    }

    protected override void Initialize()
    {
        if (AdItem != null) AdItem.Initialize();
        ShopItemContainer.Initialize();
        RefreshSkullTextText();
    }

    protected override void SubscribeUpdates()
    {
        if (AdItem != null) AdItem.Subscribe();
        ShopItemContainer.Subscribe();
        Progress.worldData.lootData.changed += SubscribeUpdates;
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        if (AdItem != null) AdItem.Cleanup();
        ShopItemContainer.Cleanup();
        Progress.worldData.lootData.changed -= SubscribeUpdates;
    }

    private string RefreshSkullTextText() =>
        SkullText.text = Progress.worldData.lootData.collected.ToString();
}
}