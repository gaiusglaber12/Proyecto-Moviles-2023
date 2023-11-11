using System;

using UnityEngine;

using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using static UnityEditor.Progress;
using Unity.Services.Economy.Model;
using Unity.Services.Economy;

public class UnitySDKInitializer : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private EconomySDKInitializer economySDKInitializer = null;
    [SerializeField] private AdvertisementSDKInitializer advertisementSDKInitializer = null;
    #endregion

    #region UNITY_CALLS
    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            SetupEvents();

            await SignInAnonymously();
            await economySDKInitializer.Init();
            advertisementSDKInitializer.Init();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    #endregion

    #region PRIVATE_METHODS
    // Setup authentication event handlers if desired
    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }

    private async Task SignInAnonymously()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"{ex.Message}");
        }
        catch (RequestFailedException requestFailed)
        {
            Debug.LogError($"{requestFailed.ErrorCode}");
        }

        if (PlayerPrefs.GetString("firstLoggin", string.Empty) != "falopa")
        {
            PlayersInventoryItem createdInventoryItem = await EconomyService.Instance.PlayerInventory.AddInventoryItemAsync("SLINGER_0");
            PlayerPrefs.SetString("firstLoggin", "true");
        }
    }
    #endregion
}
