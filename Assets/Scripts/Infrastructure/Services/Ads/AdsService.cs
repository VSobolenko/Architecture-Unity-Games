using System;
using UnityEngine;

namespace Infrastructure.Services.Ads
{
public class AdsService : IAdsService
  {
    private const string AndroidGameId = "4106937";
    private const string IOSGameId = "4106936";

    private const string UnityRewardedVideoIdAndroid = "Rewarded_Android";
    private const string UnityRewardedVideoIdIOS = "Rewarded_iOS";

    private string _placementId;
    
    private Action _onVideoFinished;

    public event Action RewardedVideoReady;

    public int Reward => 15;

    public void Initialize()
    {
      SetIdsForCurrentPlatform();
      //Advertisement.AddListener(this);
      //Advertisement.Initialize(_gameId);
    }

    public void ShowRewardedVideo(Action onVideoFinished)
    {
      _onVideoFinished = onVideoFinished;
      //Advertisement.Show(_placementId);
    }

    public bool IsRewardedVideoReady => true; //Advertisement.IsReady(_placementId);

    public void OnUnityAdsReady(string placementId)
    {
      Debug.Log($"OnUnityAdsReady {placementId}");

      if (placementId == _placementId) 
        RewardedVideoReady?.Invoke();
    }

    public void OnUnityAdsDidError(string message) => 
      Debug.Log($"OnUnityAdsDidError {message}");

    public void OnUnityAdsDidStart(string placementId) => 
      Debug.Log($"OnUnityAdsDidStart {placementId}");

    public void OnUnityAdsDidFinish(string placementId, bool showResult)
    {
      switch (showResult)
      {
          case false:
          Debug.LogError($"OnUnityAdsDidFinish {showResult}");
          break;
        case true:
          _onVideoFinished?.Invoke();
          break;
      }

      _onVideoFinished = null;
    }

    private void SetIdsForCurrentPlatform()
    {
      switch (Application.platform)
      {
        case RuntimePlatform.Android:
          //_gameId = AndroidGameId;
          _placementId = UnityRewardedVideoIdAndroid;
          break;

        case RuntimePlatform.IPhonePlayer:
          //_gameId = IOSGameId;
          _placementId = UnityRewardedVideoIdIOS;
          break;

        case RuntimePlatform.WindowsEditor:
          //_gameId = IOSGameId;
          _placementId = UnityRewardedVideoIdIOS;
          break;

        default:
          Debug.Log("Unsupported platform for ads.");
          break;
      }
    }
  }
}