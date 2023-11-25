using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

using Unity.Services.Economy;
using Unity.Services.Economy.Model;

public class EconomySDKInitializer : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private PersistentView persistentView = null;
    #endregion

    #region PUBLIC_METHODS
    public async Task Init()
    {
        try
        {
            await EconomyService.Instance.Configuration.SyncConfigurationAsync();
            await GetUserBalance();
            if (PlayerPrefs.GetString("firstLoggin", string.Empty) != "logged")
            {
                PlayersInventoryItem createdInventoryItem = await EconomyService.Instance.PlayerInventory.AddInventoryItemAsync("SLINGER_0");
                PlayerPrefs.SetString("firstLoggin", "logged");
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            FileController.WriteFile(e.ToString());
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private async Task GetUserBalance()
    {
        GetBalancesOptions options = new GetBalancesOptions
        {
            ItemsPerFetch = 5
        };

        GetBalancesResult getBalancesResult = await EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);
        List<PlayerBalance> firstFiveBalances = getBalancesResult.Balances;

        // do something with your balances

        if (getBalancesResult.HasNext)
        {
            getBalancesResult = await getBalancesResult.GetNextAsync(options.ItemsPerFetch);
            List<PlayerBalance> nextFiveBalances = getBalancesResult.Balances;
            nextFiveBalances.AddRange(firstFiveBalances);
            persistentView.Configure(nextFiveBalances);
            // do something with your balances
        }
        else
        {
            persistentView.Configure(firstFiveBalances);
        }
    }
    #endregion
}
