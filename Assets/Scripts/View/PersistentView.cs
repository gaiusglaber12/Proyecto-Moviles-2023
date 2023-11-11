using System.Collections.Generic;

using UnityEngine;

using Unity.Services.Economy.Model;
using Unity.Services.Economy;
using System.Threading.Tasks;

public class PersistentView : MonoBehaviour
{
    public static PersistentView Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #region EXPOSED_FIELDS
    [SerializeField] private List<CurrencyView> currencyViews = null;
    #endregion

    #region PRIVATE_FIELDS}
    private List<PlayerBalance> playerBalances = null;
    #endregion

    #region PUBLIC_METHODS
    public void Configure(List<PlayerBalance> playerBalance)
    {
        for (int i = 0; i < currencyViews.Count; i++)
        {
            for (int j = 0; j < playerBalance.Count; j++)
            {
                if (currencyViews[i].CurrencyId == playerBalance[j].CurrencyId)
                {
                    currencyViews[i].Init(playerBalance[j].Balance);
                }
            }
        }
        Debug.Log("PersistentView initialized");
    }

    public async Task UpdateBalance()
    {
        GetBalancesOptions options = new GetBalancesOptions
        {
            ItemsPerFetch = 5
        };

        GetBalancesResult getBalancesResult = await EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);
        playerBalances = getBalancesResult.Balances;

        Configure(playerBalances);
    }

    public long GetActualCurrencyById(string id)
    {
        for (int i = 0; i < currencyViews.Count; i++)
        {
            for (int j = 0; j < playerBalances.Count; j++)
            {
                if (currencyViews[i].CurrencyId == playerBalances[j].CurrencyId)
                {
                    return playerBalances[j].Balance;
                }
            }
        }
        return 0;
    }
    #endregion
}
