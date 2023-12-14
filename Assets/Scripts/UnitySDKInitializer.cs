using System;

using UnityEngine;

using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Economy.Model;
using Unity.Services.Economy;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Unity.VisualScripting.Antlr3.Runtime;

public class UnitySDKInitializer : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private EconomySDKInitializer economySDKInitializer = null;
    [SerializeField] private AdvertisementSDKInitializer advertisementSDKInitializer = null;
    [SerializeField] private PersistentView persistentView = null;
    #endregion

    #region UNITY_CALLS
    async void Awake()
    {
        try
        {
#if !UNITY_EDITOR
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            await UnityServices.InitializeAsync();
            PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
#else
            await UnityServices.InitializeAsync();
            await SignInAnonymously();
#endif

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
            FileController.WriteFile($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            FileController.WriteFile($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
            FileController.WriteFile(err.ToString());
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
            FileController.WriteFile("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
            FileController.WriteFile("Player session could not be refreshed and expired.");
        };
    }

    private async Task SignInAnonymously()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            await economySDKInitializer.Init();
            advertisementSDKInitializer.Init();
            persistentView.initialized = true;
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"{ex.Message}");
            FileController.WriteFile($"{ex.Message}");
            throw;
        }
        catch (RequestFailedException requestFailed)
        {
            Debug.LogError($"{requestFailed.ErrorCode}");
            FileController.WriteFile($"{requestFailed.ErrorCode}");
            throw;
        }
    }

    private async Task AuthenticateWithUnity(string token)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(token);
            await economySDKInitializer.Init();
            advertisementSDKInitializer.Init();
            persistentView.initialized = true;
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
            FileController.WriteFile(ex.ToString());
            throw;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            FileController.WriteFile(ex.ToString());
            throw;
        }
    }

    private void OnSignInResult(SignInStatus signInStatus)
    {
        if (signInStatus == SignInStatus.Success)
        {
            Debug.Log("Initialized game services");
            PlayGamesPlatform.Instance.RequestServerSideAccess(true,
                (code) =>
                {
                    Debug.Log("Auth code is: " + code);
                    FileController.WriteFile("Auth code is: " + code);
                    AuthenticateWithUnity(code);
                });
        }
        else
        {
            Debug.LogError("cant initialized game services reason: " + signInStatus);
            FileController.WriteFile("cant initialized game services reason: " + signInStatus);
        }
    }
    #endregion
}
