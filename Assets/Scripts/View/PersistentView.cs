using System.Collections.Generic;

using UnityEngine;

using Unity.Services.Economy.Model;
using Unity.Services.Economy;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections;
using System;

public class PersistentView : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject currenciesHolder = null;
    [SerializeField] private List<CurrencyView> currencyViews = null;
    [SerializeField] private Scrollbar loadingBar = null;
    [SerializeField] private GameObject barHolder = null;
    [SerializeField] private CanvasGroup backgroundImg = null;
    #endregion

    #region PRIVATE_FIELDS}
    private List<PlayerBalance> playerBalances = null;
    #endregion

    #region UNITY_CALLS
    public static PersistentView Instance { get; private set; }
    public static int CurrLevel = 0;
    public static string CurrStringDificulty = string.Empty;
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
        FileController.WriteFile("PersistentView initialized");
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

    public void ToggleView(bool toggle)
    {
        currenciesHolder.SetActive(toggle);
    }

    public LevelConfigSO.DIFICULTY GetCurrentDificultyEnum()
    {
        switch (CurrStringDificulty)
        {
            case "EASY":
                return LevelConfigSO.DIFICULTY.EASY;
            case "NORMAL":
                return LevelConfigSO.DIFICULTY.NORMAL;
            case "HARD":
                return LevelConfigSO.DIFICULTY.HARD;
            default:
                return LevelConfigSO.DIFICULTY.EASY;
        }
    }
    #endregion
}
