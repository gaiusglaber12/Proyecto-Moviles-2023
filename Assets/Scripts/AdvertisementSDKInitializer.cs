using UnityEngine;
using UnityEngine.Advertisements;

public class AdvertisementSDKInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    #region EXPOSED_FIELDS
    [SerializeField] private RewardedAdsButton rewardedAdsButton = null;
    [SerializeField] private InterstitialAdExample interstitialAdExample = null;
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    #endregion

    #region PRIVATE_FIELDS
    private string _gameId;
    #endregion

    #region PUBLIC_METHODS
    public void Init()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        FileController.WriteFile("Unity Ads initialization complete.");
        rewardedAdsButton.LoadAd();
        interstitialAdExample.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        FileController.WriteFile($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    #endregion
}
