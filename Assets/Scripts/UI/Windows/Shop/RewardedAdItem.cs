using Infrastructure.Services.Ads;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
public class RewardedAdItem : MonoBehaviour
{
    public Button ShowAdButton;
    public GameObject[] AdActiveObjects;
    public GameObject[] AdInactiveObjects;

    private IAdsService _adsService;
    private IPersistentProgressService _progressService;

    public void Construct(IAdsService adsService, IPersistentProgressService progresService)
    {
        _adsService = adsService;
        _progressService = progresService;
    }

    public void Initialize()
    {
        ShowAdButton.onClick.AddListener(OnShowAdClicked);
        RefreshAvailableAd();
    }

    public void Subscribe() =>
        _adsService.RewardedVideoReady += RefreshAvailableAd;

    public void Cleanup() =>
        _adsService.RewardedVideoReady -= RefreshAvailableAd;

    private void OnShowAdClicked() =>
        _adsService.ShowRewardedVideo(OnVideoFinished);

    private void OnVideoFinished() =>
        _progressService.Progress.worldData.lootData.Add(_adsService.Reward);

    private void RefreshAvailableAd()
    {
        bool isVideoReady = _adsService.IsRewardedVideoReady;

        foreach (GameObject adActiveObject in AdActiveObjects)
            adActiveObject.SetActive(isVideoReady);

        foreach (GameObject adInactiveObject in AdInactiveObjects)
            adInactiveObject.SetActive(!isVideoReady);
    }
}
}