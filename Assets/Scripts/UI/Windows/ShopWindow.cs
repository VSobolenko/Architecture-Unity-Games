using TMPro;

namespace UI
{
internal class ShopWindow : WindowBase
{
    public TextMeshProUGUI SkullText;

    protected override void Initialize() => 
        RefreshSkullTextText();

    protected override void SubscribeUpdates() => 
        Progress.worldData.lootData.changed += SubscribeUpdates;

    protected override void Cleanup()
    {
        base.Cleanup();
        Progress.worldData.lootData.changed -= SubscribeUpdates;
    }

    private string RefreshSkullTextText() => 
        SkullText.text = Progress.worldData.lootData.collected.ToString();
}
}