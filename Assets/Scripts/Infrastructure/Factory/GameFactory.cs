using UnityEngine;

namespace Infrastructure
{
public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;

    public GameFactory(IAssets assets)
    {
        _assets = assets;
    }

    public GameObject CreateHero(GameObject at) => _assets.Instantiate(AssetPath.HeroPath, at.transform.position);

    public void CreateHub() => _assets.Instantiate(AssetPath.HudPath);
}
}