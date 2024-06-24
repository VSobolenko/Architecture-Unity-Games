using Infrastructure.Services.Ads;
using Infrastructure.Services.PersistentProgress;
using TMPro;
using UI.Shop;

namespace UI
{
internal class ShopWindow : WindowBase
{
    public TextMeshProUGUI SkullText;
    public RewardedAdItem AdItem;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService)
    {
        base.Construct(progressService);
        AdItem.Construct(adsService, progressService);
    }
    
    protected override void Initialize() => 
        RefreshSkullTextText();

    protected override void SubscribeUpdates()
    {
        AdItem.Subscribe();
        Progress.worldData.lootData.changed += SubscribeUpdates;
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        AdItem.Cleanup();
        Progress.worldData.lootData.changed -= SubscribeUpdates;
    }

    private string RefreshSkullTextText() => 
        SkullText.text = Progress.worldData.lootData.collected.ToString();
}
}